using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 게임 씬 상에서 생길 여러가지 UI 캔버스 프리팹들의 생성과 삭제를 관리
/// </summary>
public class ManagerUI : MonoBehaviour
{
    private const int basePopupOrder = 10;
    private int order = basePopupOrder;    // 현재까지 최근에 사용한 오더 (팝업 캔버스 렌더링 순서)

    // 팝업 스택은 안쓰고 캔버스 오더만 변경하여 사용하기로 함
    //private Stack<UI_Popup> popupStack = new Stack<UI_Popup>(); // 오브젝트 말고 컴포넌트를 담음. 팝업 캔버스 UI 들을 담는다.
    //private UI_Normal normalUI = null; // 현재의 고정 캔버스 UI

    // 한번 생성된 UI는 참조를 저장하여 이미 생성된 UI인지 확인할 때 사용
    // Key : UI_타입 이름
    private Dictionary<string, GameObject> popupObjectDic = new Dictionary<string, GameObject>();

    // 생성된 UI 객체들. 외부에서 접근할 때 사용
    private Dictionary<string, GameObject> canvasObjectDic = new Dictionary<string, GameObject>();

    // 선택된 상태를 표시하는 스파인. 다른 스파인을 선택하면 이 스파인을 Disable
    //private UI_SelectSpine selectSpine;
    //public UI_SelectSpine SelectSpine { get => selectSpine; set => selectSpine = value; }

    // UI_Root라는 이름의 오브젝트를 없다면 만들어서라도 리턴해주는 프로퍼티
    // UI 오브젝트들은 이 UI_Root 빈 오브젝트 아래에 생성되게 그룹화할 것이라서 필요
    private Transform rootTransform;
    public Transform RootTransform
    {
        get
        {
            //GameObject root = GameObject.Find("UI_Root");
            //if (root == null)
            //    root = new GameObject { name = "UI_Root" };

            if (rootTransform == null)
            {
                rootTransform = new GameObject { name = "UI_Root" }.transform;
            }
            return rootTransform;
        }
    }

    // HUD 카메라 취득
    public Camera CameraHUD
    {
        get
        {
            Transform _cameraHUD = Camera.main.transform.Find("CameraHUD");
            if (_cameraHUD != null)
                return _cameraHUD.GetComponent<Camera>();
            else
                return null;
        }
    }

    private Camera cameraUI;
    // ! [2022.10.20] UI 카메라 생성
    public Camera CameraUI
    {
        get
        {
            //if (cameraUI == null)
            //    cameraUI = GameObject.Instantiate(Resources.Load<GameObject>(Constant.PATH_CAMERA_UI), Vector3.down * 100f, Quaternion.identity).GetComponent<Camera>();

            return cameraUI;
        }
    }

    // 씬 전환시 UI 관련 상태 초기화
    public void InitStateUI()
    {
        order = basePopupOrder;
        popupObjectDic.Clear();
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);

        // by 승욱 :: VR용 월드캔버스를 사용하기 위해 주석처리
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        canvas.overrideSorting = true; // 캔버스 안에 캔버스 중첩 경우 (부모 캔버스가 어떤 값을 가지던 나는 내 오더값을 가지려 할때)

        // 팝업일 경우 렌더링 순서 부여
        if (sort)
        {
            Debug.Log("Canvas Order : " + order);
            canvas.sortingOrder = order;
            // order++처리를 ShowPopupUI 에서 안하고 SetCanvas에서 해준 이유
            // 대부분의 팝업은 캔버스 프리팹을 통해 생성할 예정(ShowPopupUI 사용)이나
            // 게임 시작전부터 static 하게 존재하고 있던, 팝업 UI일 경우도 있을 수 있는데 그런 경우엔 ShowPopupUI를 거치지 않기 때문
            order++;
            canvas.sortingLayerName = "PopUp";
        }
        // sorting 요청이 없는 것은 팝업이 아닌 일반 고정 UI
        else
        {
            canvas.sortingOrder = 0;
        }

        //Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //canvas.overrideSorting = true; // 캔버스 안에 캔버스 중첩 경우 (부모 캔버스가 어떤 값을 가지던 나는 내 오더값을 가지려 할때)

