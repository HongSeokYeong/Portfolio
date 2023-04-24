using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlackBoardKeyType_Bool : BlackBoardKeyType<bool>
{
    public BlackBoardKeyType_Bool()
    {
        supportedOp = E_BlackBoardKeyOperation.Basic;
    }
}
