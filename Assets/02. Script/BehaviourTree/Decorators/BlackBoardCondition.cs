using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoardCondition : DecoratorNode
{
    public string keyName;
    public bool condition;
    public E_State prevState;

    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    // 현재 설정된 타겟이 없을때만 들어와야 한다.
    public override E_State NodeUpdate()
    {
        if (prevState == E_State.Running)
        {
            prevState = childNode.Update();

            return prevState;
        }

        // 현재 설정된 타겟이 없으면(condition 이 현재 false 이다)
        if (blackboard.GetValueAsBool(keyName) == condition)
        {
            prevState = childNode.Update();
            
            return E_State.Success;
        }
        else
        {
            return E_State.Failure;
        }
    }
}
