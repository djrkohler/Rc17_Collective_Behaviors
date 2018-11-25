using UnityEngine;
using UnityEditor;

[System.Serializable]
public class StateMachineTransition
{
    public StateMachineDecision Decision;
    public StateMachineState trueState;
    public StateMachineState falseState;
}