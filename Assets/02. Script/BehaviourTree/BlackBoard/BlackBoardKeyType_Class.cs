using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlackBoardKeyType_Class : BlackBoardKeyType<Type>
{
    public BlackBoardKeyType_Class()
    {
        supportedOp = E_BlackBoardKeyOperation.Basic;
    }
}
