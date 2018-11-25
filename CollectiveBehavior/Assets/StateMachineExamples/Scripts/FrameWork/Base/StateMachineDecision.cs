using UnityEngine;
using UnityEditor;

public abstract class StateMachineDecision : ScriptableObject
{
    public abstract bool Decide(StateMachineController controller);
}