using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Object = UnityEngine.Object;

public abstract class UI_Base : MonoBehaviour
{
	protected Dictionary<Type, Object[]> objectsDic = new Dictionary<Type, Object[]>();

	public abstract void Init();

	protected void Bind<T>(Type type) where T : Object
	{
		string[] _names = Enum.GetNames(type);  
		Object[] _objects = new Object[_names.Length];
		objectsDic.Add(typeof(T), _objects);

		for (int i = 0; i < _names.Length; i++)
		{
			if (typeof(T) == typeof(GameObject))
			{
				_objects[i] = Util.FindChild(gameObject, _names[i], true);
			}
			else
			{
				_objects[i] = Util.FindChild<T>(gameObject, _names[i], true);
			}
		}
	}

	public T Get<T>(int idx) where T : Object
	{
		Object[] objects = null;

		if (!objectsDic.TryGetValue(typeof(T), out objects))
		{
			return null;
		}

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
}
