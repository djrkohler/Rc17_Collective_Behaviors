using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Decisions/IsTooFar")]
public class IsTooFarDecision : StateMachineDecision
{
    public override bool Decide(StateMachineController controller)
    {
        bool neighborTooFar = IsNeighborTooFar(controller);
        return neighborTooFar;
    }

    bool IsNeighborTooFar(StateMachineController controller)
    {
        bool tooFar = false;
        RaycastHit hit;
        float rayLength = 0f;

        if (Physics.SphereCast(controller.transform.position, controller.parameter.sphereCastRadius,
            controller.transform.forward, out hit, controller.parameter.lookRange)&&hit.transform.CompareTag("Blocks"))
        {
            if (hit.distance > controller.parameter.farDistance)
            {
                tooFar = true;
            }

            rayLength = hit.distance;
        }
        else
        {
            rayLength = controller.parameter.lookRange;
        }

        Debug.DrawRay(controller.transform.position, controller.transform.forward *rayLength, Color.blue);

        return tooFar;
    }
}