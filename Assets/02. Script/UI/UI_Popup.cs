using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 팝업 UI 캔버스들의 공통적인 부분
/// </summary>
public class UI_Popup : UI_Base
{
    public override void Init()
    {
        //Logger.Debug($"UI Popup Name : {name}", this);
        //Managers.UI.AddUIDic(name, gameObject);
        //Managers.UI.SetCanvas(gameObject, true);
    }

    // 팝업이니까 고정 캔버스(Scene)과 다르게 닫는게 필요
    // virtual 함수이므로 자식 클래스에서 재정의 가능
    public virtual void ClosePopupUI()
    {
        //StartCoroutine(DelayClosePopupUI());
        //Managers.UI.ClosePopupUI(this);
    }

    private IEnumerator DelayClosePopupUI()
    {
        yield return null;
        //Managers.UI.ClosePopupUI(this);
    }

    public virtual void  OnDestroy()
    {
        //Managers.UI.RemoveUIDic(name);
    }
}
