using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;

/// <summary>
/// 모든 UI의 기반이 되는 추상 클래스
/// </summary>
public abstract class UI_Base : MonoBehaviour
{
	protected Dictionary<Type, Object[]> objectsDic = new Dictionary<Type, Object[]>();

	public abstract void Init();

	/// <summary>
	/// UI 오브젝트 이름으로 찾아서 바인딩.
	/// 상속받은 객체를 기준으로 하위 객체를 탐색해서 찾음
	/// </summary>
	/// <typeparam name="T">찾을 컴포넌트 or GameObject</typeparam>
	/// <param name="type">컴포넌트들의 이름을 담아 종류별로 구분한 Enum 클래스들의 Reflection 정보</param>
	protected void Bind<T>(Type type) where T : Object
	{
		string[] _names = Enum.GetNames(type);    // 열거형에 정의된 모든 내용을 문자열로 가져옴
		Object[] _objects = new Object[_names.Length];
		objectsDic.Add(typeof(T), _objects); // Dictionary 에 추가

		// T 에 속하는 오브젝트들을 Dictionary의 Value인 objects 배열의 원소들에 하나하나 추가
		for (int i = 0; i < _names.Length; i++)
		{
			// T가 컴포넌트가 없는 빈 GameObject이면 GameObject를 찾음
			if (typeof(T) == typeof(GameObject))
			{
				_objects[i] = Util.FindChild(gameObject, _names[i], true);
			}
			// T에 해당하는 컴포넌트를 찾음
			else
			{
				_objects[i] = Util.FindChild<T>(gameObject, _names[i], true);
			}
		}
	}

	/// <summary>
	/// UI Enum 번호에 해당하는 오브젝트 T 타입을 가져오기
	/// </summary>
	/// <typeparam name="T">T 컴포넌트 or GameObject</typeparam>
	/// <param name="idx">UI Enum 인덱스</param>
	/// <returns></returns>
	public T Get<T>(int idx) where T : Object
	{
		Object[] objects = null;

		// m_Objects Dictionary에 typeof(T) Key가 존재하면 True, objects 배열에 그 typeof(T) Key의 Value를 저장
		if (!objectsDic.TryGetValue(typeof(T), out objects))
		{
			return null;
		}

		// m_Objects Dictionary의 Value인 배열은 Enum 안에 정의된 순서대로 들어가 있으므로
		return objects[idx] as T;
	}

	public GameObject GetObject(int idx) { return Get<GameObject>(idx); } // 오브젝트로서 가져오기
	public Text GetText(int idx) { return Get<Text>(idx); } // Text로서 가져오기
	public TMP_Text GetTextTMP(int idx) { return Get<TMP_Text>(idx); } // TMP Text로서 가져오기
	public InputField GetInputField(int idx) { return Get<InputField>(idx); }
	public TMP_InputField GetInputFieldTMP(int idx) { return Get<TMP_InputField>(idx); }
	public Button GetButton(int idx) { return Get<Button>(idx); } // Button로서 가져오기
	public Slider GetSlider(int idx) { return Get<Slider>(idx); } // Slider 가져오기
	public Image GetImage(int idx) { return Get<Image>(idx); } // Image로서 가져오기
	public RawImage GetRawImage(int idx) { return Get<RawImage>(idx); } // RawImage로서 가져오기
	public Dropdown GetDropdown(int idx) { return Get<Dropdown>(idx); } // DropnDown으로 가져오기
	public TMP_Dropdown GetDropdownTMP(int idx) { return Get<TMP_Dropdown>(idx); } //DropnDown_Tmp으로 가져오기
	public ScrollRect GetScroll(int idx) { return Get<ScrollRect>(idx); } // Scroll로서 가져오기
	public Scrollbar GetScrollbar(int idx) { return Get<Scrollbar>(idx); } // ScrollBar로 가져오기
	public Toggle GetToggle(int idx) { return Get<Toggle>(idx); } // Toogle로 가져오기
	public CanvasGroup GetCanvasGroup(int idx) { return Get<CanvasGroup>(idx); } // CanavsGroup으로 가져오기
	public RectTransform GetRectTransform(int idx) { return Get<RectTransform>(idx); } // RectTransform으로 가져오기
	public Transform GetTransform(int idx) { return Get<Transform>(idx); } // Transform으로 가져오기
	public ScrollRect GetScrollRect(int idx) { return Get<ScrollRect>(idx); } // scrollRect로 가져오기

	/// <summary>
	/// TE 타입에 정의된 모든 UI 컴포넌트 가져오기
	/// </summary>
	/// <typeparam name="TE">UI Enum Type</typeparam>
	/// <typeparam name="TC">UI Component Type</typeparam>
	/// <returns>UI Component List</returns>
	protected List<TC> GetAll<TE, TC>() where TE : Enum where TC : Component
	{
		List<TC> _objects = new List<TC>();
		int _countObject = Enum.GetValues(typeof(TE)).Length;

		for (int idx = 0; idx < _countObject; idx++)
		{
			if (Get<TC>(idx) != null)
			{
				_objects.Add(Get<TC>(idx));
			}
		}

		return _objects;
	}

	/// <summary>
	/// TE 타입에 정의된 모든 UI 컴포넌트의 Transform 가져오기
	/// </summary>
	/// <typeparam name="TE">UI Enum Type</typeparam>
	/// <typeparam name="TC">UI Component Type</typeparam>
	/// <returns>UI Component List</returns>
	protected List<Transform> GetAllTransform<TE, TC>() where TE : Enum where TC : Component
	{
		List<Transform> _objects = new List<Transform>();
		int _countObject = Enum.GetValues(typeof(TE)).Length;

		for (int idx = 0; idx < _countObject; idx++)
		{
			if (Get<TC>(idx) != null)
			{
				_objects.Add(Get<TC>(idx).transform);
			}
		}

		return _objects;
	}

	/// <summary>
	/// go 오브젝트에 UI_EventHandler를 붙여 go 오브젝트가 이벤트 콜백을 받을 수 있도록 한다.
	/// </summary>
	/// <param name="go">UI_EventHandler를 붙일 오브젝트</param>
	/// <param name="action">이벤트 발생시 실행할 함수 포인터</param>
	/// <param name="type">이벤트 종류를 지정하여 해당 액션에 등록</param>
	/// <param name="scrollRect">ScrollRect를 전달하면 스크롤 내부의 버튼을 드래그 할 때 스크롤이 작동함</param>
	//public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.E_UIEvent type = Define.E_UIEvent.Click, ScrollRect scrollRect = null)
	//{
	//	UI_EventHandler _evt = Util.GetOrAddComponent<UI_EventHandler>(go);
	//	_evt.SetAction(action, type, scrollRect);
	//}
}
