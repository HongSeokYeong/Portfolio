using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class ListEntry
{
    Label keyName;
    VisualElement keyBackground;
    BlackBoard_Editor blackBoardEditor;
    public BlackBoardKeyType blackBoardKeyType;

    public void SetVisualElements(TemplateContainer keyElement, BlackBoard_Editor blackBoardEditor)
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/KeyEntry.uss");
        keyElement.styleSheets.Add(styleSheet);

        keyName = keyElement.Q<Label>("keyName");
        keyBackground = keyElement.Q<VisualElement>("keyBackground");

        this.blackBoardEditor = blackBoardEditor;
    }

    public void SetKeyData(BlackBoardKeyType keyType)
    {
        blackBoardKeyType = keyType;
        keyName.text = blackBoardKeyType.GetKeyName();
    }

    public void RegisterCallBacks()
    {
        keyBackground.RegisterCallback<ClickEvent>(ClickAction);
    }

    private void ClickAction(ClickEvent evt)
    {
        blackBoardEditor.SetKeyDetail(this);
    }

    public void NameValueChanged(ChangeEvent<string> str)
    {
        keyName.text = str.newValue;

        blackBoardEditor.serializer.SetKeyName(blackBoardKeyType, str.newValue);
    }

    public void DescriptionValueChanged(ChangeEvent<string> str)
    {
        blackBoardKeyType.SetKeyDescription(str.newValue);

        blackBoardEditor.serializer.SetKeyDescription(blackBoardKeyType, str.newValue);
    }
}
