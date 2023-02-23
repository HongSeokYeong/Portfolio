using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundedState
{
    private float startTime;

    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // �ȱ� ������Ʈ�� �����ϸ� �̵��ӵ� ��ȭ���� �ȴ� �ӵ��� �����Ѵ�.
    public override void Enter()
    {
        stateMachine.player.reusableData.movementSpeedModifier = groundedData.runSpeedModifier;

        base.Enter();

        StartAnimation(stateMachine.player.animationdata.movingParameterHash);
        StartAnimation(stateMachine.player.animationdata.runParameterHash);

        //stateMachine.player.reusableData.CurrentJumpForce = airborneData.jumpData.mediumForce;

        startTime = Time.time;
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.movingParameterHash);
        StopAnimation(stateMachine.player.animationdata.runParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (!stateMachine.player.reusableData.shouldWalk)
        {
            return;
        }

        if (Time.time < startTime + groundedData.runToWalkTime)
        {
            return;
        }

        StopRunning();
    }

    private void StopRunning()
    {
        if (stateMachine.player.reusableData.movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.idleState);

            return;
        }

        stateMachine.ChangeState(stateMachine.walkState);
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.runStopState);

        base.OnMovementCanceled(context);
    }

    // �ȱ�, �޸��� ���Ű�� �ԷµǸ� ȣ��Ǵ� �ݹ� �Լ�
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.ChangeState(stateMachine.walkState);
    }
}
