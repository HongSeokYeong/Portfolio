using UnityEngine;

public abstract class StateMachineBase
{
    protected IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();

        currentState = newState;

        currentState.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }

    public void OnAnimationEnterEvent()
    {
        currentState?.OnAnimationEnterEvent();
    }
    public void OnAnimationExitEvent()
    {
        currentState?.OnAnimationEnterEvent();
    }
    public void OnAnimationTransitionEvent()
    {
        currentState?.OnAnimationTransitionEvent();
    }

    public void OnTriggerEnter(Collider collider)
    {
        currentState?.OnTriggerEnter(collider);
    }

    public void OnTriggerExit(Collider collider)
    {
        currentState?.OnTriggerExit(collider);
    }

    public void OnCollisionEnter(Collision collision)
    {
        currentState?.OnCollisionEnter(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        currentState?.OnCollisionExit(collision);
    }
}
