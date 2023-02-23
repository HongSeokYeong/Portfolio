using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Failure : DecoratorNode
{
    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        var state = childNode.Update();

        if(state == E_State.Success)
        {
            return E_State.Failure;
        }

        return state;
    }
}
