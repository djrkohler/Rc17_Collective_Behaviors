using UnityEngine;
using System.Collections;


namespace GA
{
    public abstract class Properties : ScriptableObject
    {
        [HideInInspector]public float health;
        [HideInInspector] public float strength;
        [HideInInspector] public float speed;
        [HideInInspector] public float flexibility;
        [HideInInspector] public float lifeTime;
        [HideInInspector] public float harm;

        [HideInInspector] public float score=0f;

       [HideInInspector]public bool set;

      public Properties Instance()
        {
            return this;
        }

        public abstract void SetProperties(AgentBase agent);

    }
}
