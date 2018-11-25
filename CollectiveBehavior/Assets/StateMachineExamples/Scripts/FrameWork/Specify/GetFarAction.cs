using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Actions/GetFar")]
public class GetFarAction : StateMachineAction
{
    public override void Act(StateMachineController controller)
    {
        GetFar(controller);
    }

    void GetFar(StateMachineController controller)
    {
        RaycastHit hit;

        if (Physics.SphereCast(controller.transform.position, controller.parameter.sphereCastRadius,
            controller.transform.forward, out hit, controller.parameter.lookRange))
        {
           //if( controller.CoolDownTime(controller.parameter.actionRate))
            {
                controller.transform.Translate(-(hit.transform.position-controller.transform.position).normalized*controller.parameter.moveSpeed);
            }
        }
        
    }
}