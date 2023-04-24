using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using System;
using UnityEditor.UIElements;

public class BehaviourTree_Editor : EditorWindow
{
    BehaviourTreeView treeView;
    InspectorView inspectorView;
    //BlackBoardView blackBoardView;

    SerializedBehaviourTree serializer;

    ToolbarMenu toolbarMenu;
    Label titleLabel;

    //SerializedObject treeObject;
    //SerializedProperty blackBoardProperty;

    //[SerializeField]
    //private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("BehaviourTree_Editor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviourTree_Editor wnd = GetWindow<BehaviourTree_Editor>();
        wnd.titleContent = new GUIContent("BehaviourTree_Editor");
        wnd.minSize = new Vector2(800, 600);
        // 윈도우가 열릴때 윈도우 초기화 시켜줘야 한다.
        var tree = Selection.activeObject as BehaviourTree;
        wnd.SelectTree(tree);
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }

        return false;
    }

    private void ClearSelection()
    {
        serializer = null;
        //overlayView.Show();
        treeView.ClearView();
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/BehaviourTree_Editor.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTree_Editor.uss");
        root.styleSheets.Add(styleSheet);

        treeView = root.Q<BehaviourTreeView>();
        inspectorView = root.Q<InspectorView>();
        //blackBoardView = root.Q<BlackBoardView>();
        toolbarMenu = root.Q<ToolbarMenu>();
        titleLabel = root.Q<Label>("TitleLabel");

        // Toolbar assets menu
        //toolbarMenu.RegisterCallback<MouseEnterEvent>(e => {

        //    // Refresh the menu options just before it's opened (on mouse enter)
        //    toolbarMenu.menu.MenuItems().Clear();
        //    var behaviourTrees = EditorUtility.GetAssetPaths<BehaviourTree>();
        //    behaviourTrees.ForEach(path => {
        //        var fileName = System.IO.Path.GetFileName(path);
        //        toolbarMenu.menu.AppendAction($"{fileName}", (a) => {
        //            var tree = AssetDatabase.LoadAssetAtPath<BehaviourTree>(path);
        //            SelectTree(tree);
        //        });
        //    });
        //    toolbarMenu.menu.AppendSeparator();
        //    toolbarMenu.menu.AppendAction("New Tree...", (a) => OnToolbarNewAsset());
        //});

        treeView.onNodeSelected = OnNodeSelectionChanged;
        Undo.undoRedoPerformed += OnUndoRedo;

        if (serializer != null)
        {
            SelectTree(serializer.tree);
        }
    }

    private void OnUndoRedo()
    {
        if (serializer != null)
        {
            treeView.PopulateView(serializer);
        }
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
                // TODO : EditorApplication.delayCall 찾아보기
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
        if (Selection.activeGameObject)
        {
            BehaviourTreeController controller = Selection.activeGameObject.GetComponent<BehaviourTreeController>();
            if (controller)
            {
                SelectTree(controller.behaviourTree);
            }
        }
    }

    public void SelectTree(BehaviourTree tree)
    {
        if (!tree)
        {
            ClearSelection();
            return;
        }

        serializer = new SerializedBehaviourTree(tree);

        if (titleLabel != null)
        {
            string path = AssetDatabase.GetAssetPath(serializer.tree);
            if (path == "")
            {
                path = serializer.tree.name;
            }
            titleLabel.text = $"TreeView ({path})";
        }

        treeView.PopulateView(serializer);
        //blackBoardView.Bind(serializer);
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        inspectorView.UpdateSelection(serializer, node);
    }

    private void OnInspectorUpdate()
    {
        treeView?.UpdateNodeState();
    }

    //void OnToolbarNewAsset()
    //{
    //    BehaviourTree tree = EditorUtility.CreateNewTree("New Behaviour Tree", "Assets/");
    //    if (tree)
    //    {
    //        SelectTree(tree);
    //    }
    //}
}



//// -----------------------------------------------------
//public class CharacterListEntryController
//{
//    Label m_NameLabel;

//    public void SetVisualElement(VisualElement visualElement)
//    {
//        m_NameLabel = visualElement.Q<Label>("CharacterName");
//    }

//    public void SetCharacterData(CharacterData characterData)
//    {
//        m_NameLabel.text = characterData.m_CharacterName;
//    }
//}

//public class MainView : MonoBehaviour
//{
//    [SerializeField]
//    VisualTreeAsset m_ListEntryTemplate;

//    void OnEnable()
//    {
//        // The UXML is already instantiated by the UIDocument component
//        var uiDocument = GetComponent<UIDocument>();

//        // Initialize the character list controller
//        var characterListController = new CharacterListController();
//        characterListController.InitializeCharacterList(uiDocument.rootVisualElement, m_ListEntryTemplate);
//    }
//}

//public class CharacterListController
//{
//    // UXML template for list entries
//    VisualTreeAsset m_ListEntryTemplate;

//    // UI element references
//    ListView m_CharacterList;

//    public void InitializeCharacterList(VisualElement root, VisualTreeAsset listElementTemplate)
//    {
//        EnumerateAllCharacters();

//        // Store a reference to the template for the list entries
//        m_ListEntryTemplate = listElementTemplate;

//        // Store a reference to the character list element
//        m_CharacterList = root.Q<ListView>("CharacterList");

//        FillCharacterList();
//    }

//    List<CharacterData> m_AllCharacters;

//    void EnumerateAllCharacters()
//    {
//        m_AllCharacters = new List<CharacterData>();
//        m_AllCharacters.AddRange(Resources.LoadAll<CharacterData>("Characters"));
//    }

//    void FillCharacterList()
//    {
//        // Set up a make item function for a list entry
//        m_CharacterList.makeItem = () =>
//        {
//            // Instantiate the UXML template for the entry
//            var newListEntry = m_ListEntryTemplate.Instantiate();

//            // Instantiate a controller for the data
//            var newListEntryLogic = new CharacterListEntryController();

//            // Assign the controller script to the visual element
//            newListEntry.userData = newListEntryLogic;

//            // Initialize the controller script
//            newListEntryLogic.SetVisualElement(newListEntry);

//            // Return the root of the instantiated visual tree
//            return newListEntry;
//        };

//        // Set up bind function for a specific list entry
//        m_CharacterList.bindItem = (item, index) =>
//        {
//            (item.userData as CharacterListEntryController).SetCharacterData(m_AllCharacters[index]);
//        };

//        // Set a fixed item height
//        m_CharacterList.fixedItemHeight = 45;

//        // Set the actual item's source list/array
//        m_CharacterList.itemsSource = m_AllCharacters;
//    }
//}