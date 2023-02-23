using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO : 이 클래스의 함수들 필요한지 생각해보고 필요 없으면 삭제.
public class PlayerInput : MonoBehaviour
{
    public PlayerInputAction inputActions;

    public PlayerInputAction.PlayerActions playerActions;

    private void Awake()
    {
        inputActions = new PlayerInputAction();

        playerActions = inputActions.Player;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        DisablePlayerInput();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void DisablePlayerInput()
    {
    }

    // 잠시동안 액션을 비활성화 시키는 코루틴 함수를 호출한다.
    public void DisableActionFor(InputAction action, float seconds)
    {
        StartCoroutine(DisableAction_co(action, seconds));
    }

    // 잠시동안 액션을 비활성화 시키는 코루틴 함수
    private IEnumerator DisableAction_co(InputAction action, float seconds)
    {
        action.Disable();

        yield return new WaitForSeconds(seconds);

        action.Enable();
    }
}
