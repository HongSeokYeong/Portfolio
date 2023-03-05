using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandingState : PlayerGroundedState
{


    public PlayerLandingState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
        var behaviour = stateMachine.player.animator.GetBehaviour<LandingToIdle>();

        behaviour.stateEvent += Behaviour_stateEvent;
    }

    public override void Enter()
    {
        stateMachine.player.reusableData.movementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.player.animationdata.landingParameterHash);

        ResetVelocity();

        
    }

    private void Behaviour_stateEvent(AnimatorStateInfo obj)
    {
        stateMachine.ChangeState(stateMachine.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.landingParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.player.reusableData.movementInput == Vector2.zero)
        {
            return;
        }

        OnMove();
    }


}
