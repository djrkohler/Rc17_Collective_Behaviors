using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "StateMachine/Decisions/LookSourceAngle")]
public class LookSourceAngleDecision : StateMachineDecision
{
    public override bool Decide(StateMachineController controller)
    {
        bool angleOutRange = IsAngleOutRange(controller);
        return angleOutRange;
    }

    bool IsAngleOutRange(StateMachineController controller)
    {
        bool outRange = false;
        foreach (var s in controller.sources)
        {
            var d = Vector3.Distance(controller.transform.position, s.position);

            if (d > controller.parameter.lookSourceRange)
            {
                continue;
            }

            var a = controller.transform.forward.normalized;
            var b = (s.position-controller.transform.position).normalized;

            DrawAngle(controller.transform.position,a,b);

            float angle = Vector3.Angle(a, b);

            if (s.position.x > controller.transform.position.x)
            {
                angle = -angle;
            }

            if (angle > controller.parameter.maxAngle || angle < controller.parameter.minAngle)
            {
                outRange = true;
            }
        }

        return outRange;
    }

    public static float GetMinimalAngle(Vector3 a, Vector3 b)
    {
        var aa = new Vector3(a.x,0,a.z);
        var bb = new Vector3(b.x,0,b.z);
        float angle_a = Vector3.Angle(aa, bb);
        float angle_b = Vector3.Angle(bb, aa);

        float angle = Mathf.Min(angle_a, angle_b);
        //Debug.Log("a : "+a+", b : "+b+", angle : "+angle);

        return angle;
    }

    public static void DrawAngle(Vector3 start, Vector3 a, Vector3 b)
    {
        Debug.DrawRay(start,a*3f,Color.cyan);
        Debug.DrawRay(start,b*3f,Color.red);
    }
}