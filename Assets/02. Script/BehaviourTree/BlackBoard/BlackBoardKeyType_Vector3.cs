using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlackBoardKeyType_Vector3 : BlackBoardKeyType<Vector3>
{
    public BlackBoardKeyType_Vector3()
    {
        supportedOp = E_BlackBoardKeyOperation.Basic;
    }
}
