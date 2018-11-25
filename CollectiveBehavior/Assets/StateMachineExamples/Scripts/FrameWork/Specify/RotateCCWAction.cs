using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Actions/RotateCCW")]
public class RotateCCWAction : StateMachineAction
{
    public override void Act(StateMachineController controller)
    {
       RotateCcw(controller);
    }

    void RotateCcw(StateMachineController controller)
    {
        //if (controller.CoolDownTime(controller.parameter.actionRate))
        {
            controller.transform.Rotate(0, -controller.parameter.rotateSpeed, 0);
        }
    }

}