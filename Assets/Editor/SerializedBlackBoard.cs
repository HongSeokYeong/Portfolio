using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SerializedBlackBoard
{
    public SerializedObject serializedObject;
    public BlackBoard blackBoard;

    const string keysProperty = "keys";
    const string guidProperty = "guid";
    const string keyNameProperty = "keyName";
    const string keyDescriptionProperty = "keyDescription";

    public SerializedProperty Keys
    {
        get
        {
            return serializedObject.FindProperty(keysProperty);
        }
    }

    public SerializedBlackBoard(BlackBoard blackBoard)
    {
        serializedObject = new SerializedObject(blackBoard);
        this.blackBoard = blackBoard;
    }

    public BlackBoardKeyType AddKey(Type keyType)
    {
        var createdKeyType = CreateKeyTypeInstance(keyType);

        var newKeyType = AppendArrayElement(Keys);
        newKeyType.managedReferenceValue = createdKeyType;

        serializedObject.ApplyModifiedProperties();

        return createdKeyType;
    }

    public BlackBoardKeyType CreateKeyTypeInstance(Type type)
    {
        var keyType = Activator.CreateInstance(type) as BlackBoardKeyType;
        keyType.guid = GUID.Generate().ToString();
        return keyType;
    }

    SerializedProperty AppendArrayElement(SerializedProperty arrayProperty)
    {
        arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
        return arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
    }

    public bool DeleteKey(BlackBoardKeyType keyType)
    {
        for (int i = 0; i < Keys.arraySize; i++)
        {
            var current = Keys.GetArrayElementAtIndex(i);
            if (current.FindPropertyRelative(guidProperty).stringValue == keyType.guid)
            {
                Keys.DeleteArrayElementAtIndex(i);
                AssetDatabase.SaveAssets();
                return true;
            }
        }

        return false;
    }

    public void RefreshKey(BlackBoardKeyType keyType)
    {
        for (int i = 0; i < Keys.arraySize; i++)
        {
            var current = Keys.GetArrayElementAtIndex(i);
            if (current.FindPropertyRelative(guidProperty).stringValue == keyType.guid)
            {
                var key = current.managedReferenceValue as BlackBoardKeyType;
                key.SetKeyCategory(keyType.GetkeyCategory());
                key.SetKeyDescription(keyType.GetDescription());
                key.SetKeyName(keyType.GetKeyName());
                key.SetKeySync(keyType.keySync);

                serializedObject.ApplyModifiedProperties();

                AssetDatabase.SaveAssets();
            }
        }
    }

    private SerializedProperty FindKey(BlackBoardKeyType keyType)
    {
        for (int i = 0; i < Keys.arraySize; i++)
        {
            var current = Keys.GetArrayElementAtIndex(i);

            if (current.FindPropertyRelative(guidProperty).stringValue == keyType.guid)
            {
                return current;
            }
        }

        return null;
    }

    public void SetKeyName(BlackBoardKeyType keyType, string newValue)
    {
        var findKey = FindKey(keyType);

        if (findKey != null)
        {
            findKey.FindPropertyRelative(keyNameProperty).stringValue = newValue;
            serializedObject.ApplyModifiedProperties();
        }
    }

    public void SetKeyDescription(BlackBoardKeyType keyType, string newValue)
    {
        var findKey = FindKey(keyType);

        if (findKey != null)
        {
            findKey.FindPropertyRelative(keyDescriptionProperty).stringValue = newValue;
            serializedObject.ApplyModifiedProperties();
        }
    }
}
