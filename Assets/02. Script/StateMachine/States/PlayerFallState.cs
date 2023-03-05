using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerAirborneState
{
    private Vector3 playerPositionOnEnter;

    public PlayerFallState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.player.animationdata.fallParameterHash);

        stateMachine.player.reusableData.movementSpeedModifier = 0f;

        playerPositionOnEnter = stateMachine.player.transform.position;

        ResetVerticalVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.fallParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        LimitVerticalVelocity();
    }

    protected override void OnContactWithGround(Collider collider)
    {
        float fallDistance = playerPositionOnEnter.y - stateMachine.player.transform.position.y;

        stateMachine.ChangeState(stateMachine.landingState);

        //stateMachine.ChangeState(stateMachine.idleState);

        // 밑의 코드는 떨어진 높이에 따라서 착지의 모션이 달라지게 한다.
        if (fallDistance < airborneData.minimumDistanceToBeConsideredHardFall)
        {
            //stateMachine.ChangeState(stateMachine.lightLandingState);

            return;
        }

        if (stateMachine.player.reusableData.shouldWalk && !stateMachine.player.reusableData.shouldSprint || stateMachine.player.reusableData.movementInput == Vector2.zero)
        {
            //stateMachine.ChangeState(stateMachine.hardLandingState);

            return;
        }

        //stateMachine.ChangeState(stateMachine.rollingState);

        
    }

    private void LimitVerticalVelocity()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

        if (playerVerticalVelocity.y >= -airborneData.fallSpeedLimit)
        {
            return;
        }

        Vector3 limitedVelocity = new Vector3(0f, -airborneData.fallSpeedLimit - playerVerticalVelocity.y, 0f);

        stateMachine.player.rigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
    }
}
