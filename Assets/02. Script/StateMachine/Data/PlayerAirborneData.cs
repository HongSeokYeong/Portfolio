using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerAirborneData
{
    [Header("JumpData")]
    public float jumpToGroundRayDistance = 2f;
    public AnimationCurve jumpForceModifierOnSlopeUpwards;
    public AnimationCurve jumpForceModifierOnSlopeDownwards;
    public Vector3 TargetRotationReachTime;
    public float turnSpeed = 60f;
    public Vector3 stationaryForce;
    public Vector3 weakForce;
    public Vector3 mediumForce;
    public Vector3 strongForce;
    public float decelerationForce = 1.5f;

    [Header("FallData")]
    public float fallSpeedLimit = 10f;
    public float minimumDistanceToBeConsideredHardFall = 3f;
}
