using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    #region IState Methods
    // ���̵� ������Ʈ�� �����ϸ� �̵��ӵ� ��ȭ���� 0���� �����Ѵ�.
    // ������ �ٵ��� �ӵ��� 0���� �����Ѵ�.
    public override void Enter()
    {
        stateMachine.player.reusableData.movementSpeedModifier = 0f;

        base.Enter();

        StartAnimation(stateMachine.player.animationdata.idleParameterHash);

        stateMachine.player.reusableData.CurrentJumpForce = airborneData.stationaryForce;

        ResetVelocity();
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.player.animationdata.idleParameterHash);
    }

    // ���̵� ������Ʈ������ �̵�Ű�� �ԷµǱ� ��ٷȴٰ�
    // �̵�Ű�� �ԷµǸ� �̵� ������Ʈ�� �ѱ��.
    public override void Update()
    {
        base.Update();

        stateMachine.player.animator.SetFloat("MoveSpeed", 0, 0.25f, Time.deltaTime);
        //stateMachine.player.animator.SetFloat("MoveSpeed", stateMachine.player.characterController.velocity.magnitude, 0.25f, Time.deltaTime);

        if (stateMachine.player.reusableData.movementInput == Vector2.zero)
        {
            return;
        }

        OnMove();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!IsMovingHorizontally())
        {
            return;
        }

        ResetVelocity();
    }
    #endregion
}
