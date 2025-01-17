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

    public abstract void NodeStart();
    public abstract void NodeStop();
    public abstract E_State NodeUpdate();
}
