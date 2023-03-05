using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        StartAnimation(stateMachine.player.animationdata.groundedParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.groundedParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // 이걸로 걷기 구현 해야한다.
        if(stateMachine.player.input.playerActions.Walk.IsPressed())
        {
        }
    }

    // 땅에서의 스테이트라서 점프랑 대쉬를 추가해줘야한다.
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.player.input.playerActions.Jump.started += OnJumpStarted;
        stateMachine.player.input.playerActions.Dash.started += OnDashStarted;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        stateMachine.player.input.playerActions.Jump.started -= OnJumpStarted;
        stateMachine.player.input.playerActions.Dash.started -= OnDashStarted;
    }

    protected virtual void OnMove()
    {
        if (stateMachine.player.reusableData.shouldWalk)
        {
            stateMachine.ChangeState(stateMachine.walkState);

            return;
        }

        stateMachine.ChangeState(stateMachine.runState);
    }

    // 바닥과 닿아있다가 떨어졌는지 확인하는 함수
    protected override void OnContactWithGroundExited(Collider collider)
    {
        if (IsThereGroundUnderneath())
        {
            return;
        }

        //Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.colliderUtility.capsuleColliderData.collider.bounds.center;

        //Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine.player.colliderUtility.capsuleColliderData.colliderVerticalExtents, Vector3.down);

        //// 캐릭터 캡슐 콜라이더의 중앙에서 밑으로 레이를 쐈는데 닿은게 없다면 떨어지는 함수 호출
        //if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, groundedData.groundToFallRayDistance, stateMachine.player.groundLayer, QueryTriggerInteraction.Ignore))
        //{
        //    OnFall();
        //}
    }

    protected virtual void OnFall()
    {
        //stateMachine.ChangeState(stateMachine.fallingState);
    }

    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        //stateMachine.ChangeState(stateMachine.dashState);
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.jumpState);
    }

    protected override void OnMovementPerformed(InputAction.CallbackContext context)
    {
        base.OnMovementPerformed(context);

        UpdateTargetRotation(GetMovementInputDirection());
    }

    protected virtual void OnAttackStarted(InputAction.CallbackContext context)
    {
        //stateMachine.ChangeState(stateMachine.attackState);
    }

    protected virtual void OnAttackPerformed(InputAction.CallbackContext context)
    {
    }

    private bool IsThereGroundUnderneath()
    {
        //PlayerTriggerColliderData triggerColliderData = stateMachine.player.colliderUtility.triggerColliderData;

        //Vector3 groundColliderCenterInWorldSpace = triggerColliderData.groundCheckCollider.bounds.center;

        //Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, triggerColliderData.groundCheckColliderVerticalExtents, triggerColliderData.groundCheckCollider.transform.rotation, stateMachine.player.layerData.groundLayer, QueryTriggerInteraction.Ignore);

        //return overlappedGroundColliders.Length > 0;

        return true;
    }

    private void UpdateShouldSprintState()
    {
        //if (!stateMachine.reusableData.shouldSprint)
        //{
        //    return;
        //}

        //if (stateMachine.reusableData.movementInput != Vector2.zero)
        //{
        //    return;
        //}

        //stateMachine.reusableData.shouldSprint = false;
    }

    private void Float()
    {
        //Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.colliderUtility.capsuleColliderData.collider.bounds.center;

        //Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        //if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, stateMachine.player.colliderUtility.slopeData.floatRayDistance, stateMachine.player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
        //{
        //    float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

        //    float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

        //    if (slopeSpeedModifier == 0f)
        //    {
        //        return;
        //    }

        //    float distanceToFloatingPoint = stateMachine.player.colliderUtility.capsuleColliderData.colliderCenterInLocalSpace.y * stateMachine.player.transform.localScale.y - hit.distance;

        //    if (distanceToFloatingPoint == 0f)
        //    {
        //        return;
        //    }

        //    float amountToLift = distanceToFloatingPoint * stateMachine.player.colliderUtility.slopeData.stepReachForce - GetPlayerVerticalVelocity().y;

        //    Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

        //    stateMachine.player.rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        //}
    }

    // 언덕 각도에 따라 속도 보정치를 결정하는 함수
    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = groundedData.slopeSpeedAngles.Evaluate(angle);

        if (stateMachine.player.reusableData.movementOnSlopesSpeedModifier != slopeSpeedModifier)
        {
            stateMachine.player.reusableData.movementOnSlopesSpeedModifier = slopeSpeedModifier;
        }

        return slopeSpeedModifier;
    }
}
