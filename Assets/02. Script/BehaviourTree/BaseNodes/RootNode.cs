using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootNode :Node
{
    [SerializeReference]
    public Node childNode;

    public override void NodeStart()
    {
    }

    public override void NodeStop()
    {
    }

    public override E_State NodeUpdate()
    {
        return childNode.Update();
    }
}
