using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

[Serializable]
public abstract class Node
{
    [HideInInspector] public E_State state;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;

    // Context�� �� ������Ʈ�� �°� ���� ������ �Ұ� ����
    // ex) OrcContext, TrollContext ���
    [HideInInspector] public Context context;
    [HideInInspector] public BehaviourTreeController behaviourTreeController;
    [HideInInspector] public BehaviourTree behaviourTree;

    // BlackBoard�� ���������� �� Ÿ�Կ� �´� BlackBoard�� ����� �ְ� �ؾ��Ұ� ����.
    // ex) OrcBlackBoard, TrollBlackBoard
    [HideInInspector] public BlackBoard blackboard;

    [TextArea] public string description;

    public E_State Update()
    {
        if (!started)
        {
            NodeStart();
            started = true;
        }

        state = NodeUpdate();

        if (state != E_State.Running)
        {
            NodeStop();
            started = false;
        }

        return state;
    }

    public BehaviourTree GetBehaviourTree()
    {
        return behaviourTree;
    }

    public BehaviourTreeController GetBehaviourTreeController()
    {
        return behaviourTreeController;
    }

    // ��带 �ߴ��ϴ� ����Լ�
    // Ư�� ��忡�� �� �Լ��� ȣ��Ǹ� �ش� ��带 ������ ��� �ڽĳ����� �ߴܵȴ�.
    //public void Abort()
    //{
    //    BehaviourTree.Traverse(this, (node) => {
    //        node.started = false;
    //        node.state = State.Running;
    //        node.OnStop();
    //    });
    //}

    public abstract void NodeStart();
    public abstract void NodeStop();
    public abstract E_State NodeUpdate();
}
