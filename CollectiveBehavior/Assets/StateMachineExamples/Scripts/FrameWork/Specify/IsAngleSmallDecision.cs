using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Decisions/AngleTooSmall")]
public class IsAngleSmallDecision : StateMachineDecision
{
    public override bool Decide(StateMachineController controller)
    {
        bool angleSmall = IsAngleSmall(controller);
        return angleSmall;
    }

    bool IsAngleSmall(StateMachineController controller)
    {
        bool small = false;
        foreach (var s in controller.sources)
        {
            var d = Vector3.Distance(controller.transform.position, s.position);

            if (d > controller.parameter.lookSourceRange)
            {
                continue;
            }

            var a = controller.transform.forward.normalized;
            var b = (s.position - controller.transform.position).normalized;

            LookSourceAngleDecision.DrawAngle(controller.transform.position, a, b);

            float angle = Vector3.Angle(a, b);

            if (s.position.x > controller.transform.position.x)
            {
                angle = -angle;
            }


            if (angle < controller.parameter.minAngle)
            {
                small = true;
            }
        }

        return small;
    }

   
}