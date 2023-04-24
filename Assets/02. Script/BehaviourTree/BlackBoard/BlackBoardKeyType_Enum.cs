using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlackBoardKeyType_Enum : BlackBoardKeyType<Enum>
{
    public BlackBoardKeyType_Enum()
    {
        supportedOp = E_BlackBoardKeyOperation.Basic;
    }
}
