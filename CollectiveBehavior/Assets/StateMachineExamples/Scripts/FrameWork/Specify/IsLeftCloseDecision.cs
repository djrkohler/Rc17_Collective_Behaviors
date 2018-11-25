using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StateMachine/Decisions/IsLeftCloseDecision")]
public class IsLeftCloseDecision : StateMachineDecision
{
    public override bool Decide(StateMachineController controller)
    {
        return IsClose(controller);
    }

    bool IsClose(StateMachineController controller)
    {
        bool close = false;
        var p = controller.transform.TransformPoint(Vector3.left*0.5f);
        RaycastHit hit;

        float rayLength = 0f;

        if (Physics.Raycast(p, controller.transform.forward, out hit, controller.parameter.lookRange) && hit.transform.CompareTag("Blocks"))
        {
            if (hit.distance < controller.parameter.nearDistance)
            {
                close = true;
            }

            rayLength = hit.distance;
        }
        else
        {
            rayLength = controller.parameter.lookRange;
        }

        Debug.DrawRay(p, controller.transform.forward * rayLength, controller.currentState.stateColor);

        return close;
    }

}
