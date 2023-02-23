using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 캐릭터 스크립터블 오브젝트에서 사용될 데이터 클래스
// 캐릭터의 움직임 관련한 데이터들을 묶은 클래스이다.
[Serializable]
public class PlayerGroundedData
{
    public float baseSpeed;
    public float groundToFallRayDistance;

    public AnimationCurve slopeSpeedAngles;
    #region Base Rotation
    public Vector3 baseTargetRotationReachTime;
    public float baseTurnSpeed = 60f;
    #endregion

    #region Walk Data
    [Header("Walk Data")]
    public float walkSpeedModifier = 0.225f;
    #endregion

    #region Run Data
    [Header("Run Data")]
    public float runSpeedModifier = 1f;
    #endregion

    #region Dash Data
    [Header("Dash Data")]
    public float dashSpeedModifier = 2f;
    public Vector3 dashTargetRotationReachTime;
    public float dashTurnSpeed = 60f;
    public float dashTimeToBeConsideredConsecutive = 1f;
    public int dashConsecutiveDashesLimitAmount = 2;
    public float dashLimitReachedCooldown = 1.75f;
    #endregion

    #region Sprint Data
    [Header("Sprint Data")]
    public float sprintSpeedModifier = 1.7f;
    public float sprintToRunTime = 1f;
    public float runToWalkTime = 0.5f;
    #endregion

    #region Stop Data
    [Header("Stop Data")]
    public float walkStopDecelerationForce = 5f;
    public float runStopDecelerationForce = 6.5f;
    public float hardStopDecelerationForce = 5f;
    #endregion

    #region Roll Data
    [Header("Roll Data")]
    public float rollSpeedModifier = 1f;
    #endregion

}
