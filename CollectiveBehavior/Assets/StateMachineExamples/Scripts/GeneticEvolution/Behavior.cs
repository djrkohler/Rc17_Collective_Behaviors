using UnityEngine;
using System.Collections;

namespace GA
{
    public abstract class Behavior : ScriptableObject
    {
       [HideInInspector] public float groupSize;

        public float GroupSize()
        {
            return groupSize;
        }
        public abstract void UpdateBehavior(AgentBase agent, Enviroment en);

    }
}