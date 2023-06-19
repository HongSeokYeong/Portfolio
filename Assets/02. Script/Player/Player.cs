using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

// TODO : �� Ŭ������ �ּ�ó�� �Ǿ��ִ� �͵� �ʿ����� Ȯ���غ��� ����
public class Player : MonoBehaviour
{
    public PlayerDataScriptableObject playerData;

    // �÷��̾� �浹 ���� ��ƿ Ŭ���� �ε� �ʿ��Ѱ�?
    //[field: SerializeField] public PlayerCapsuleColliderUtility colliderUtility { get; private set; }

    public LayerMask groundLayer;

    // public CharacterController characterController;
    public Rigidbody rigidbody;
    public Animator animator;

    public Transform mainCameraTransform;

    public PlayerInput input;

    // ī�޶� ����ũ �ʿ��Ѱ�?
    //public CameraShake CameraShake { get; private set; }    // ī�޶� ��鸲 ȿ��

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
        //// ĳ���� ��Ʈ�ѷ��� ����Ѵٸ�
        //rigidbody.Move(velocity);
        //transform.rotation *= animator.deltaRotation;
    }
}
