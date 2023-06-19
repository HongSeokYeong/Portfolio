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

    // duration�� ������ �ð�(tick) �� ������ ���� �׼��� �����ϰԲ� �ϴ� ��ũ��Ʈ (���ڷ�����???)
    // ������ �ð��� �ƴϸ� ������ ���� �ʴ´�.
    // ������ �ð��� �ž��� timeCheck�� true�� �ؼ� success�� ��ȯ
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