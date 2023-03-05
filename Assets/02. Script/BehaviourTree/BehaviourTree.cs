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
        //if (rootNode.state == E_State.Running)
        {
            behaviourTreeState = rootNode.Update();
        }

        return behaviourTreeState;
    }

    //이 밑의 함수들은 필요한지 체크하고 사용
    // 노드의 자식들을 모아서 반환시켜주는 함수
    // 재귀함수에서 사용함
    public static List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        if (parent is DecoratorNode decorator && decorator.childNode != null)
        {
            decorator.childNode = Instantiate(decorator.childNode);
            children.Add(decorator.childNode);
        }

        if (parent is RootNode rootNode && rootNode.childNode != null)
        {
            rootNode.childNode = Instantiate(rootNode.childNode);
            children.Add(rootNode.childNode);
        }

        if (parent is CompositeNode composite)
        {
            for (int i = 0; i < composite.childNodeList.Count; i++)
            {
                composite.childNodeList[i] = Instantiate(composite.childNodeList[i]);
            }

            return composite.childNodeList;
        }

        return children;
    }

    //노드를 순회하면서 매개변수의 함수들을 실행시키는 함수
    public static void Traverse(Node node, System.Action<Node> visiter)
    {
        if (node != null)
        {
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }
    }

    //BehaviourTree를 복사해서 생성해주는 함수
    // 원본 ScriptableObject를 사용하면 데이터가 변경될 여지가 있기 때문에 복사해서 사용한다.
    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.rootNode = Instantiate(rootNode);
        tree.blackBoard = Instantiate(blackBoard);
        
        return tree;
    }

    //RootNode부터 시작해서 모든 노드에 BehaviourTree의 Context와 BlackBoard를 바인드 시킴
    public void Bind(Context context)
    {
        Traverse(rootNode, node =>
        {
            node.context = context;
            node.blackboard = blackBoard;
        });
    }
}