using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

[Serializable]
public class BlackBoardKeyType_Object : BlackBoardKeyType<Object>
{
    public BlackBoardKeyType_Object()
    {
        supportedOp = E_BlackBoardKeyOperation.Basic;
    }
}
