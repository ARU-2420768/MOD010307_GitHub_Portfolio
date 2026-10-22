using UnityEngine;

[CreateAssetMenu(menuName = "Framework/Movement Profile")]

public class MovementProfileData : ScriptableObject
{
    [Header("Movement")]
    public float maxSpeed = 5f;
    public float acceleration = 7f;
    public float deceleration = 7f;
    public bool useSmoothing = true;

    [Header("Bounds")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Rotation")]
    public float maxRotationAngle;
    public float rotationSpeed;

}
