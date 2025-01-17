
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public class Util
{
    /// <summary>
    /// go 오브젝트의 모든 자식들 중 name과 이름이 일치하는 GameObject를 찾아 리턴
    /// </summary>
    /// <param name="go">탐색 기준이 되는 객체</param>
    /// <param name="name">찾을 오브젝트 이름</param>
    /// <param name="recursive">true이면 모든 자식들 중에서 찾음. false이면 직속 자식들 중에서만 찾음</param>
    /// <returns></returns>
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
        {
            return null;
        }

        return transform.gameObject;
    }

    /// <summary>
    /// go 오브젝트의 모든 자식들 중 T 컴포넌트를 가지며, name과 이름이 일치하는 오브젝트를 찾아 리턴
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go">탐색 기준이 되는 객체</param>
    /// <param name="name">찾을 오브젝트 이름</param>
    /// <param name="recursive">true이면 모든 자식들 중에서 찾음. false이면 직속 자식들 중에서만 찾음</param>
    /// <returns></returns>
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
        {
            return null;
        }

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                // name이 null이면 그냥 T 컴포넌트에 해당 되는 것을 리턴, 이름을 지정했으면 이름이 일치하는 컴포넌트를 리턴
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
        }
        else
        {
            T[] components = go.GetComponentsInChildren<T>(true);

            for (int idx = 0; idx < components.Length; idx++)
            {
                // name이 null이면 그냥 T 컴포넌트에 해당 되는 것을 리턴, 이름을 지정했으면 이름이 일치하는 컴포넌트를 리턴
                if (string.IsNullOrEmpty(name) || components[idx].name == name)
                {
                    return components[idx];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// go 오브젝트의 T 컴포넌트 가져오기. T 컴포넌트가 없다면 AddComponent로 붙여준 후 가져옴
    /// </summary>
    /// <typeparam name="T">가져올 컴포넌트</typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static void AddListener(EventTrigger eventTrigger, EventTriggerType triggerType, UnityAction<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener(callback);
        eventTrigger.triggers.Add(entry);
    }

    public static bool ContainsLayer(LayerMask layerMask, int layer)
    {
        return (1 << layer & layerMask) != 0;
    }
}
