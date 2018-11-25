using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Decisions/LookNeighborDistance")]
public class LookNeighborDistanceDecision : StateMachineDecision
{
    public override bool Decide(StateMachineController controller)
    {
        bool neighborOutRange = IsNeighborCloseOrFar(controller);
        return neighborOutRange;
    }

    bool IsNeighborCloseOrFar(StateMachineController controller)
    {
        bool neighborDistOutRange = false;
        RaycastHit hit;
        float rayLength = 0f;

        if (Physics.SphereCast(controller.transform.position, controller.parameter.sphereCastRadius,
            controller.transform.forward, out hit, controller.parameter.lookRange)&&hit.transform.CompareTag("Blocks"))
        {
            if (hit.distance > controller.parameter.farDistance || hit.distance < controller.parameter.nearDistance)
            {
                neighborDistOutRange = true;
            }

            rayLength = hit.distance;
        }
        else
        {
            rayLength = controller.parameter.lookRange;
        }

        Debug.DrawRay(controller.transform.position, controller.transform.forward * rayLength, controller.currentState.stateColor);

        return neighborDistOutRange;
    }
}