using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

// 언리얼의 구조
// BlackBoardComponent의 GetValueAsObject, GetValueAsInt 등등의 함수로 값을 불러온다
// 해당 함수를 호출하면 GetValue<자료형> 함수를 호출하고 GetValue<자료형> 함수에서는
// BlackBoardAsset(BlackBoardData)의 GetKeyID함수에 접근해서 KeyID를 받아온다.
// 해당 KeyID로 진짜 데이터를 찾아서 반환 시켜준다.
[CreateAssetMenu(fileName = "BlackBoard", menuName = "Behaviour Tree/BlackBoard")]
public class BlackBoard : ScriptableObject
{
    [SerializeReference]
    public List<BlackBoardKeyType> keys = new List<BlackBoardKeyType>();

    public void AddKey(BlackBoardKeyType keyType)
    {
        keys.Add(keyType);
    }

    public void DeleteKey(BlackBoardKeyType keyType)
    {
        keys.Remove(keyType);
    }

    public List<BlackBoardKeyType> GetKeys()
    {
        keys = keys.Where(key => key != null).ToList();
        return keys;
    }

    public void SetKeys(List<BlackBoardKeyType> keys)
    {
        this.keys = keys;
    }

    #region Get/Set Value
    public BlackBoardKeyType GetKey(string keyName)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].GetKeyName() == keyName)
            {
                return keys[i];
            }
        }

        return default;
    }

    public Object GetValueAsObject(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_Object).GetValue();
    }

    public Type GetValueAsClass(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_Class).GetValue();
    }

    public Enum GetValueAsEnum(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_Enum).GetValue();
    }

    public int GetValueAsInt(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_Int).GetValue();
    }

    public float GetValueAsFloat(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_Float).GetValue();
    }

    public bool GetValueAsBool(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_Bool).GetValue();
    }

    public string GetValueAsString(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_String).GetValue();
    }

    public Vector3 GetValueAsVector(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_Vector3).GetValue();
    }

    public Quaternion GetValueAsRotator(string keyName)
    {
        return (GetKey(keyName) as BlackBoardKeyType_Rotation).GetValue();
    }

    public void SetValueAsObject(string keyName, Object ObjectValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_Object).SetValue(ObjectValue);
    }

    public void SetValueAsClass(string keyName, Type classValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_Class).SetValue(classValue);
    }

    public void SetValueAsEnum(string keyName, Enum enumValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_Enum).SetValue(enumValue);
    }

    public void SetValueAsInt(string keyName, int IntValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_Int).SetValue(IntValue);
    }

    public void SetValueAsFloat(string keyName, float floatValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_Float).SetValue(floatValue);
    }

    public void SetValueAsBool(string keyName, bool BoolValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_Bool).SetValue(BoolValue);
    }

    public void SetValueAsString(string keyName, string stringValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_String).SetValue(stringValue);
    }

    public void SetValueAsVector(string keyName, Vector3 vectorValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_Vector3).SetValue(vectorValue);
    }


    public void SetValueAsRotator(string keyName, Quaternion vectorValue)
    {
        (GetKey(keyName) as BlackBoardKeyType_Rotation).SetValue(vectorValue);
    }
    #endregion
}
