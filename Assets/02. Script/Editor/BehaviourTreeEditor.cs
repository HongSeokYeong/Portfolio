using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[CustomEditor(typeof(BehaviourTree))]
public class BehaviourTreeEditor : Editor
{
    BehaviourTree behaviourTree;
    Dictionary<Type, MemberInfo[]> childFieldsDic = new Dictionary<Type, MemberInfo[]>();

    private void OnEnable()
    {
        behaviourTree = (BehaviourTree)target;

        //childFieldsDic.Clear();

        //foreach (var item in behaviourTree.nodes)
        //{
        //    var type = item.GetType().BaseType;
        //    if (type == typeof(CompositeNode))
        //    {

        //        childFieldsDic.Add(type, type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
        //    }
        //}
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //foreach (var field in childFieldsDic)
        //{
        //    //if(field.IsNotSerialized || field.IsStatic)
        //    //{
        //    //    continue;
        //    //}

        //    if (field.IsPublic || field.GetCustomAttribute(typeof(SerializeField)) != null)
        //    {

        //        EditorGUILayout.PropertyField(serializedObject.FindProperty(field.Name));
        //    }
        //}
    }
}
