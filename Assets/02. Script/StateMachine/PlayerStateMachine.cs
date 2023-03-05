using UnityEngine;

public class PlayerStateMachine : StateMachineBase
{
    public Player player;

    public PlayerIdleState idleState;
    public PlayerWalkState walkState;
    public PlayerRunState runState;

    public PlayerJumpState jumpState;
    public PlayerFallState fallState;

    public PlayerLandingState landingState;

    public PlayerWalkStopState walkStopState;
    public PlayerRunStopState runStopState;

    public PlayerStateMachine(Player character)
    {
        this.player = character;

        idleState = new PlayerIdleState(this);
        walkState = new PlayerWalkState(this);
        runState = new PlayerRunState(this);

        jumpState = new PlayerJumpState(this);
        fallState = new PlayerFallState(this);
        landingState = new PlayerLandingState(this);

        walkStopState = new PlayerWalkStopState(this);
        runStopState = new PlayerRunStopState(this);
    }
}
