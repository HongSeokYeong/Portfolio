using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 고정적인 UI 캔버스들의 공통적인 부분
/// </summary>
public class UI_Normal : UI_Base
{
	public override void Init()
	{
		//Logger.Debug($"UI Name : {name}", this);
		//Managers.UI.AddUIDic(name, gameObject);
		//Managers.UI.SetCanvas(gameObject, false);    // 고정 UI니까 sorting 할 필요가 없으므로 false 전달
	}

	protected virtual void OnDestroy()
	{
		//Managers.UI.RemoveUIDic(name);
	}
}
