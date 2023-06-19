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

    // ���� ������ Ÿ���� �������� ���;� �Ѵ�.
    public override E_State NodeUpdate()
    {
        if (prevState == E_State.Running)
        {
            prevState = childNode.Update();

            return prevState;
        }

        // ���� ������ Ÿ���� ������(condition �� ���� false �̴�)
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
