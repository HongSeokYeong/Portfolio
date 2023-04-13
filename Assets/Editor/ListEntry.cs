using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

public class ListEntry
{
    Label keyName;
    Button keyButton;

    public void SetVisualElements(TemplateContainer keyElement)
    {
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/KeyEntry.uss");
        keyElement.styleSheets.Add(styleSheet);

        keyName = keyElement.Q<Label>("keyName");
        keyButton = keyElement.Q<Button>("keyButton");
    }

    public void SetKeyData(BlackBoardKeyType keyType)
    {
        keyName.text = keyType.GetKeyName();
    }

    public void RegisterCallBacks()
    {
        keyButton.RegisterCallback<ClickEvent>(ClickAction);
    }

    private void ClickAction(ClickEvent evt)
    {
        Debug.Log("asdf");
    }
}
