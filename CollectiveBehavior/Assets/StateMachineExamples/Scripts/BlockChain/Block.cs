using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RC17.BlockChain
{

    public class Block : MonoBehaviour
    {

        public Dictionary<HookType, HookType> pattern = new Dictionary<HookType, HookType>();

        public Whole whole;

        public int index { get; set; }

        public Transform arm;

        public Motor motor;

        

        public ChainParameter parameter;

        [HideInInspector] public int chain = 0;
        [HideInInspector] public int connection = 0;



        /// <summary>
        /// set start direction of block
        /// </summary>
        public void Initialize()
        {
            motor.SetRotation(this);
        }

        public int ChainCount()
        {
            int count = 0;
            if (whole != null)
            {
                count = whole.elements.Count;
            }

            return count;
          
        }

        public float CurrentForce()
        {
            return motor.GetJoint().currentForce.magnitude;
        }

        public float CurrentTorque()
        {
            return motor.GetJoint().currentTorque.magnitude;
        }


        public int GetDir(float d)
        {
            int dir;
            if (d > 0)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }
            return dir;
        }

        public void UpdateColor(float t,Color a,Color b)
        {
            var renderer0 = motor.GetComponent<MeshRenderer>();
            var renderer1 = arm.GetComponent<MeshRenderer>();

            renderer0.material.color = renderer1.material.color=Color.Lerp(a, b, t);
        }
       
    }
}