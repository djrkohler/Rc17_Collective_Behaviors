using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GA
{
    [CreateAssetMenu(menuName = "Genetic/Nature")]
    public class Nature : ScriptableObject
    {
        public BoundRange[] Bound;

        public RaceType _type;

        public float healthBase;
        public float strengthBase;
        public float speedBase;
        public float flexibilityBase;
        public float harmBase;

        public Vector3[] GetCode()
        {
            var code = new Vector3[Bound.Length];

            for (int i = 0; i < Bound.Length; i++)
            {
                code[i] = Bound[i].GetVector();
            }
            return code;
        }

        public BoundRange[] BoundInstance()
        {
            BoundRange[] b = new BoundRange[Bound.Length];
            for(int i = 0; i < Bound.Length; i++)
            {
                b[i] = new BoundRange(Bound[i]);
            }
            return b;

        }

    }
}