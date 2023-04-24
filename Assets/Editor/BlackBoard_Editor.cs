using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Reflection;
using System.Collections.Generic;

public class BlackBoard_Editor : EditorWindow
{
    // Stored required properties
    private BlackBoard blackBoard;

    // Stored required elements.
    private ToolbarButton toolbarNewKey;
    private VisualElement keyList;

    private BlackBoardKeyType selectedKey;
    private VisualElement selectedElement;
    private ListEntry selectedListEntry;
    private Label keyType;

    public TextField nameTextField;
    public TextField descriptionTextField;

    public SerializedBlackBoard serializer;

    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    // [MenuItem("Window/UI Toolkit/BlackBoard_Editor")]
    public static void OpenWindow()
    {
        BlackBoard_Editor wnd = GetWindow<BlackBoard_Editor>();
        wnd.titleContent = new GUIContent("BlackBoard_Editor");
        wnd.Show();
        var blackBoard = Selection.activeObject as BlackBoard;
        wnd.SelectBlackBoard(blackBoard);
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BlackBoard)
        {
            OpenWindow();
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                EditorApplication.delayCall += OnSelectionChange;
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                EditorApplication.delayCall += OnSelectionChange;
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
            default:
                break;
        }
    }

    private void OnSelectionChange()
    {
        if (Selection.activeObject is BlackBoard activeBlackBoard)
        {
            blackBoard = activeBlackBoard;

            UpdateSelection(blackBoard);
        }
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BlackBoard_Editor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BlackBoard_Editor.uss");
        root.styleSheets.Add(styleSheet);

        toolbarNewKey = root.Q<ToolbarButton>("toolbar-NewKey");
        nameTextField = root.Q<TextField>("key-detail-name");
        descriptionTextField = root.Q<TextField>("key-detail-description");
        keyType = root.Q<Label>("keyType");
        keyList = root.Q("KeyList");

        toolbarNewKey.clicked -= ShowKeyCreationWindow;// TODO : 추후 기능 구현 해야함
        toolbarNewKey.clicked += ShowKeyCreationWindow;

        if (blackBoard != null)
        {
            UpdateSelection(blackBoard);
        }
        else
        {
            OnSelectionChange();
        }

        EditorApplication.update -= CheckDeleteCallback;
        EditorApplication.update += CheckDeleteCallback;

        // TODO : 추후 기능 구현 해야함
        RefreshKeys();
    }

    private void InitializeKeys(VisualElement parentElement)
    {
        parentElement.Clear();

        if (serializer.Keys.arraySize > 0)
        {
            for (int i = 0; i < serializer.Keys.arraySize; i++)
            {
                var serializedKey = serializer.Keys.GetArrayElementAtIndex(i);
                var key = (BlackBoardKeyType)serializedKey.managedReferenceValue;

                var keyElement = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ListEntry.uxml");
                var keyElementInstance = keyElement.Instantiate();

                ListEntry listEntryController = new ListEntry();
                listEntryController.SetVisualElements(keyElementInstance, this);
                listEntryController.SetKeyData(key);

                listEntryController.RegisterCallBacks();

                parentElement.Add(keyElementInstance);
            }
        }
    }

    private void CheckDeleteCallback()
    {
        if (Event.current == null)
        {
            return;
        }

        if (Event.current.keyCode == KeyCode.Delete && selectedKey != null)
        {
            if (serializer.DeleteKey(selectedKey))
            {
                selectedKey = null;
                selectedElement = null;

                RefreshKeys();
            }
        }
    }

    public void UpdateSelection(BlackBoard blackBoard)
    {
        serializer = new SerializedBlackBoard(blackBoard);

        RefreshKeys();
    }

    // TODO : 추후 기능 구현 해야함
    public void ShowKeyCreationWindow()
    {
        KeySearchWindowProvider searchWindow = new KeySearchWindowProvider();

        foreach (Type item in TypeCache.GetTypesDerivedFrom<BlackBoardKeyType>())
        {
            if (item.IsAbstract || item.IsGenericType)
            {
                continue;
            }

            string name = item.Name;

            const string KEY_SUFFIX = "Key";

            if (name.EndsWith(KEY_SUFFIX, StringComparison.OrdinalIgnoreCase))
            {
                int index = name.LastIndexOf(KEY_SUFFIX);
                name = name.Remove(index, KEY_SUFFIX.Length);
            }

            searchWindow.AddEntry(new GUIContent(name), () =>
            {
                serializer.AddKey(item);
                RefreshKeys();
            });
        }

        Rect buttonRect = toolbarNewKey.contentRect;
        buttonRect.x += 88;
        buttonRect.y += 4;
        searchWindow.Open(buttonRect);
    }

    // TODO : 추후 기능 구현 해야함
    public void RefreshKeys()
    {
        if (blackBoard != null)
        {
            InitializeKeys(keyList);
            Repaint();
        }
    }

    public void SelectBlackBoard(BlackBoard blackBoard)
    {
        if (!blackBoard)
        {
            ClearSelection();
            return;
        }

        serializer = new SerializedBlackBoard(blackBoard);
    }

    private void ClearSelection()
    {
         serializer = null;
    }

    // 버튼 클릭 했을때
    // 1. 기존에 등록 되어있던 레지스터 해제
    // 2. 선택된 버튼 교체
    // 3. 선택된 버튼의 함수 등록
    // 4. 선택된 버튼의 키에서 이름과 설명 세팅
    public void SetKeyDetail(ListEntry listEntry)
    {
        if (selectedListEntry != null)
        {
            nameTextField.UnregisterValueChangedCallback(selectedListEntry.NameValueChanged);
            descriptionTextField.UnregisterValueChangedCallback(selectedListEntry.DescriptionValueChanged);
        }

        selectedListEntry = listEntry;

        nameTextField.RegisterValueChangedCallback(selectedListEntry.NameValueChanged);
        descriptionTextField.RegisterValueChangedCallback(selectedListEntry.DescriptionValueChanged);

        nameTextField.value = selectedListEntry.blackBoardKeyType.GetKeyName();
        descriptionTextField.value = selectedListEntry.blackBoardKeyType.GetDescription();

        var str = selectedListEntry.blackBoardKeyType.GetType().ToString().Split("_");
        keyType.text = str[1];
    }

    public void RefreshKey(BlackBoardKeyType keytype)
    {
        serializer.RefreshKey(keytype);
    }

    #region GetSet Function
    public BlackBoard GetBlackBoard()
    {
        return blackBoard;
    }
    #endregion
}