using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkStopState : PlayerStopState
{
    public PlayerWalkStopState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.player.reusableData.movementDecelerationForce = groundedData.walkStopDecelerationForce;

        //stateMachine.player.reusableData.CurrentJumpForce = airborneData.jumpData.weakForce;
    }
}
