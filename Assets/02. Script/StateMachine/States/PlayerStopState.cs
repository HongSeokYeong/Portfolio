using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStopState : PlayerGroundedState
{
    public PlayerStopState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.player.reusableData.movementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.player.animationdata.stoppingParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.stoppingParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();


        RotateTowardsTargetRotation();

        if (!IsMovingHorizontally())
        {
            return;
        }

        DecelerateHorizontally();
    }

    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.idleState);
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.player.input.playerActions.Move.started += OnMovementStarted;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        stateMachine.player.input.playerActions.Move.started -= OnMovementStarted;
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }
}
