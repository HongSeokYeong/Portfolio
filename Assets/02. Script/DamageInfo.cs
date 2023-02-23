using UnityEngine;

public class DamageInfo
{
    public Vector3 location;
    public Vector3 direction;
    public float damage;
    public Collider bodyPart;
    public GameObject origin;

    public DamageInfo(Vector3 location, Vector3 direction, float damage, Collider bodyPart = null, GameObject origin = null)
    {
        this.location = location;
        this.direction = direction;
        this.damage = damage;
        this.bodyPart = bodyPart;
        this.origin = origin;
    }
}