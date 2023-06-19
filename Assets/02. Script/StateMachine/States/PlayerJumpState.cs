using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PlayerJumpState : PlayerAirborneState
{
    // ���� ��ǲ�� zero�� �ƴ϶�� ��� ȸ���ϰ� �Ѵ�.
    private bool shouldKeepRotating;
    private bool canStartFalling;

    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //stateMachine.player.reusableData.movementSpeedModifier = 0f;
        stateMachine.player.reusableData.movementDecelerationForce = airborneData.decelerationForce;

        shouldKeepRotating = stateMachine.player.reusableData.movementInput != Vector2.zero;

        StartAnimation(stateMachine.player.animationdata.jumpParameterHash);

        Jump();
    }

    public override void Exit()
    {
        base.Exit();

        SetBaseRotationData();

        canStartFalling = false;

        StopAnimation(stateMachine.player.animationdata.jumpParameterHash);
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

        stateMachine.ChangeState(stateMachine.fallState);
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

        stateMachine.player.GetComponent<Rigidbody>().AddForce(jumpForce, ForceMode.VelocityChange);
    }

    // ��������� �������� ��� �Լ�
    //private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
    //{
    //    // �÷��̾� ĸ�� �ݶ��̴��� �߾Ӱ��� ������ǥ�� �����´�.
    //    Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.colliderUtility.capsuleColliderData.collider.bounds.center;

    //    // ĸ�� �ݶ��̴� ��ġ���� �Ʒ� ������ ���̸� ����.
    //    Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

    //    // �ش� �������� ���̸� ���.
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
