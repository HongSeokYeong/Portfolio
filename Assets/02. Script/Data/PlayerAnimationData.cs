using UnityEngine;
using System;

// 플레이어 애니메이션 키값들의 해쉬값을 저장해놓는 데이터 클래스
// 애니메이션의 키값과 변수의 값들을 항상 동일하게 관리해서 유지시켜줘야 한다.
[Serializable]
public class PlayerAnimationData
{
    [Header("State Group Parameter Names")]
    [SerializeField] private string groundedParameterName = "Grounded";
    [SerializeField] private string movingParameterName = "Moving";
    [SerializeField] private string stoppingParameterName = "Stopping";
    [SerializeField] private string landingParameterName = "Landing";
    [SerializeField] private string airborneParameterName = "Airborne";
    [SerializeField] private string jumpParamterName = "Jump";

    [Header("Grounded Parameter Names")]
    [SerializeField] private string idleParameterName = "IsIdling";
    [SerializeField] private string dashParameterName = "IsDashing";
    [SerializeField] private string walkParameterName = "IsWalking";
    [SerializeField] private string runParameterName = "IsRunning";
    [SerializeField] private string sprintParameterName = "IsSprinting";
    [SerializeField] private string mediumParameterName = "IsMediumStopping";
    [SerializeField] private string hardStopParameterName = "IsHardStopping";
    [SerializeField] private string rollParameterName = "IsRolling";
    [SerializeField] private string hardLandParameterName = "IsHardLanding";
    [SerializeField] private string attackParameterName = "IsAttack";

    [Header("Airborne Parameter Names")]
    [SerializeField] private string fallParameterName = "IsFalling";

    public int groundedParameterHash { get; private set; }
    public int movingParameterHash { get; private set; }
    public int stoppingParameterHash { get; private set; }
    public int landingParameterHash { get; private set; }
    public int airborneParameterHash { get; private set; }
    public int jumpParameterHash { get; private set; }
    public int idleParameterHash { get; private set; }
    public int dashParameterHash { get; private set; }
    public int walkParameterHash { get; private set; }
    public int runParameterHash { get; private set; }
    public int sprintParameterHash { get; private set; }
    public int mediumStopParameterHash { get; private set; }
    public int hardStopParameterHash { get; private set; }
    public int rollParameterHash { get; private set; }
    public int hardLandParameterHash { get; private set; }
    public int attackParameterHash { get; private set; }
    public int fallParameterHash { get; private set; }

    public void Initialize()
    {
        groundedParameterHash = Animator.StringToHash(groundedParameterName);
        movingParameterHash = Animator.StringToHash(movingParameterName);
        stoppingParameterHash = Animator.StringToHash(stoppingParameterName);
        landingParameterHash = Animator.StringToHash(landingParameterName);
        airborneParameterHash = Animator.StringToHash(airborneParameterName);
        jumpParameterHash = Animator.StringToHash(jumpParamterName);


        idleParameterHash = Animator.StringToHash(idleParameterName);
        dashParameterHash = Animator.StringToHash(dashParameterName);
        walkParameterHash = Animator.StringToHash(walkParameterName);
        runParameterHash = Animator.StringToHash(runParameterName);
        sprintParameterHash = Animator.StringToHash(sprintParameterName);
        mediumStopParameterHash = Animator.StringToHash(mediumParameterName);
        hardStopParameterHash = Animator.StringToHash(hardStopParameterName);
        rollParameterHash = Animator.StringToHash(rollParameterName);
        hardLandParameterHash = Animator.StringToHash(hardLandParameterName);
        attackParameterHash = Animator.StringToHash(attackParameterName);

        fallParameterHash = Animator.StringToHash(fallParameterName);
    }
}
