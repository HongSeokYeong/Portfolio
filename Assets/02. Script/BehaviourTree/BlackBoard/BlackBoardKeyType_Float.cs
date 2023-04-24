using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlackBoardKeyType_Float : BlackBoardKeyType<float>
{
    public BlackBoardKeyType_Float()
    {
        supportedOp = E_BlackBoardKeyOperation.Arithmetic;
    }
}
