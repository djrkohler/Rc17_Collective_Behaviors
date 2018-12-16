using UnityEngine;
using System.Collections;

namespace GA
{
    public abstract class AgentBase : MonoBehaviour
    {
        public GameObject[] BodyParts;
        public Nature nature;
        public  Behavior behaviorPrefab;
        public  Properties propPrefab;
        [HideInInspector]public Behavior behavior;
        [HideInInspector]public Properties properties;

        [HideInInspector]public float speed = 10f;


        [HideInInspector] public Dna dna;
        [HideInInspector] public float initialHealth;

        public bool dead;

        public bool IsGrouping()
        {
            return behavior.GroupSize() > 0;
        }

        public bool isActive;


        public abstract void Born();

        public abstract void Born(AgentBase d0, AgentBase d1);

        public abstract void UpdateAgent(Enviroment en);

        public abstract void Move(Vector3 dir);

        public abstract void Rotate(Vector3 dir);


        public abstract void SetActive(bool active);

        public void InitialDna()
        {
            dna = new Dna(nature);
        }
        public void InitialDna(Dna d0,Dna d1)
        {
            dna = new Dna(d0,d1);
        }

        public GameObject GetBodyPart(int part)
        {
            return BodyParts[part];
        }
        public bool IsDead()
        {
            return dead;
        }

    }
}

