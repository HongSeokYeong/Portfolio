using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCheck : DecoratorNode
{
    public float duration;
    float startTime;
    bool timeReset = true;

    public override void NodeStart()
    {
        if (timeReset)
        {
            timeReset = false;
            startTime = Time.time;
        }
    }

    public override void NodeStop()
    {
    }

    // duration에 정해진 시간(tick) 이 지나면 다음 액션을 실행하게끔 하는 스크립트 (데코레이터???)
    // 정해진 시간이 아니면 진행을 하지 않는다.
    // 정해진 시간이 돼야지 timeCheck를 true로 해서 success를 반환
    public override E_State NodeUpdate()
    {
        float timeRemaining = Time.time - startTime;

        if (timeRemaining > duration)
        {
            timeReset = true;
            return childNode.Update();
        }

        return E_State.Success;
    }
}