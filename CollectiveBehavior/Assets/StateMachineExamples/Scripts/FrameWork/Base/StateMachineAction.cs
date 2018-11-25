using UnityEngine;
using UnityEditor;


public abstract class StateMachineAction : ScriptableObject
{
    public abstract void Act(StateMachineController controller);
}