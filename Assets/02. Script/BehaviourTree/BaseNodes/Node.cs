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

    // Context는 각 오브젝트에 맞게 새로 만들어야 할것 같다
    // ex) OrcContext, TrollContext 등등
    [HideInInspector] public Context context;
    [HideInInspector] public BehaviourTreeController behaviourTreeController;
    [HideInInspector] public BehaviourTree behaviourTree;

    // BlackBoard도 마찬가지로 각 타입에 맞는 BlackBoard를 만들수 있게 해야할것 같다.
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

    // 노드를 중단하는 재귀함수
    // 특정 노드에서 이 함수가 호출되면 해당 노드를 포함한 모든 자식노드들이 중단된다.
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
