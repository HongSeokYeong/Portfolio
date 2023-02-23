using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReusableData
{
    public Vector2 movementInput { get; set; }
    public float movementSpeedModifier { get; set; } = 1f;
    public float movementOnSlopesSpeedModifier { get; set; } = 1f;
    public float movementDecelerationForce { get; set; } = 1f;
    public Vector3 TargetRotationReachTime { get; private set; }
    public float turnSpeed { get; private set; } = 60f;

    public Vector3 CurrentJumpForce { get; set; }

    public bool shouldWalk { get; set; }
    public bool shouldSprint { get; set; }

    private Vector3 currentTargetRotation;
    private Vector3 timeToReachTargetRotation;
    private Vector3 dampedTargetRotationCurrentVelocity;
    private Vector3 dampedTargetRotationPassedTime;
    public ref Vector3 CurrentTargetRotation
    {
        get
        {
            return ref currentTargetRotation;
        }
    }

    public ref Vector3 TimeToReachTargetRotation
    {
        get
        {
            return ref timeToReachTargetRotation;
        }
    }
    public ref Vector3 DampedTargetRotationCurrentVelocity
    {
        get
        {
            return ref dampedTargetRotationCurrentVelocity;
        }
    }
    public ref Vector3 DampedTargetRotationPassedTime
    {
        get
        {
            return ref dampedTargetRotationPassedTime;
        }
    }
}
