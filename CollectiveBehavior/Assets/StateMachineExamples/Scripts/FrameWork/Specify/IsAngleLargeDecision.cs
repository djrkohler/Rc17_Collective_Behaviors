using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Decisions/AngleTooLarge")]
public class IsAngleLargeDecision : StateMachineDecision
{
    public override bool Decide(StateMachineController controller)
    {
        bool angleLarge = IsAngleLarge(controller);
        return angleLarge;
    }

    bool IsAngleLarge(StateMachineController controller)
    {
        bool large = false;
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

            //if (angle > 360f)
            //{
            //    angle = 360f - angle;
            //}

            if (s.position.x > controller.transform.position.x)
            {
                angle = -angle;
            }



            if (angle > controller.parameter.maxAngle)
            {
                large = true;
            }
        }

        return large;
    }

   
}