        //// 팝업일 경우 렌더링 순서 부여
        //if (sort)
        //{
        //    canvas.sortingOrder = order;
        //    // order++처리를 ShowPopupUI 에서 안하고 SetCanvas에서 해준 이유
        //    // 대부분의 팝업은 캔버스 프리팹을 통해 생성할 예정(ShowPopupUI 사용)이나
        //    // 게임 시작전부터 static 하게 존재하고 있던, 팝업 UI일 경우도 있을 수 있는데 그런 경우엔 ShowPopupUI를 거치지 않기 때문
        //    order++;
        //}
        //// sorting 요청이 없는 것은 팝업이 아닌 일반 고정 UI
        //else
        //{
        //    canvas.sortingOrder = 0;
        //}
    }

    /// <summary>
    /// 디바이스 종류에 따라 UI 캔버스 모드와 몇가지 설정을 바꿈
    /// </summary>
    /// <param name="go">Canvas 프리팹</param>
    /// <param name="scale">VR 모드일 때 UI Scale</param>
    /// <param name="isWorldSpace">WorldSpace 모드이면 true</param>
    /// <param name="scale">캔버스 크기</param>
    /// <param name="postion">캔버스 위치</param>
    /// <param name="rotation">캔버스 회전값</param>
    public void SetCanvasMode(GameObject go, float scale = 1, bool isWorldSpace = false, Vector3 postion = default(Vector3), Vector3 rotation = default(Vector3))
    {
        Canvas canvas = go.GetComponent<Canvas>();

            //canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            //int screenMode = Screen.width > Screen.height ? 0 : 1;
            //SetCanvasScaler(go, screenMode);
            SetCanvasScaler(go);
    }

    /// <summary>
    /// 디바이스 구분 없이 UI 캔버스 모드를 바꿀 때 사용
    /// </summary>
    /// <param name="go">Canvas 프리팹</param>
    /// <param name="renderMode">설정할 캔버스 모드</param>
    /// <param name="scale">캔버스 크기</param>
    /// <param name="postion">캔버스 위치</param>
    /// <param name="rotation">캔버스 회전값</param>
    public void SetCanvasMode(GameObject go, RenderMode renderMode, float scale = 1, Vector3 postion = default(Vector3), Vector3 rotation = default(Vector3))
    {
        Canvas canvas = go.GetComponent<Canvas>();
        canvas.renderMode = renderMode;

        switch(renderMode)
        {
            // WorldSpace 일 때는 크기/위치/회전 값을 캔버스에 바로 적용
            case RenderMode.WorldSpace:
                go.transform.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
                go.transform.localPosition = postion;
                go.transform.rotation = Quaternion.Euler(rotation);
                break;

            // 카메라 모드일 때는 캔버스 바로 아래의 자식 객체에 Scale만 적용
            case RenderMode.ScreenSpaceCamera:
                canvas.worldCamera = Camera.main;
                canvas.planeDistance = 1;
                go.transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
                break;

            // 기본값 Overlay는 특이사항 없음
            default:
                break;
        }
    }

    public void ReplaceCanvasToFront(GameObject obj, float scale = 0.001f, float distance = 1f, float angle = 30)
    {
        var canvasPos = Camera.main.transform.position + new Vector3(Camera.main.transform.forward.x, -0.2f, Camera.main.transform.forward.z) * distance;
        SetCanvasMode(obj, scale, true, canvasPos);
        obj.transform.forward = Camera.main.transform.forward;
        obj.transform.rotation = Quaternion.Euler(angle, obj.transform.rotation.eulerAngles.y, obj.transform.rotation.eulerAngles.z);
    }

    /// <summary>
    /// 화면 가로/세로 모드에 따라 UI 해상도 대응
    /// 가로 모드이면 높이를 기준으로 맞추기
    /// 세로 모드이면 확장
    /// </summary>
    /// <param name="obj">조정할 UI 객체</param>
    /// <param name="screenMode">0 : 가로 모드 (높이 기준),  1 : 세로 모드 (확장)</param>
    /// <param name="matchMode">0 : 가로/세로 기준,  1 : 확장</param>
    /// <param name="matchValue">matchMode가 0일 때 가로/세로 기준값 0 ~ 1</param>
    public void SetCanvasScaler(GameObject obj, int screenMode = 0, int matchMode = 1, float matchValue = 1)
    {
        CanvasScaler canvasScaler = obj.GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        if (screenMode == 0)
        {
            canvasScaler.referenceResolution = new Vector2(2400, 1080);
        }
        else
        {
            canvasScaler.referenceResolution = new Vector2(1080, 2400);
        }
        canvasScaler.screenMatchMode = (CanvasScaler.ScreenMatchMode)matchMode;
        canvasScaler.matchWidthOrHeight = matchValue;
    }

    /// <summary>
    /// 캔버스 ScreenSpace-Camera로 변경 함수
    /// <para/>모바일만 가능
    /// </summary>
    /// <param name="_canvasObject"></param>
    private void CanvasScreenSpaceCamera(GameObject _canvasObject)
    {
            Canvas _canvas = _canvasObject.GetComponent<Canvas>();
            _canvas.renderMode  = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = CameraUI;
    }

    /// <summary>
    /// 고정 UI 캔버스 프리팹 생성
    /// </summary>
    /// <typeparam name="T">UI_Scene 자식들만 가능</typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    public T ShowSceneUI<T>(string name = null)
        //where T : UI_Normal
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        // 프리팹을 Instantiate으로 생성하고 sceneUI에 바인딩 후 이를 리턴
        //GameObject go = Managers.Resource.Instantiate($"UI/Normal/{name}");
        //T _sceneUI = Util.GetOrAddComponent<T>(go);
        //normalUI = _sceneUI;

        //go.transform.SetParent(RootTransform);
        //CanvasScreenSpaceCamera(go);

        //return _sceneUI;

        return default(T);
    }

    /// <summary>
    /// 팝업 UI 캔버스 프리팹 생성
    /// </summary>
    /// <typeparam name="T">UI_PopUp 자식들만 가능</typeparam>
    /// <param name="name">사용 금지</param>
    /// <returns></returns>
    public T ShowPopupUI<T>(Transform parent = null, string name = null, bool isCameraMode = true)
        //where T : UI_Popup
    {
        //if (string.IsNullOrEmpty(name)) // 이름을 안받았다면 T로 ㄱㄱ
        //{
        //    name = typeof(T).Name;
        //}

        //Transform _parent = parent == null ? RootTransform : parent;

        //// 프리팹을 Instantiate으로 생성하고 popup에 바인딩 후 스택에 추가. 그리고 이를 리턴
        //GameObject _go = Managers.Resource.Instantiate($"UI/Popup/{name}", _parent);
        //if (parent != null) { _go.transform.position = parent.position; }    // 부모 객체를 따로 지정한 경우 초기 위치도 부모 객체 위치로 지정
        //T popup = Util.GetOrAddComponent<T>(_go);
        ////popupStack.Push(popup);
        //if (isCameraMode) { CanvasScreenSpaceCamera(_go); }

        ////Logger.Debug($"Open UI Object Name : {_go.name}, T Name : {popup.name}", this);
        ////popupObjectDic.Add(name, _go);
        //bool isAddDic = popupObjectDic.TryAdd(name, _go);
        //if (!isAddDic) { Debug.LogWarning("Failed to add UI object"); }

        //return popup;

        return default(T);
    }

    /// <summary>
    /// 팝업 UI 캔버스 프리팹 생성 / 이미 생성된 UI는 활성화
    /// </summary>
    /// <typeparam name="T">UI_PopUp 자식들만 가능</typeparam>
    /// <param name="parent">캔버스 모드가 WorldSpace일 경우 부모 지정</param>
    /// <returns></returns>
    public T OpenUI<T>(Transform parent = null, bool isCameraMode = true)
        //where T : UI_Popup
    {
        // 생성된 UI가 없으면 새로 생성
        if (popupObjectDic.TryGetValue(typeof(T).Name, out GameObject _go) == false)
        {
            return ShowPopupUI<T>(parent, isCameraMode: isCameraMode);    // UI 생성
        }
        // 이미 생성된 UI일 경우 우선순위 설정 & 활성화
        else
        {
            SetCanvas(_go);    // 우선순위 sort order 설정
            _go.SetActive(true);
            T component = _go.GetComponent<T>();
            if (isCameraMode) { CanvasScreenSpaceCamera(_go); }
            //popupStack.Push(component);
            return component;
        }
    }

    /// <summary>
    /// 특정 팝업 UI 지정해서 닫기
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T ClosePopupUI<T>()
        //where T : UI_Popup
    {
        // 생성된 UI가 없으면 무시
        if (popupObjectDic.TryGetValue(typeof(T).Name, out GameObject _go) == false)
        {
            return default(T);
        }
        // 생성된 UI일 경우 비활성화
        else
        {
            _go.SetActive(false);
            T component = _go.GetComponent<T>();
            return component;
        }
    }

    /// <summary>
    /// 팝업 안전하게 닫기
    /// </summary>
    /// <param name="popup"></param>
    public void ClosePopupUI(
        //UI_Popup popup
        )
    {
        // 비어있는 스택이라면 삭제 불가
        //if (popupStack.Count == 0) { return; }

        //if (popupStack.Peek() != popup)
        {
            //Logger.Debug("Close Popup Failed!", this); // 스택의 가장 위에있는 Peek() 것만 삭제할 수 있기 때문에 popup이 Peek()가 아니면 삭제 못함
            //return;
        }

        //popup.gameObject.SetActive(false);
        //order--;    // 팝업을 닫았으니 팝업 캔버스 렌더링 순서 줄이기
        //ClosePopupUI();
    }



    //public void ClosePopupUI()
    //{
    //    //if (popupStack.Count == 0) { return; }

    //    //UI_Popup popup = popupStack.Pop();

    //    popup.gameObject.SetActive(false);

    //    //Managers.Resource.Destroy(popup.gameObject);

    //    popup = null;

    //    // 모든 팝업이 닫히면 팝업 order 초기화
    //    if (popupStack.Count == 0)
    //    {
    //        order = basePopupOrder;
    //    }
    //    else
    //    {
    //        order--;    // 팝업을 닫았으니 팝업 캔버스 렌더링 순서 줄이기
    //    }
    //}

    //public void CloseAllPopupUI()
    //{
    //    while (popupStack.Count > 0)
    //    {
    //        ClosePopupUI();
    //    }
    //}

    /// <summary>
    /// 사용 안함 : UI 껐다 켜기. 언어 변경 후 UI에 반영할 때 사용
    /// </summary>
    public void RefreshUI()
    {
        rootTransform.gameObject.SetActive(false);
        rootTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// UI 생성시 해당 객체 참조 저장
    /// </summary>
    /// <param name="name">UI 프리팹/스크립트 이름</param>
    /// <param name="gameObject">생성할 UI 프리팹</param>
    public void AddUIDic(string name, GameObject gameObject)
    {
        if (!canvasObjectDic.ContainsKey(name))
        {
            canvasObjectDic.Add(name, gameObject);
        }
    }

    /// <summary>
    /// UI 파괴시 해당 객체 참조 삭제
    /// </summary>
    /// <param name="name">UI 프리팹/스크립트 이름</param>
    public void RemoveUIDic(string name)
    {
        //Logger.Debug("RemoveUIDic : " + name, this);
        if (canvasObjectDic.ContainsKey(name))
        {
            canvasObjectDic.Remove(name);
        }
        //Debug.Log("UI ObjectDic Size : " + canvasObjectDic.Count);
    }

    public void AddPopUpDic(string name, GameObject gameObject)
    {
        if (!popupObjectDic.ContainsKey(name))
        {
            popupObjectDic.Add(name, gameObject);
        }
    }

    public void RemovePopUpDic(string name)
    {
        if (popupObjectDic.ContainsKey(name))
        {
            popupObjectDic.Remove(name);
        }
    }

    /// <summary>
    /// 다른 UI 캔버스 스크립트 찾기
    /// </summary>
    /// <typeparam name="T">UI 클래스</typeparam>
    /// <returns>찾은 UI 클래스 참조</returns>
    public T GetOtherUI<T>() where T : UI_Base
    {
        //Logger.Debug("GetOtherUI : " + typeof(T).Name, this);
        canvasObjectDic.TryGetValue(typeof(T).Name, out GameObject _go);
        return _go?.GetComponent<T>();
    }

    public void ButtonInfo(PointerEventData data)
    {
        if(data == null) { return; }
        //Logger.Debug($"Button Name : {data.pointerPress.name} / Count : {data.clickCount} / Position : {data.pressPosition}", this);
    }

    public event EventHandler FadeInEvent;
    public event EventHandler FadeOutEvent;
    public void FadeInPopup(float delay = 0)
    {
        FadeInEvent?.Invoke(this, EventArgs.Empty);
    }

    public void FadeOutPopup(float delay = 0)
    {
        FadeOutEvent?.Invoke(this, EventArgs.Empty);
    }
}
