using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/StateMachineState")]
public class StateMachineState : ScriptableObject
{
    public StateMachineAction[] actions;
    public StateMachineTransition[] transitions;
    public Color stateColor = Color.grey;


    public void UpdateState(StateMachineController controller)
    {
            DoAction(controller);
    }

    void DoAction(StateMachineController controller)
    {
        if (controller.CoolDownTime(controller.parameter.actionRate))
        {
            foreach (var action in actions)
            {
                action.Act(controller);
            }

            controller.OnActionDone();
        }
          
        CheckTransition(controller);
    }

    void CheckTransition(StateMachineController controller)
    {
        foreach (var transition in transitions)
        {
            var toTransition = transition.Decision.Decide(controller);
            if (toTransition)
            {
                controller.TransitionToState(transition.trueState);
            }
            else
            {
                controller.TransitionToState(transition.falseState);
            }
        }
    }
}