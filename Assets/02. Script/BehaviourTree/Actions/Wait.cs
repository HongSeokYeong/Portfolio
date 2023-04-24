using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : ActionNode
{
    public float duration;
    float startTime;

    public override void NodeStart()
    {
        Debug.Log("Wait Node");
        startTime = Time.time;
        context.animator.SetTrigger("Idle");
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        float timeRemaining = Time.time - startTime;

        if (timeRemaining > duration)
        {
            return E_State.Success;
        }

        return E_State.Running;
    }
}
