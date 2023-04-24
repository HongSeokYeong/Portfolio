using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlackBoardKeyType_Transform : BlackBoardKeyType<Transform>
{
    public BlackBoardKeyType_Transform()
    {
        supportedOp = E_BlackBoardKeyOperation.Basic;
    }
}
