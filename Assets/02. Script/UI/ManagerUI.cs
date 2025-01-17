using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    private const int basePopupOrder = 10;
    private int order = basePopupOrder;

    private Dictionary<string, GameObject> popupObjectDic = new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> canvasObjectDic = new Dictionary<string, GameObject>();

    private Transform rootTransform;
    public Transform RootTransform
    {
        get
        {
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
    public Camera CameraUI
    {
        get
        {
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

        canvas.overrideSorting = true;

        // 팝업일 경우 렌더링 순서 부여
        if (sort)
        {
            Debug.Log("Canvas Order : " + order);
            canvas.sortingOrder = order;
            order++;
            canvas.sortingLayerName = "PopUp";
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public void SetCanvasMode(GameObject go, float scale = 1, bool isWorldSpace = false, Vector3 postion = default(Vector3), Vector3 rotation = default(Vector3))
    {
        Canvas canvas = go.GetComponent<Canvas>();
            SetCanvasScaler(go);
    }

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

    private void CanvasScreenSpaceCamera(GameObject _canvasObject)
    {
            Canvas _canvas = _canvasObject.GetComponent<Canvas>();
            _canvas.renderMode  = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = CameraUI;
    }

    public T ShowSceneUI<T>(string name = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        return default(T);
    }
    public T ShowPopupUI<T>(Transform parent = null, string name = null, bool isCameraMode = true)
    {
        return default(T);
    }
    public T OpenUI<T>(Transform parent = null, bool isCameraMode = true)
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
            return component;
        }
    }
    public T ClosePopupUI<T>()
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

    public void RefreshUI()
    {
        rootTransform.gameObject.SetActive(false);
        rootTransform.gameObject.SetActive(true);
    }

    public void AddUIDic(string name, GameObject gameObject)
    {
        if (!canvasObjectDic.ContainsKey(name))
        {
            canvasObjectDic.Add(name, gameObject);
        }
    }

    public void RemoveUIDic(string name)
    {
        if (canvasObjectDic.ContainsKey(name))
        {
            canvasObjectDic.Remove(name);
        }
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

    public T GetOtherUI<T>() where T : UI_Base
    {
        canvasObjectDic.TryGetValue(typeof(T).Name, out GameObject _go);
        return _go?.GetComponent<T>();
    }

    public void ButtonInfo(PointerEventData data)
    {
        if(data == null) { return; }
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
