using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RootNode", menuName = "BehaviourTree/RootNode")]
public class RootNode : Node
{
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
