using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundedState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // �ȱ� ������Ʈ�� �����ϸ� �̵��ӵ� ��ȭ���� �ȴ� �ӵ��� �����Ѵ�.
    public override void Enter()
    {
        stateMachine.player.reusableData.movementSpeedModifier = groundedData.walkSpeedModifier;

        base.Enter();

        StartAnimation(stateMachine.player.animationdata.movingParameterHash);
        StartAnimation(stateMachine.player.animationdata.walkParameterHash);

        //stateMachine.player.reusableData.CurrentJumpForce = airborneData.jumpData.weakForce;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.movingParameterHash);
        StopAnimation(stateMachine.player.animationdata.walkParameterHash);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.walkStopState);

        base.OnMovementCanceled(context);
    }
    // �ȱ�, �޸��� ���Ű�� �ԷµǸ� ȣ��Ǵ� �ݹ� �Լ�
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.ChangeState(stateMachine.runState);
    }
}
