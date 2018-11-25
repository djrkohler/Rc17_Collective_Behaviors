using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "StateMachine/Actions/RanAway")]
public class RunAwayAction : StateMachineAction
{
    public override void Act(StateMachineController controller)
    {
        GetAway(controller);
    }

    void GetAway(StateMachineController controller)
    {
        var closetEnemy = controller.GetClosetEnemy();

        var dir = -(closetEnemy.transform.position - controller.transform.position).normalized;

        controller.transform.Translate(new Vector3(dir.x,0f,dir.z) * controller.parameter.moveSpeed);
    }

}
