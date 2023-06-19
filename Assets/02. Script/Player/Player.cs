using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

// TODO : 이 클래스의 주석처리 되어있는 것들 필요한지 확인해보고 삭제
public class Player : MonoBehaviour
{
    public PlayerDataScriptableObject playerData;

    // 플레이어 충돌 관리 유틸 클래스 인데 필요한가?
    //[field: SerializeField] public PlayerCapsuleColliderUtility colliderUtility { get; private set; }

    public LayerMask groundLayer;

    // public CharacterController characterController;
    public Rigidbody rigidbody;
    public Animator animator;

    public Transform mainCameraTransform;

    public PlayerInput input;

    // 카메라 쉐이크 필요한가?
    //public CameraShake CameraShake { get; private set; }    // 카메라 흔들림 효과

    public PlayerAnimationData animationdata;

    private PlayerStateMachine playerStateMachine;

    public Transform cameraLookPoint;

    public CinemachineBrain cinemachineBrain;

    public PlayerReusableData reusableData;

    protected virtual void Awake()
    {
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        //colliderUtility.Initialize(gameObject);
        //colliderUtility.CalculateCapsuleColliderDimenstions();

        reusableData = new PlayerReusableData();

        animationdata.Initialize();
        playerStateMachine = new PlayerStateMachine(this);
    }

    protected virtual void Start()
    {
        mainCameraTransform = Camera.main.transform;

        playerStateMachine?.ChangeState(playerStateMachine.idleState);
    }

    private void OnValidate()
    {
        //colliderUtility.Initialize(gameObject);
        //colliderUtility.CalculateCapsuleColliderDimenstions();
    }

    private void Update()
    {
        playerStateMachine?.HandleInput();

        playerStateMachine?.Update();
    }

    private void FixedUpdate()
    {
        playerStateMachine?.PhysicsUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerStateMachine?.OnCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        playerStateMachine?.OnCollisionExit(collision);  
    }

    private void OnTriggerEnter(Collider collider)
    {
        playerStateMachine?.OnTriggerEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        playerStateMachine?.OnTriggerExit(collider);
    }

    public virtual void OnMovementStateAnimationEnterEvent()
    {
        playerStateMachine?.OnAnimationEnterEvent();
    }

    public virtual void OnMovementStateAnimationExitEvent()
    {
        playerStateMachine?.OnAnimationExitEvent();
    }

    public virtual void OnMovementStateAnimationTransitionEvent()
    {
        playerStateMachine?.OnAnimationTransitionEvent();
    }

    public bool IsGroundLayer(int layer)
    {
        return Util.ContainsLayer(groundLayer, layer);
    }

    private void OnAnimatorMove()
    {
        //Vector3 velocity = animator.deltaPosition;
        //velocity.y = 0.0f;
        //// 캐릭터 컨트롤러를 사용한다면
        //rigidbody.Move(velocity);
        //transform.rotation *= animator.deltaRotation;
    }
}
