using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Sequencer", menuName = "BehaviourTree/Sequencer")]
public class Sequencer : CompositeNode
{
    protected int current;

    public override void NodeStart()
    {
        current = 0;
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        // 시퀀스는 실패를 반환 받을 때까지 자식의 모든 노드를 순회한다.
        for (int i = current; i < childNodeList.Count; i++)
        {
            current = i;
            var child = childNodeList[current];

            switch (child.Update())
            {
                case E_State.Success:
                    continue;
                case E_State.Failure:
                    return E_State.Failure;
                case E_State.Running:
                    return E_State.Running;
                default:
                    return E_State.Success;
            }
        }

        return E_State.Success;
    }
}
