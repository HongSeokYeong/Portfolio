using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlackBoardKeyType_String : BlackBoardKeyType<string>
{
    public BlackBoardKeyType_String()
    {
        supportedOp = E_BlackBoardKeyOperation.Text;
    }
}
