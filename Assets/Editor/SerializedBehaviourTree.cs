using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

// This is a helper class which wraps a serialized object for finding properties on the behaviour.
// It's best to modify the behaviour tree via SerializedObjects and SerializedProperty interfaces
// to keep the UI in sync, and undo/redo
// It's a hodge podge mix of various functions that will evolve over time. It's not exhaustive by any means.
public class SerializedBehaviourTree
{
    // wrapper serialized object for writing changes to the behaviour tree
    public SerializedObject serializedObject;
    public BehaviourTree tree;

    // BehaviourTree ���� ��� �ϴ� �������� �����ؾ��Ѵ�.
    const string rootNodeProperty = "rootNode";
    const string nodesProperty = "nodes";
    const string guidProperty = "guid";
    const string blackboardProperty = "blackBoard";
    const string childProperty = "childNode";
    const string childrenProperty = "childNodeList";
    const string positionProperty = "position";
    const string viewTransformPositionProperty = "viewPosition";
    const string viewTransformScaleProperty = "viewScale";

    // �����Ϸ��� ������ ������Ƽ�� �������� �Ѵ�.
    public SerializedProperty RootNode
    {
        get
        {
            return serializedObject.FindProperty(rootNodeProperty);
        }
    }

    public SerializedProperty Nodes
    {
        get
        {
            return serializedObject.FindProperty(nodesProperty);
        }
    }

    public SerializedProperty Blackboard
    {
        get
        {
            return serializedObject.FindProperty(blackboardProperty);
        }
    }

    public SerializedBehaviourTree(BehaviourTree tree)
    {
        serializedObject = new SerializedObject(tree);
        this.tree = tree;
    }

    // ���������� ������ ���ϴµ� ���ϴ� �Լ����� �𸣰ڴ�.
    // TODO : ApplyModifiedProperties �Լ��� ���� ã�ƺ����Ѵ�.
    public void Save()
    {
        serializedObject.ApplyModifiedProperties();
    }

    public SerializedProperty FindNode(SerializedProperty array, Node node)
    {
        for (int i = 0; i < array.arraySize; i++)
        {
            var current = array.GetArrayElementAtIndex(i);
            // TODO : FindPropertyRelative �Լ� ã�ƺ���
            if (current.FindPropertyRelative(guidProperty).stringValue == node.guid)
            {
                return current;
            }
        }

        return null;
    }

    public void SetViewTransform(Vector3 position, Vector3 scale)
    {
        //serializedObject.FindProperty(viewTransformPositionProperty).vector3Value = position;
        //serializedObject.FindProperty(viewTransformScaleProperty).vector3Value = scale;
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    public void SetNodePosition(Node node, Vector2 position)
    {
        var nodeProp = FindNode(Nodes, node);
        nodeProp.FindPropertyRelative(positionProperty).vector2Value = position;
        serializedObject.ApplyModifiedProperties();
    }

    public void DeleteNode(SerializedProperty array, Node node)
    {
        for (int i = 0; i < array.arraySize; ++i)
        {
            var current = array.GetArrayElementAtIndex(i);
            if (current.FindPropertyRelative(guidProperty).stringValue == node.guid)
            {
                array.DeleteArrayElementAtIndex(i);
                return;
            }
        }
    }

    public Node CreateNodeInstance(Type type)
    {
        // TODO : Activator.CreateInstance �� ���� ã�ƺ���
        var node = Activator.CreateInstance(type) as Node;
        node.guid = GUID.Generate().ToString();
        return node;
    }

    SerializedProperty AppendArrayEelement(SerializedProperty arrayProperty)
    {
        arrayProperty.InsertArrayElementAtIndex(arrayProperty.arraySize);
        return arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);
    }

    public Node CreateNode(Type type, Vector2 position)
    {
        var node = CreateNodeInstance(type);
        node.position = position;

        var newNode = AppendArrayEelement(Nodes);
        newNode.managedReferenceValue = node;

        serializedObject.ApplyModifiedProperties();

        return node;
    }

    public void SetRootNode(RootNode node)
    {
        RootNode.managedReferenceValue = node;
        serializedObject.ApplyModifiedProperties();
    }

    public void DeleteNode(Node node)
    {
        var nodesProperty = Nodes;

        DeleteNode(Nodes, node);
        serializedObject.ApplyModifiedProperties();
    }

    public void AddChild(Node parent, Node child)
    {
        var parentProperty = FindNode(Nodes, parent);

        // RootNode, Decorator node
        var findChildProperty = parentProperty.FindPropertyRelative(childProperty);
        if (findChildProperty != null)
        {
            // TODO : managedReferenceValue�� ���� ã�ƺ���
            findChildProperty.managedReferenceValue = child;
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // compositeNodes
        var findChildrenProperty = parentProperty.FindPropertyRelative(childrenProperty);
        if (findChildrenProperty != null)
        {
            var newChild = AppendArrayEelement(findChildrenProperty);
            newChild.managedReferenceValue = child;
            serializedObject.ApplyModifiedProperties();
            return;
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        var parentProperty = FindNode(Nodes, parent);

        // RootNode, DecoratorNode
        var findChildProperty = parentProperty.FindPropertyRelative(childProperty);
        if (findChildProperty != null)
        {
            findChildProperty.managedReferenceValue = null;
            serializedObject.ApplyModifiedProperties();
            return;
        }

        // CompositeNodes
        var findChildrenProperty = parentProperty.FindPropertyRelative(childrenProperty);
        if (findChildrenProperty != null)
        {
            DeleteNode(findChildrenProperty, child);
            serializedObject.ApplyModifiedProperties();
            return;
        }
    }
}

