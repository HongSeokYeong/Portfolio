using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Player : MonoBehaviour
{
    public PlayerDataScriptableObject playerData;

    public LayerMask groundLayer;

    public Rigidbody rigidbody;
    public Animator animator;

    public Transform mainCameraTransform;

    public PlayerInput input;

    public PlayerAnimationData animationdata;

    private PlayerStateMachine playerStateMachine;

    public Transform cameraLookPoint;

    public CinemachineBrain cinemachineBrain;

    public PlayerReusableData reusableData;

    protected virtual void Awake()
    {
        input = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        reusableData = new PlayerReusableData();

        animationdata.Initialize();
        playerStateMachine = new PlayerStateMachine(this);
    }

    protected virtual void Start()
    {
        mainCameraTransform = Camera.main.transform;

        playerStateMachine?.ChangeState(playerStateMachine.idleState);
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
}
