using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDown : DecoratorNode
{
    public bool coolDownStart = false;
    public float duration;
    float startTime;

    // ó���� ��带 �����ϰ� �ڽ� ��尡 �������� ������ �׶����� �ð��� üũ�Ѵ�.
    public override void NodeStart()
    {
        if (!coolDownStart)
        {
            startTime = Time.time;
        }
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        if (!coolDownStart)
        {
            var state = childNode.Update();

            if (state == E_State.Success)
            {
                coolDownStart = true;
            }

            return state;
        }

        if (coolDownStart)
        {
            float timeRemaining = Time.time - startTime;

            if (timeRemaining > duration)
            {
                coolDownStart = false;
            }
        }

        return E_State.Success;
    }
}
