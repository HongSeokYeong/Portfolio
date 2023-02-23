using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerJumpState : PlayerAirborneState
{
    // 무브 인풋이 zero가 아니라면 계속 회전하게 한다.
    private bool shouldKeepRotating;
    private bool canStartFalling;

    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.player.reusableData.movementSpeedModifier = 0f;
        stateMachine.player.reusableData.movementDecelerationForce = airborneData.decelerationForce;

        shouldKeepRotating = stateMachine.player.reusableData.movementInput != Vector2.zero;

        Jump();
    }

    public override void Exit()
    {
        base.Exit();

        SetBaseRotationData();

        canStartFalling = false;
    }

    public override void Update()
    {
        base.Update();

        if (!canStartFalling && IsMovingUp(0f))
        {
            canStartFalling = true;
        }

        if (!canStartFalling || IsMovingUp(0f))
        {
            return;
        }

        //stateMachine.ChangeState(stateMachine.fallingState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (shouldKeepRotating)
        {
            RotateTowardsTargetRotation();
        }

        if (IsMovingUp())
        {
            DecelerateVertical();
        }
    }

    #region Main Methods
    private void Jump()
    {
        Vector3 jumpForce = stateMachine.player.reusableData.CurrentJumpForce;

        Vector3 jumpDirection = stateMachine.player.transform.forward;

        if (shouldKeepRotating)
        {
            UpdateTargetRotation(GetMovementInputDirection());

            jumpDirection = GetTargetRotationDirection(stateMachine.player.reusableData.CurrentTargetRotation.y);
        }

        jumpForce.x *= jumpDirection.x;
        jumpForce.z *= jumpDirection.z;

        //jumpForce = GetJumpForceOnSlope(jumpForce);

        ResetVelocity();

        stateMachine.player.rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
    }

    // 언덕에서의 점프력을 얻는 함수
    //private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
    //{
    //    // 플레이어 캡슐 콜라이더의 중앙값을 월드좌표로 가져온다.
    //    Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.colliderUtility.capsuleColliderData.collider.bounds.center;

    //    // 캡슐 콜라이더 위치에서 아래 방향의 레이를 생성.
    //    Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

    //    // 해당 방향으로 레이를 쏜다.
    //    if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, airborneData.jumpData.jumpToGroundRayDistance, stateMachine.player.layerData.groundLayer, QueryTriggerInteraction.Ignore))
    //    {
    //        float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

    //        if (IsMovingUp())
    //        {
    //            float forceModifier = airborneData.jumpData.jumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

    //            jumpForce.x *= forceModifier;
    //            jumpForce.z *= forceModifier;
    //        }

    //        if (IsMovingDown())
    //        {
    //            float forceModifier = airborneData.jumpData.jumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

    //            jumpForce.y *= forceModifier;
    //        }
    //    }

    //    return jumpForce;
    //}
    #endregion

    #region Input Methods
    protected override void OnMovementCanceled(InputAction.CallbackContext context) { }
    #endregion
}
