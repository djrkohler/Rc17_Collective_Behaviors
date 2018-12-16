using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RC17.BlockChain
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(HingeJoint))]
    public class Motor : MonoBehaviour
    {
        HingeJoint joint;

        public HingeJoint GetJoint()
        {
            return joint;
        }

        /// <summary>
        /// Set the direction of rotation(1,-1)
        /// </summary>
        /// <param name="dir"></param>
        public void SetRotation(Block block)
        {
            joint = GetComponent<HingeJoint>();

            var m = joint.motor;

            m.targetVelocity = block.parameter.GetVelocity();
            m.force = block.parameter.GetForce();

            joint.motor = m;
        }
    }
}

