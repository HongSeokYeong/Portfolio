using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Wait : ActionNode
{
    public float duration;
    float startTime;

    public override void NodeStart()
    {
        startTime = Time.time;
        GetBehaviourTreeController().animator.SetBool("Move", false);
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
