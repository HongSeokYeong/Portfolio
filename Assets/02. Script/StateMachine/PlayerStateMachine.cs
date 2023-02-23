using UnityEngine;

public class PlayerStateMachine : StateMachineBase
{
    public Player player;

    public PlayerIdleState idleState;
    public PlayerWalkState walkState;
    public PlayerRunState runState;

    public PlayerWalkStopState walkStopState;
    public PlayerRunStopState runStopState;

    public PlayerStateMachine(Player character)
    {
        this.player = character;

        idleState = new PlayerIdleState(this);
        walkState = new PlayerWalkState(this);
        runState = new PlayerRunState(this);

        walkStopState = new PlayerWalkStopState(this);
        runStopState = new PlayerRunStopState(this);
    }
}
