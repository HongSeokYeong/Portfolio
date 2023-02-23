using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : DecoratorNode
{
    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        switch (childNode.Update())
        {
            case E_State.Success:
                return E_State.Failure;
            case E_State.Failure:
                return E_State.Success;
            case E_State.Running:
                return E_State.Running;
            default:
                return E_State.Failure;
        }
    }
}
