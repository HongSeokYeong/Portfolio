using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 필요한 객체에 코드로 붙이는 스크립트
/// IPointerClickHandler, IDragHandler 같은 인터페이스를 상속 받아 오버라이딩 하면, 해당 이벤트가 실행될 때 자동으로 이벤트 함수가 실행
/// 이 스크립트가 붙는 오브젝트들은 해당 이벤트를 받을 수 있다.
/// </summary>
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private string clickActionName, dragActionName;
	private Action<PointerEventData> OnClickHandler = null;
	private Action<PointerEventData> OnDragHandler = null;
	private Action<PointerEventData> OnEnterHandler = null;
	private Action<PointerEventData> OnExitHandler = null;
	private Action<PointerEventData> OnDownHandler = null;
	private Action<PointerEventData> OnUpHandler = null;
	private ScrollRect scrollRect;

	/// <summary>
	/// 이벤트 등록
	/// </summary>
	/// <param name="action">이벤트 발생시 실행할 함수 포인터</param>
	/// <param name="type">이벤트 종류를 지정하여 해당 액션에 등록</param>
	/// <param name="scrollRect">ScrollRect를 전달하면 스크롤 내부의 버튼을 드래그 할 때 스크롤이 작동함</param>
	public void SetAction(Action<PointerEventData> action, E_UIEvent type = E_UIEvent.Click, ScrollRect scrollRect = null)
    {
		switch (type)
		{
			case E_UIEvent.Click:
				OnClickHandler -= action;    // 이미 등록된 함수는 빼기
				OnClickHandler += action;
				clickActionName = action.Method.Name;
				break;

			case E_UIEvent.Drag:
				OnDragHandler -= action;
				OnDragHandler += action;
				dragActionName = action.Method.Name;
				break;
			case E_UIEvent.Enter:
				OnEnterHandler -= action;
				OnEnterHandler += action;
				dragActionName = action.Method.Name;
				break;
			case E_UIEvent.Exit:
				OnExitHandler -= action;
				OnExitHandler += action;
				dragActionName = action.Method.Name;
				break;
			case E_UIEvent.Down:
				OnDownHandler -= action;
				OnDownHandler += action;
				dragActionName = action.Method.Name;
				break;
			case E_UIEvent.Up:
				OnUpHandler -= action;
				OnUpHandler += action;
				dragActionName = action.Method.Name;
				break;
		}
		this.scrollRect = scrollRect;
	}

	// 클릭 이벤트 오버라이딩
	public void OnPointerClick(PointerEventData eventData)
	{
		if (OnClickHandler != null)
		{
			OnClickHandler.Invoke(eventData); // 클릭와 관련된 액션 실행
		}
	}

	// 드래그 시작 이벤트 오버라이딩 : ScrollRect를 전달 받았을 때만 실행
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (scrollRect != null)
		{
			scrollRect.OnBeginDrag(eventData);
		}
	}

	// 드래그 이벤트 오버라이딩
	public void OnDrag(PointerEventData eventData)
	{
		if (OnDragHandler != null)
		{
			OnDragHandler.Invoke(eventData); // 드래그와 관련된 액션 실행
		}

		// ScrollRect를 전달 받았을 때만 실행
		if (scrollRect != null)
		{
			scrollRect.OnDrag(eventData);
		}
	}

	// 드래그 종료 이벤트 오버라이딩 : ScrollRect를 전달 받았을 때만 실행
	public void OnEndDrag(PointerEventData eventData)
	{
		if (scrollRect != null)
		{
			scrollRect.OnEndDrag(eventData);
		}
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
		OnEnterHandler?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
		OnExitHandler?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
		OnDownHandler?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
		OnUpHandler?.Invoke(eventData);
    }
}
