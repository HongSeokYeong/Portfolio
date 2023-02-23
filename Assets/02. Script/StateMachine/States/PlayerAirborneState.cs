using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.player.animationdata.airborneParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.airborneParameterHash);
    }

    // 땅이랑 닿았을때 호출되는 함수
    // 점프 후 랜딩을 할지 안할지 정해야한다.
    protected override void OnContactWithGround(Collider collider)
    {
        //stateMachine.ChangeState(stateMachine.lightLandingState);
    }
}
