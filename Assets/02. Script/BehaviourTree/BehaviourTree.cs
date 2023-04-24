using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "BehaviourTree", menuName = "Behaviour Tree/BehaviourTree")]
public class BehaviourTree : ScriptableObject
{
    public BlackBoard blackBoard;

    [SerializeReference]
    public Node rootNode;

    [SerializeReference]
    public List<Node> nodes = new List<Node>();

    public E_State behaviourTreeState = E_State.Running;

    public BehaviourTree()
    {
        rootNode = new RootNode();
        nodes.Add(rootNode);
    }

    public E_State BehaviourTreeUpdate()
    {
        //if (rootNode.state == E_State.Running)
        {
            behaviourTreeState = rootNode.Update();
        }

        return behaviourTreeState;
    }

    //�� ���� �Լ����� �ʿ����� üũ�ϰ� ���
    // ����� �ڽĵ��� ��Ƽ� ��ȯ�����ִ� �Լ�
    // ����Լ����� �����
    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        if (parent is DecoratorNode decorator && decorator.childNode != null)
        {
            children.Add(decorator.childNode);
        }

        if (parent is RootNode rootNode && rootNode.childNode != null)
        {
            children.Add(rootNode.childNode);
        }

        if (parent is CompositeNode composite)
        {
            return composite.childNodeList;
        }

        return children;
    }

    //��带 ��ȸ�ϸ鼭 �Ű������� �Լ����� �����Ű�� �Լ�
    public void Traverse(Node node, System.Action<Node> visiter)
    {
        if (node !=null)
        {
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }
    }

    //BehaviourTree�� �����ؼ� �������ִ� �Լ�
    // ���� ScriptableObject�� ����ϸ� �����Ͱ� ����� ������ �ֱ� ������ �����ؼ� ����Ѵ�.
    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.blackBoard = Instantiate(blackBoard);

        return tree;
    }

    //RootNode���� �����ؼ� ��� ��忡 BehaviourTree�� Context�� BlackBoard�� ���ε� ��Ŵ
    public void Bind(Context context)
    {
        Traverse(rootNode, node =>
        {
            node.context = context;
            node.blackboard = blackBoard;
        });
    }
}