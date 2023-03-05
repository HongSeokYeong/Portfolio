using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    #region IState Methods
    // 아이들 스테이트로 진입하면 이동속도 변화값을 0으로 세팅한다.
    // 리지드 바디의 속도를 0으로 리셋한다.
    public override void Enter()
    {
        stateMachine.player.reusableData.movementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.player.animationdata.idleParameterHash);

        stateMachine.player.reusableData.CurrentJumpForce = airborneData.stationaryForce;

        ResetVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.idleParameterHash);
    }

    // 아이들 스테이트에서는 이동키가 입력되길 기다렸다가
    // 이동키가 입력되면 이동 스테이트로 넘긴다.
    public override void Update()
    {
        base.Update();

        stateMachine.player.animator.SetFloat("MoveSpeed", stateMachine.player.rigidbody.velocity.magnitude, 0.25f, Time.deltaTime);

        if (stateMachine.player.reusableData.movementInput == Vector2.zero)
        {
            return;
        }

        OnMove();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!IsMovingHorizontally())
        {
            return;
        }

        ResetVelocity();
    }
    #endregion
}
