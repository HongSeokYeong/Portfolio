using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDown : DecoratorNode
{
    public bool coolDownStart = false;
    public float duration;
    float startTime;

    // 처음에 노드를 실행하고 자식 노드가 성공으로 끝나면 그때부터 시간을 체크한다.
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
