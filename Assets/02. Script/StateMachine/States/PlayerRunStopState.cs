using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunStopState : PlayerStopState
{
    public PlayerRunStopState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //stateMachine.player.animator.SetFloat("PivotWeight", stateMachine.player.animator.pivotWeight);

        StartAnimation(stateMachine.player.animationdata.mediumStopParameterHash);

        stateMachine.player.reusableData.movementDecelerationForce = groundedData.runStopDecelerationForce;

        //stateMachine.player.reusableData.CurrentJumpForce = airborneData.jumpData.mediumForce;

        stateMachine?.ChangeState(stateMachine.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.mediumStopParameterHash);
    }
}
