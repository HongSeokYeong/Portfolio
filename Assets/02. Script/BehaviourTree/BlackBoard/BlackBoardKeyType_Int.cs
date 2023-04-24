using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlackBoardKeyType_Int : BlackBoardKeyType<int>
{
    public BlackBoardKeyType_Int()
    {
        supportedOp = E_BlackBoardKeyOperation.Arithmetic;
    }
}
