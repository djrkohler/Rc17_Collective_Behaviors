using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Actions/GetClose")]
public class GetCloseAction : StateMachineAction
{
    public override void Act(StateMachineController controller)
    {
        GetClose(controller);
    }

    void GetClose(StateMachineController controller)
    {
        RaycastHit hit;

        if (Physics.SphereCast(controller.transform.position, controller.parameter.sphereCastRadius,
            controller.transform.forward, out hit, controller.parameter.lookRange))
        {
           //if( controller.CoolDownTime(controller.parameter.actionRate))
            {
                controller.transform.Translate((hit.transform.position-controller.transform.position).normalized*controller.parameter.moveSpeed);
            }
        }
    }
}