using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : DecoratorNode
{
    public bool restartOnSuccess = true;
    public bool restartOnFailure = false;

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
                if (restartOnSuccess)
                {
                    return E_State.Running;
                }
                else
                {
                    return E_State.Success;
                }
            case E_State.Failure:
                if (restartOnFailure)
                {
                    return E_State.Running;
                }else
                {
                    return E_State.Failure;
                }
            case E_State.Running:
            default:
                return E_State.Running;
        }
    }
}
