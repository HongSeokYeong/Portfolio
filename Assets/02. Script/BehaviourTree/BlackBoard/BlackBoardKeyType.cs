using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public enum E_BlackBoardCompare
{
    Less = -1,
    Equal = 0,
    Greater = 1,
    NotEqual = 1,
}

public enum E_BlackBoardKeyOperation
{
    Basic,
    Arithmetic,
    Text
}

public enum E_BasicKeyOperation
{
    Set,
    NotSet
}

public enum E_ArithmeticKeyOperation
{
    Equal,
    NotEqual,
    Less,
    LessOrEqual,
    Greater,
    GreaterOrEqual
}

public enum E_TextKeyOperation
{
    Equal,
    NotEqual,
    Contain,
    NotContain
}

[Serializable]
public abstract class BlackBoardKeyType
{
    [HideInInspector]
    public string keyCategory;
    
    public string keyName;
    [HideInInspector]
    public string keyDescription;
    [HideInInspector]
    public bool keySync = false;
    [HideInInspector]
    public bool keyIsLocal = false;
    [HideInInspector]
    public string guid;

    protected E_BlackBoardKeyOperation supportedOp;

    protected abstract object GetValueObject();

    // 키의 이름을 반환하는 함수
    public string GetKeyName()
    {
        if (IsKeyLocal())
        {
            var value = GetValueObject();
            return value != null ? value.ToString() : "Null";
        }
        else
        {
            return keyName;
        }
    }

    public bool TryCastValue<T>(out T value)
    {
        if (GetValueObject() is T valueObject)
        {
            value = valueObject;
            return true;
        }

        value = default(T);
        return false;
    }

    public bool IsKeySync()
    {
        return keySync;
    }

    public bool IsKeyLocal()
    {
        return keyIsLocal;
    }

    public abstract Type GetValueType();

    public abstract event Action OnValueChange;

    public string GetDescription()
    {
        return keyDescription;
    }

    // TODO : internal 키워드 공부
    internal void SetKeyLocal(bool value)
    {
        keyIsLocal = value;
    }

    public string GetkeyCategory()
    {
        return keyCategory;
    }

    public void SetKeyCategory(string category)
    {
        keyCategory = category;
    }

    public void SetKeyName(string name)
    {
        keyName = name;
    }

    public void SetKeyDescription(string description)
    {
        keyDescription = description;
    }

    public void SetKeySync(bool sync)
    {
        keySync = sync;
    }
}

public abstract class BlackBoardKeyType<T> : BlackBoardKeyType
{
    [SerializeField]
    public T value = default;

    public T GetValue()
    {
        return value;
    }

    public override Type GetValueType()
    {
        return typeof(T);
    }

    protected override object GetValueObject()
    {
        return value;
    }

    public void SetValue(T setValue)
    {
        //if (setValue == null && value != null || setValue != null && (value == null || !value.Equals(setValue)))
        //{
        //    OnValueChange?.Invoke();
        //}

        value = setValue;
    }

    public void OnValueChangeFunc()
    {
        OnValueChange?.Invoke();
    }

    public override event Action OnValueChange;
}