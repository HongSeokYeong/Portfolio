using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Selector : CompositeNode
{
    protected int current;

    public override void NodeStart()
    {
        current = 0;
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        // �����ʹ� ������ ��ȯ ���������� �ڽ��� ��� ��带 ��ȸ�Ѵ�.
        for (int i = current; i < childNodeList.Count; i++)
        {
            current = i;
            var child = childNodeList[current];

            switch (child.Update())
            {
                case E_State.Success:
                    return E_State.Success;
                case E_State.Failure:
                    continue;
                case E_State.Running:
                    return E_State.Running;
                default:
                    return E_State.Failure;
            }
        }

        return E_State.Failure;
    }
}
