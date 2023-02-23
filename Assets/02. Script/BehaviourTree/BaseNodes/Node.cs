using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public abstract class Node : ScriptableObject
{
    public E_State state;
    public bool started = false;

    // Context�� �� ������Ʈ�� �°� ���� ������ �Ұ� ����
    // ex) OrcContext, TrollContext ���
    //[HideInInspector] public Context context;

    // BlackBoard�� ���������� �� Ÿ�Կ� �´� BlackBoard�� ����� �ְ� �ؾ��Ұ� ����.
    // ex) OrcBlackBoard, TrollBlackBoard
    //[HideInInspector] public Blackboard blackboard;

    public E_State Update()
    {
        if(!started)
        {
            NodeStart();
            started = true;
        }

        state = NodeUpdate();

        if(state != E_State.Running)
        {
            NodeStop();
            started = false;
        }

        return state;
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
