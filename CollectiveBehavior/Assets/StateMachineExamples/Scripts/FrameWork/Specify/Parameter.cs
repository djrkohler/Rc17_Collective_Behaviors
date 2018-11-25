using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Parameter")]
public class Parameter : ScriptableObject
{
    public float nearDistance;
    public float farDistance;
    public float minAngle;
    public float maxAngle;
    public float sphereCastRadius;
    public float moveSpeed;
    public float rotateSpeed;
    public float actionRate;
    public float lookRange;
    public float lookSourceRange;
    public float enemySafeDistance;
}
