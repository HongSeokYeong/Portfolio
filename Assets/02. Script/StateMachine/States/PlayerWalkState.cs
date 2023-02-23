using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundedState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // 걷기 스테이트로 진입하면 이동속도 변화값을 걷는 속도로 세팅한다.
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
    // 걷기, 달리기 토글키가 입력되면 호출되는 콜백 함수
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.ChangeState(stateMachine.runState);
    }
}
