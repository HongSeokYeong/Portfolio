using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : IState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerGroundedData groundedData;

    public PlayerMovementState(PlayerStateMachine playerStateMachine)
    {
        stateMachine = playerStateMachine;
        groundedData = stateMachine.player.playerData.groundedData;

        InitializeData();
    }

    // ������Ʈ�� ���������� ȣ��Ǵ� �Լ�
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
    // �̵� ���� �Է��� �о���̴� �Լ�
    private void ReadMovementInput()
    {
        stateMachine.player.reusableData.movementInput = stateMachine.player.input.playerActions.Move.ReadValue<Vector2>();
    }

    // �������� ó���ϴ� �Լ�
    private void Move()
    {
        // �Է��� ���ų� �̵��ӵ� ��ȭ���� 0�̶�� �����Ѵ�.
        if (stateMachine.player.reusableData.movementInput == Vector2.zero || stateMachine.player.reusableData.movementSpeedModifier == 0f)
        {
            return;
        }

        // �̵��ϴ� ������ ���´�
        Vector3 movementDirection = GetMovementInputDirection();

        // ��ǥ�� yȸ������ ��´�
        float targetRotationYAngle = Rotate(movementDirection);

        // ���� z�� �������� ������ ��´�
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        //// ���� �� �̵��Ҽ� �ִ� �� �ƴϸ� ����
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        if (!Physics.Raycast(stateMachine.player.transform.position + Vector3.up + targetRotationDirection * 0.2f, Vector3.down, layerMask))
        {
            return;
        }

        // �̵��ӵ��� ��´�
        float movementSpeed = GetMovementSpeed();

        // �÷��̾��� x, z ���� �������� �ϴ� �̵��ӵ�
        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        // ������ٵ� �Ⱦ����� ĳ���� ��Ʈ�ѷ��� Move �Լ��� �ٲ��־�� �Ѵ�.
        // ���� �̵��ӵ��� �÷��̾�� �����Ѵ�.
        stateMachine.player.animator.SetFloat("MoveSpeed", stateMachine.player.reusableData.movementInput.sqrMagnitude);
        var result = targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity;
        stateMachine.player.rigidbody.AddForce(result, ForceMode.VelocityChange);
        //stateMachine.player.agent.Move((targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity) * Time.deltaTime);
    }

    // �Ű������� �������� ȸ���ϴ� �Լ�
    private float Rotate(Vector3 direction)
    {
        // ��ǥ ���������� ȸ������ ��´�.
        float directionAngle = UpdateTargetRotation(direction);

        // ��ǥ ȸ�� ���������� ȸ�� ��Ų��.
        RotateTowardsTargetRotation();

        return directionAngle;
    }

    // �̵��Ϸ��� ������ �������� ī�޶��� y�� ȸ������ �����ִ� �Լ�
    private float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.player.mainCameraTransform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }

    // ��ǥ ȸ������ �����ϴ� �Լ�
    private void UpdateTargetRotationData(float targetAngle)
    {
        stateMachine.player.reusableData.CurrentTargetRotation.y = targetAngle;

        stateMachine.player.reusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    // �Ű����� ���������� ������ ���ϴ� �Լ�
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
        // �ϴ� ������ pivotweight�� �ް� �ϰ� ���߿� ����
        stateMachine.player.animator.SetFloat("PivotWeight", animationFloat);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.player.animator.SetBool(animationHash, false);
    }

    // �ȱ�, �޸��� ��� ��ư�� �ݹ� �Լ��� ���ε� ��Ű�� �Լ�
    protected virtual void AddInputActionsCallbacks()
    {
        //stateMachine.player.input.playerActions.WalkToggle.started += OnWalkToggleStarted;

        stateMachine.player.input.playerActions.Look.started += OnMouseMovementStarted;

        stateMachine.player.input.playerActions.Move.performed += OnMovementPerformed;

        stateMachine.player.input.playerActions.Move.canceled += OnMovementCanceled;
    }

    // �ȱ�, �޸��� ��� ��ư�� �ݹ� �Լ��� ����ε� ��Ű�� �Լ�
    protected virtual void RemoveInputActionsCallbacks()
    {
        //stateMachine.player.input.playerActions.WalkToggle.started -= OnWalkToggleStarted;

        stateMachine.player.input.playerActions.Look.started -= OnMouseMovementStarted;

        stateMachine.player.input.playerActions.Move.performed -= OnMovementPerformed;

        stateMachine.player.input.playerActions.Move.canceled -= OnMovementCanceled;
    }

    // �Է°��� ���� ��� �������� �����̴��� ������ִ� �Լ�
    // ex)
    // wŰ�� ������ x�� 0, y�� 1 ���� new Vector3(0, 0, 1)
    protected private Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.player.reusableData.movementInput.x, 0f, stateMachine.player.reusableData.movementInput.y);
    }

    // �⺻ �̵��ӵ��� �̵��ӵ� ��ȭ���� ���ؼ� ���� �̵��ӵ��� ���ϴ� �Լ�
    protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
    {
        //float movementSpeed = groundedData.baseSpeed * stateMachine.reusableData.movementSpeedModifier;

        //float movementSpeed = groundedData.baseSpeed * stateMachine.movementInput.sqrMagnitude;
        float movementSpeed = groundedData.baseSpeed * stateMachine.player.reusableData.movementInput.sqrMagnitude;

        //if (shouldConsiderSlopes)
        //{
        //    movementSpeed *= stateMachine.reusableData.movementOnSlopesSpeedModifier;
        //}

        return movementSpeed;
    }

    // �÷��̾��� x�� z���� �������� �� �̵��ӵ��� ���ϴ� �Լ�
    protected Vector3 GetPlayerHorizontalVelocity()
    {
        //Vector3 playerHorizontalVelocity = stateMachine.player.agent.velocity;
        Vector3 playerHorizontalVelocity = stateMachine.player.rigidbody.velocity;

        playerHorizontalVelocity.y = 0f;

        return playerHorizontalVelocity;
    }

    protected Vector3 GetPlayerVerticalVelocity()
    {
        //return new Vector3(0f, stateMachine.player.agent.velocity.y, 0f);
        return new Vector3(0f, stateMachine.player.rigidbody.velocity.y, 0f);
    }

    // �÷��̾ ��ǥ �������� ȸ����Ű�� �Լ�
    protected void RotateTowardsTargetRotation()
    {
        // ���� �÷��̾��� ȸ������ �޾ƿ´�
        float currentYAngle = stateMachine.player.rigidbody.rotation.eulerAngles.y;
        //float currentYAngle = stateMachine.player.transform.rotation.eulerAngles.y;

        // ���� ȸ������ ��ǥ ȸ�����̶� ���ٸ� ����
        if (currentYAngle == stateMachine.player.reusableData.CurrentTargetRotation.y)
        {
            return;
        }

        // ���� ȸ�������� ��ǥ ȸ�������� ������ ȸ�� ��Ų��.
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.player.reusableData.CurrentTargetRotation.y, ref stateMachine.player.reusableData.DampedTargetRotationCurrentVelocity.y, stateMachine.player.reusableData.TimeToReachTargetRotation.y - stateMachine.player.reusableData.DampedTargetRotationPassedTime.y);

        // ��ǥ ȸ�������� �����ϴµ� ������ �ð�
        stateMachine.player.reusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

        // ȸ������ ���Ѵ�
        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

        // �÷��̾��� ȸ�������� �����Ѵ�
        stateMachine.player.rigidbody.MoveRotation(targetRotation);
        //stateMachine.player.transform.rotation = targetRotation;
    }

    // �Ű����� direction�������� ȸ�������ִ� �Լ�
    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        // ���� direction������ ������ ���ؿ´�
        float directionAngle = GetDirectionAngle(direction);

        // ī�޶��� ȸ������ ����ϴ��� Ȯ��
        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        // ���� ��ǥ ȸ�����̶� ���� ����� ȸ�����̶� �ٸ��� ������Ʈ ���ش�.
        if (directionAngle != stateMachine.player.reusableData.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    // ����z���� �������� ȸ�������� ������ ���ϴ� �Լ�
    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    // �÷��̾��� �ӵ��� ���½�Ű�� �Լ�
    protected void ResetVelocity()
    {
        stateMachine.player.rigidbody.velocity = Vector3.zero;
        //stateMachine.player.agent.velocity = Vector3.zero;
    }

    protected void ResetVerticalVelocity()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        stateMachine.player.rigidbody.velocity = playerHorizontalVelocity;
        //stateMachine.player.agent.velocity = playerHorizontalVelocity;
    }

    protected void DecelerateHorizontally()
    {
        Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

        stateMachine.player.rigidbody.AddForce(-playerHorizontalVelocity * stateMachine.player.reusableData.movementDecelerationForce, ForceMode.Acceleration);
        //stateMachine.player.agent.velocity = -playerHorizontalVelocity * stateMachine.reusableData.movementDecelerationForce;
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

    // �⺻ ȸ�� ������ ����
    protected void SetBaseRotationData()
    {
        //stateMachine.player.reusableData.rotationData = groundedData.baseRotationData;

        stateMachine.player.reusableData.TimeToReachTargetRotation = stateMachine.player.reusableData.TargetRotationReachTime;
    }

    protected virtual void OnContactWithGround(Collider collider)
    {

    }

    protected virtual void OnContactWithGroundExited(Collider collider)
    {

    }

    protected void UpdateCameraRecenteringState(Vector2 movementInput)
    {
        if (movementInput == Vector2.zero)
        {
            return;
        }

        float cameraVericalAngle = stateMachine.player.mainCameraTransform.eulerAngles.x;

        if (cameraVericalAngle >= 270)
        {
            cameraVericalAngle -= 360f;
        }

        cameraVericalAngle = Mathf.Abs(cameraVericalAngle);
    }

    protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
    {
        float movementSpeed = GetMovementSpeed();

        if (movementSpeed == 0f)
        {
            movementSpeed = stateMachine.player.playerData.groundedData.baseSpeed;
        }

        //stateMachine.player.cameraUtility.EnableRecentering(waitTime, recenteringTime, stateMachine.player.playerData.groundedData.baseSpeed, movementSpeed);
    }
    #endregion

    #region Input Methods
    // �ȱ�, �޸��� ��ư�� �������� ȣ��Ǵ� �ݹ� �Լ�
    protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        stateMachine.player.reusableData.shouldWalk = !stateMachine.player.reusableData.shouldWalk;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
    {
        UpdateCameraRecenteringState(context.ReadValue<Vector2>());
    }

    private void OnMouseMovementStarted(InputAction.CallbackContext context)
    {
        UpdateCameraRecenteringState(stateMachine.player.reusableData.movementInput);
    }
    #endregion    
}
