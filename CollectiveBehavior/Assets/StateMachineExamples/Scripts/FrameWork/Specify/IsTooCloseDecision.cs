using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Decisions/IsTooClose")]
public class IsTooCloseDecision : StateMachineDecision
{
    public override bool Decide(StateMachineController controller)
    {
        bool neighborTooClose = IsNeighborTooClose(controller);
        return neighborTooClose;
    }

    bool IsNeighborTooClose(StateMachineController controller)
    {
        bool tooClose = false;
        RaycastHit hit;
        Debug.DrawRay(controller.transform.position,controller.transform.forward*controller.parameter.lookRange,Color.red);

        if (Physics.SphereCast(controller.transform.position, controller.parameter.sphereCastRadius,
            controller.transform.forward, out hit, controller.parameter.lookRange)&& hit.transform.CompareTag("Blocks"))
        {
            if (hit.distance < controller.parameter.nearDistance)
            {
                tooClose = true;
            }
        }

        return tooClose;
    }
}