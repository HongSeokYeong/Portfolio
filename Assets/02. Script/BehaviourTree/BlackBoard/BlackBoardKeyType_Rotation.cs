using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlackBoardKeyType_Rotation : BlackBoardKeyType<Quaternion>
{
    public BlackBoardKeyType_Rotation()
    {
        supportedOp = E_BlackBoardKeyOperation.Basic;
    }
}
