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

    // 스테이트에 진입했을때 호출되는 함수
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
    // 이동 관련 입력을 읽어들이는 함수
    private void ReadMovementInput()
    {
        stateMachine.player.reusableData.movementInput = stateMachine.player.input.playerActions.Move.ReadValue<Vector2>();
    }

    // 움직임을 처리하는 함수
    private void Move()
    {
        // 입력이 없거나 이동속도 변화값이 0이라면 리턴한다.
        if (stateMachine.player.reusableData.movementInput == Vector2.zero || stateMachine.player.reusableData.movementSpeedModifier == 0f)
        {
            return;
        }

        // 이동하는 방향을 얻어온다
        Vector3 movementDirection = GetMovementInputDirection();

        // 목표의 y회전값을 얻는다
        float targetRotationYAngle = Rotate(movementDirection);

        // 월드 z축 기준으로 방향을 얻는다
        Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

        //// 레이 찍어서 이동할수 있는 곳 아니면 리턴
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        if (!Physics.Raycast(stateMachine.player.transform.position + Vector3.up + targetRotationDirection * 0.2f, Vector3.down, layerMask))
        {
            return;
        }

        // 이동속도를 얻는다
        float movementSpeed = GetMovementSpeed();

        // 플레이어의 x, z 축을 기준으로 하는 이동속도
        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        // 리지드바디 안쓸꺼면 캐릭터 컨트롤러의 Move 함수로 바꿔주어야 한다.
        // 최종 이동속도를 플레이어에게 전달한다.
        stateMachine.player.animator.SetFloat("MoveSpeed", stateMachine.player.reusableData.movementInput.sqrMagnitude);
        var result = targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity;
        stateMachine.player.rigidbody.AddForce(result, ForceMode.VelocityChange);
        //stateMachine.player.agent.Move((targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity) * Time.deltaTime);
    }

    // 매개변수의 방향으로 회전하는 함수
    private float Rotate(Vector3 direction)
    {
        // 목표 방향으로의 회전값을 얻는다.
        float directionAngle = UpdateTargetRotation(direction);

        // 목표 회전 방향쪽으로 회전 시킨다.
        RotateTowardsTargetRotation();

        return directionAngle;
    }

    // 이동하려는 방향의 각도값에 카메라의 y축 회전값을 더해주는 함수
    private float AddCameraRotationToAngle(float angle)
    {
        angle += stateMachine.player.mainCameraTransform.eulerAngles.y;

        if (angle > 360f)
        {
            angle -= 360f;
        }

        return angle;
    }

    // 목표 회전값을 저장하는 함수
    private void UpdateTargetRotationData(float targetAngle)
    {
        stateMachine.player.reusableData.CurrentTargetRotation.y = targetAngle;

        stateMachine.player.reusableData.DampedTargetRotationPassedTime.y = 0f;
    }

    // 매개변수 방향으로의 각도를 구하는 함수
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
        // 일단 지금은 pivotweight만 받게 하고 나중에 수정
        stateMachine.player.animator.SetFloat("PivotWeight", animationFloat);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.player.animator.SetBool(animationHash, false);
    }

    // 걷기, 달리기 토글 버튼에 콜백 함수를 바인드 시키는 함수
    protected virtual void AddInputActionsCallbacks()
    {
        //stateMachine.player.input.playerActions.WalkToggle.started += OnWalkToggleStarted;

        stateMachine.player.input.playerActions.Look.started += OnMouseMovementStarted;

        stateMachine.player.input.playerActions.Move.performed += OnMovementPerformed;

        stateMachine.player.input.playerActions.Move.canceled += OnMovementCanceled;
    }

    // 걷기, 달리기 토글 버튼에 콜백 함수를 언바인드 시키는 함수
    protected virtual void RemoveInputActionsCallbacks()
    {
        //stateMachine.player.input.playerActions.WalkToggle.started -= OnWalkToggleStarted;

        stateMachine.player.input.playerActions.Look.started -= OnMouseMovementStarted;

        stateMachine.player.input.playerActions.Move.performed -= OnMovementPerformed;

        stateMachine.player.input.playerActions.Move.canceled -= OnMovementCanceled;
    }

    // 입력값을 토대로 어느 방향으로 움직이는지 계산해주는 함수
    // ex)
    // w키만 누르면 x는 0, y는 1 따라서 new Vector3(0, 0, 1)
    protected private Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.player.reusableData.movementInput.x, 0f, stateMachine.player.reusableData.movementInput.y);
    }

    // 기본 이동속도에 이동속도 변화값을 곱해서 최종 이동속도를 구하는 함수
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

    // 플레이어의 x축 z축을 기준으로 한 이동속도를 구하는 함수
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

    // 플레이어를 목표 방향으로 회전시키는 함수
    protected void RotateTowardsTargetRotation()
    {
        // 현재 플레이어의 회전값을 받아온다
        float currentYAngle = stateMachine.player.rigidbody.rotation.eulerAngles.y;
        //float currentYAngle = stateMachine.player.transform.rotation.eulerAngles.y;

        // 현재 회전값이 목표 회전값이랑 같다면 리턴
        if (currentYAngle == stateMachine.player.reusableData.CurrentTargetRotation.y)
        {
            return;
        }

        // 현재 회전값에서 목표 회전값까지 서서히 회전 시킨다.
        float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.player.reusableData.CurrentTargetRotation.y, ref stateMachine.player.reusableData.DampedTargetRotationCurrentVelocity.y, stateMachine.player.reusableData.TimeToReachTargetRotation.y - stateMachine.player.reusableData.DampedTargetRotationPassedTime.y);

        // 목표 회전값까지 도달하는데 지나간 시간
        stateMachine.player.reusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

        // 회전값을 구한다
        Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

        // 플레이어의 회전값으로 대입한다
        stateMachine.player.rigidbody.MoveRotation(targetRotation);
        //stateMachine.player.transform.rotation = targetRotation;
    }

    // 매개변수 direction방향으로 회전시켜주는 함수
    protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
    {
        // 방향 direction으로의 각도를 구해온다
        float directionAngle = GetDirectionAngle(direction);

        // 카메라의 회전값을 고려하는지 확인
        if (shouldConsiderCameraRotation)
        {
            directionAngle = AddCameraRotationToAngle(directionAngle);
        }

        // 현재 목표 회전값이랑 최종 계산한 회전값이랑 다르면 업데이트 해준다.
        if (directionAngle != stateMachine.player.reusableData.CurrentTargetRotation.y)
        {
            UpdateTargetRotationData(directionAngle);
        }

        return directionAngle;
    }

    // 월드z축을 기준으로 회전값으로 방향을 구하는 함수
    protected Vector3 GetTargetRotationDirection(float targetAngle)
    {
        return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    }

    // 플레이어의 속도를 리셋시키는 함수
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

    // 기본 회전 데이터 세팅
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
    // 걷기, 달리기 버튼이 눌렸을때 호출되는 콜백 함수
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
