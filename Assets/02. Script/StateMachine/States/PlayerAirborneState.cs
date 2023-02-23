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

    // ���̶� ������� ȣ��Ǵ� �Լ�
    // ���� �� ������ ���� ������ ���ؾ��Ѵ�.
    protected override void OnContactWithGround(Collider collider)
    {
        //stateMachine.ChangeState(stateMachine.lightLandingState);
    }
}
