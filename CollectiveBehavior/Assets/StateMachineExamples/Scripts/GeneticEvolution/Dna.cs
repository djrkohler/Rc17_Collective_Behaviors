using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace GA
{
    public class Dna
    {
        public Vector3[] Code;

        BoundRange[] Bound;

        RaceType _type;

        public RaceType GetType()
        {
            return _type;
        }
        public Vector3[] GetCode()
        {
            return Code;
        }

        public Dna(Nature nature)
        {
            Code = nature.GetCode();

            Bound = nature.BoundInstance();

            nature.Bound.CopyTo(Bound, 0);

            _type = nature._type;
        }
        public Dna(Dna d0, Dna d1)
        {
            var parDna = ParentDna(d0, d1);

            _type = d0.GetType();
            Bound = d0.Bound;

            Bound = d0.BoundInstance();

            Code = new Vector3[Bound.Length];

            for (int i = 0; i < Bound.Length; i++)
            {
                int tx = UnityEngine.Random.Range(0, 2);
                int ty = UnityEngine.Random.Range(0, 2);
                int tz = UnityEngine.Random.Range(0, 2);

                var x = parDna[tx][i].x;
                var y = parDna[ty][i].y;
                var z = parDna[tz][i].z;

                var v = new Vector3(x, y, z);

                v = GetMutation(i, "X", 0.1f, v);
                v = GetMutation(i, "Y", 0.1f, v);
                v = GetMutation(i, "Z", 0.1f, v);

                Code[i] = v;
            }
        }

        List<Vector3[]> ParentDna(Dna d0, Dna d1)
        {
            var lst = new List<Vector3[]>() { d0.GetCode(), d1.GetCode() };
            return lst;
        }

        Vector3 GetMutation(int part, string axis, float amout, Vector3 original)
        {
            Vector3 m = original;

            var prob = UnityEngine.Random.Range(0, 100f);
            if (prob > 80f)
            {
                m = Bound[part].Mutation(axis, amout, original);
            }
            return m;
        }
        public BoundRange[] BoundInstance()
        {
            BoundRange[] b = new BoundRange[Bound.Length];
            for (int i = 0; i < Bound.Length; i++)
            {
                b[i] = new BoundRange(Bound[i]);
            }
            return b;

        }
    }

   
}