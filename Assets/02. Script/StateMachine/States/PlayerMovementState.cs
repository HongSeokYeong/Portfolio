using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : IState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerGroundedData groundedData;
    protected PlayerAirborneData airborneData;

    public PlayerMovementState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundedData = stateMachine.player.playerData.groundedData;
        airborneData = stateMachine.player.playerData.airborneData;

        InitializeData();
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void OnAnimationEnterEvent()
    {
    }

    public virtual void OnAnimationExitEvent()
    {
    }

    public virtual void OnAnimationTransitionEvent()
    {
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (stateMachine.player.IsGroundLayer(collision.gameObject.layer))
        {
            OnContactWithGround(collision.collider);
            return;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (stateMachine.player.IsGroundLayer(collision.gameObject.layer))
        {
            OnContactWithGroundExited(collision.collider);
            return;
        }
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        if (stateMachine.player.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGround(collider);

            return;
        }
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        if (stateMachine.player.IsGroundLayer(collider.gameObject.layer))
        {
            OnContactWithGroundExited(collider);

            return;
        }
    }

    public virtual void PhysicsUpdate()
    {
        Move();
    }

    public virtual void Update()
    {
    }

    private void InitializeData()
    {
        SetBaseRotationData();
    }

    #region Main Methods
    private void ReadMovementInput()
    {
        stateMachine.player.reusableData.movementInput = stateMachine.player.input.playerActions.Move.ReadValue<Vector2>();
    }

    private void Move()
    {
        if (stateMachine.player.reusableData.movementInput == Vector2.zero || stateMachine.player.reusableData.movementSpeedModifier == 0f)
        {
            return;
        }

        Vector3 movementDirection = GetMovementInputDirection();

        float targetRotationYAngle = Rotate(movementDirection);

        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        if (!Physics.Raycast(stateMachine.player.transform.position + Vector3.up + targetRotationDirection * 0.2f, Vector3.down, layerMask))
        {
            return;
        }

        float movementSpeed = GetMovementSpeed();

        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        stateMachine.player.animator.SetFloat("MoveSpeed", stateMachine.player.reusableData.movementInput.sqrMagnitude);
        var result = (targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity);
        stateMachine.player.rigidbody.AddForce(result, ForceMode.VelocityChange);
        //stateMachine.player.characterController.Move(result);
    }

    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);

        RotateTowardsTargetRotation();

        return directionAngle;
    }

    private float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.player.mainCameraTransform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }

    private void UpdateTargetRotationData(float targetAngle)
    {
        stateMachine.player.reusableData.CurrentTargetRotation.y = targetAngle;

        stateMachine.player.reusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    private float GetDirectionAngle(Vector3 direction)
    {
        float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (directionAngle < 0f)
        {
            directionAngle += 360f;
        }

        return directionAngle;
    }
    #endregion

    #region Reusable Methods
    protected void StartAnimation(int animationHash)
    {
        stateMachine.player.animator.SetBool(animationHash, true);
    }

    protected void StartAnimation(float animationFloat)
    {
        stateMachine.player.animator.SetFloat("PivotWeight", animationFloat);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.player.animator.SetBool(animationHash, false);
    }

    protected virtual void AddInputActionsCallbacks()
    {
        stateMachine.player.input.playerActions.Look.started += OnMouseMovementStarted;

        stateMachine.player.input.playerActions.Move.performed += OnMovementPerformed;

        stateMachine.player.input.playerActions.Move.canceled += OnMovementCanceled;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        stateMachine.player.input.playerActions.Look.started -= OnMouseMovementStarted;

        stateMachine.player.input.playerActions.Move.performed -= OnMovementPerformed;

        stateMachine.player.input.playerActions.Move.canceled -= OnMovementCanceled;
    }

    protected private Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.player.reusableData.movementInput.x, 0f, stateMachine.player.reusableData.movementInput.y);
    }

    protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
    {
        float movementSpeed = groundedData.baseSpeed * stateMachine.player.reusableData.movementInput.sqrMagnitude;

        return movementSpeed;
    }

    protected Vector3 GetPlayerHorizontalVelocity()
    {
        Vector3 playerHorizontalVelocity = stateMachine.player.rigidbody.velocity;
        //Vector3 playerHorizontalVelocity = stateMachine.player.characterController.velocity;

        playerHorizontalVelocity.y = 0f;

        return playerHorizontalVelocity;
    }

    protected Vector3 GetPlayerVerticalVelocity()
    {
        //return new Vector3(0f, stateMachine.player.characterController.velocity.y, 0f);
        return new Vector3(0f, stateMachine.player.rigidbody.velocity.y, 0f);
    }

    protected void RotateTowardsTargetRotation()
    {
        float currentYAngle = stateMachine.player.transform.rotation.eulerAngles.y;

        if (currentYAngle == stateMachine.player.reusableData.CurrentTargetRotation.y)
        {
            return;
        }

        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.player.reusableData.CurrentTargetRotation.y, ref stateMachine.player.reusableData.DampedTargetRotationCurrentVelocity.y, stateMachine.player.reusableData.TimeToReachTargetRotation.y - stateMachine.player.reusableData.DampedTargetRotationPassedTime.y);

        stateMachine.player.reusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

        //stateMachine.player.transform.rotation = targetRotation;
        stateMachine.player.rigidbody.MoveRotation(targetRotation);
    }

    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        float directionAngle = GetDirectionAngle(direction);

        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        if (directionAngle != stateMachine.player.reusableData.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    protected void ResetVelocity()
    {
        stateMachine.player.rigidbody.velocity = Vector3.zero;
    }

    protected void ResetVerticalVelocity()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.player.rigidbody.velocity = playerHorizontalVelocity;
    }

    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();
        stateMachine.player.rigidbody.AddForce(-playerHorizontalVelocity * stateMachine.player.reusableData.movementDecelerationForce, ForceMode.Acceleration);
    }

    protected void DecelerateVertical()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
        stateMachine.player.rigidbody.AddForce(-playerVerticalVelocity * stateMachine.player.reusableData.movementDecelerationForce, ForceMode.Acceleration);
    }

    protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        Vector2 playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);

        return playerHorizontalMovement.magnitude > minimumMagnitude;
    }

    protected bool IsMovingUp(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y > minimumVelocity;
    }

    protected bool IsMovingDown(float minimumVelocity = 0.1f)
    {
        return GetPlayerVerticalVelocity().y < -minimumVelocity;
    }

    protected void SetBaseRotationData()
    {
        stateMachine.player.reusableData.TimeToReachTargetRotation = stateMachine.player.reusableData.TargetRotationReachTime;
    }

    protected virtual void OnContactWithGround(Collider collider)
    {

    }

    protected virtual void OnContactWithGroundExited(Collider collider)
    {

    }
    #endregion

    #region Input Methods
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        stateMachine.player.reusableData.shouldWalk = !stateMachine.player.reusableData.shouldWalk;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.idleState);
    }

    protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
    {
    }

    private void OnMouseMovementStarted(InputAction.CallbackContext context)
    {
    }
    #endregion
}
