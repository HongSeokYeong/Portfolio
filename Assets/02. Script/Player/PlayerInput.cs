using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO : �� Ŭ������ �Լ��� �ʿ����� �����غ��� �ʿ� ������ ����.
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

    // ��õ��� �׼��� ��Ȱ��ȭ ��Ű�� �ڷ�ƾ �Լ��� ȣ���Ѵ�.
    public void DisableActionFor(InputAction action, float seconds)
    {
        StartCoroutine(DisableAction_co(action, seconds));
    }

    // ��õ��� �׼��� ��Ȱ��ȭ ��Ű�� �ڷ�ƾ �Լ�
    private IEnumerator DisableAction_co(InputAction action, float seconds)
    {
        action.Disable();

        yield return new WaitForSeconds(seconds);

        action.Enable();
    }
}
