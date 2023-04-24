using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum testenum
{
    a,b,c,d,e
}

public class testscript : MonoBehaviour
{
    public BlackBoard bb;
    testenum te;

    // Start is called before the first frame update
    void Start()
    {
        var v = new BlackBoardKeyType_Int();
        v.keyName = "test";
        v.SetValue(10);

        var v2 = new BlackBoardKeyType_String();
        v2.keyName = "testString";
        v2.SetValue("testString");

        var v3 = new BlackBoardKeyType_Enum();
        v3.keyName = "testEnum";
        te = testenum.e;
        v3.SetValue(te);

        var v4 = new BlackBoardKeyType_Object();
        v4.keyName = "testObject";
        v4.SetValue(new testClassType());

        var v5 = new BlackBoardKeyType_Class();
        v5.keyName = "testClass";
        v5.SetValue(typeof(testClassType));

        bb.AddKey(v);
        bb.AddKey(v2);
        bb.AddKey(v3);
        bb.AddKey(v4);
        bb.AddKey(v5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // TODO : TryCastValue는 쓰지 말자. GC 생긴다.
            bb.GetValueAsInt("test");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log(bb.GetValueAsString("testString"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log(bb.GetValueAsEnum("testEnum"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            bb.SetValueAsEnum("testEnum", testenum.d);
            Debug.Log(bb.GetValueAsEnum("testEnum"));
        }
    }
}
