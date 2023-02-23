using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BehaviourTree", menuName ="BehaviourTree/BehaviourTree")]
public class BehaviourTree : ScriptableObject
{
    public BlackBoard blackBoard;

    public RootNode rootNode;

    //public List<Node> nodes;

    public E_State behaviourTreeState = E_State.Running;

    public E_State BehaviourTreeUpdate()
    {
        if (rootNode.state == E_State.Running)
        {
            behaviourTreeState = rootNode.Update();
        }

        return behaviourTreeState;
    }

    // �� ���� �Լ����� �ʿ����� üũ�ϰ� ���
    // ����� �ڽĵ��� ��Ƽ� ��ȯ�����ִ� �Լ�
    // ����Լ����� �����
    //public static List<Node> GetChildren(Node parent)
    //{
    //    List<Node> children = new List<Node>();

    //    if (parent is DecoratorNode decorator && decorator.child != null)
    //    {
    //        children.Add(decorator.child);
    //    }

    //    if (parent is RootNode rootNode && rootNode.child != null)
    //    {
    //        children.Add(rootNode.child);
    //    }

    //    if (parent is CompositeNode composite)
    //    {
    //        return composite.children;
    //    }

    //    return children;
    //}
    
    // ��带 ��ȸ�ϸ鼭 �Ű������� �Լ����� �����Ű�� �Լ�
    //public static void Traverse(Node node, System.Action<Node> visiter)
    //{
    //    if (node != null)
    //    {
    //        visiter.Invoke(node);
    //        var children = GetChildren(node);
    //        children.ForEach((n) => Traverse(n, visiter));
    //    }
    //}

    // BehaviourTree�� �����ؼ� �������ִ� �Լ�
    // ���� ScriptableObject�� ����ϸ� �����Ͱ� ����� ������ �ֱ� ������ �����ؼ� ����Ѵ�.
    //public BehaviourTree Clone()
    //{
    //    BehaviourTree tree = Instantiate(this);
    //    return tree;
    //}

    // RootNode���� �����ؼ� ��� ��忡 BehaviourTree�� Context�� BlackBoard�� ���ε� ��Ŵ
    //public void Bind(Context context)
    //{
    //    Traverse(rootNode, node => {
    //        node.context = context;
    //        node.blackboard = blackboard;
    //    });
    //}
}