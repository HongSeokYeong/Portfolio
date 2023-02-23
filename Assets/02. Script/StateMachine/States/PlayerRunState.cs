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

    // 걷기 스테이트로 진입하면 이동속도 변화값을 걷는 속도로 세팅한다.
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

    // 걷기, 달리기 토글키가 입력되면 호출되는 콜백 함수
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.ChangeState(stateMachine.walkState);
    }
}
