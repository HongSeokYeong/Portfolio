using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class CompositeNode : Node
{    
    public List<Node> childNodeList = new List<Node>();
}
    