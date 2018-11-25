using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "StateMachine/Decisions/CheckEnemyDistance")]
public class CheckEnemyDistanceDecision:StateMachineDecision
{
    public override bool Decide(StateMachineController controller)
    {
        return IsEnemyClose(controller);
    }

    bool IsEnemyClose(StateMachineController controller)
    {
        bool close = false;
        foreach(var en in controller.enemys)
        {
            var dist = Vector3.Distance(en.position, controller.transform.position);

            if (dist < controller.parameter.enemySafeDistance)
            {
                close = true;
                Debug.Log("enemy too close");
            }
        }
        return close;
    }

}
