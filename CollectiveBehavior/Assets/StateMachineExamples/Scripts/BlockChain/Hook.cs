using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RC17.BlockChain
{
    public class Hook : MonoBehaviour
    {
        public HookType type;

        public Block block;

        public Transform arm;

        ConfigurableJoint[] configurableJoints = new ConfigurableJoint[1];

      
        [HideInInspector] bool hooking = false;

        public void OnTriggerEnter(Collider other)
        {
            if (!other.transform.CompareTag("hook")||other.transform.GetComponent<ConfigurableJoint>())
            {
                return;
            }

            var hook = other.transform.GetComponent<Hook>();

            if (block.connection > block.parameter.MaxConnection||hooking)
            {
                return;
            }

            if (block.pattern.ContainsKey(type))
            {
                if (hook.type != block.pattern[type]&&block.chain<block.parameter.patternCount)
                {
                    return;
                }
            }

            if (block.whole == null)
            {
                if(hook.block.whole == null)
                {
                    var w = new Whole();
                    w.AddElement(block.index);
                    w.AddElement(hook.block.index);
                    block.whole =hook.block.whole= w;
                }
                else
                {
                    hook.block.whole.AddElement(block.index);
                    block.whole = hook.block.whole;
                }
            }
            else
            {
                if (hook.block.whole == null)
                {
                    block.whole.AddElement(hook.block.index);
                    hook.block.whole = block.whole;
                   
                }
                else
                {
                    block.whole.AddElement(hook.block.whole.elements.ToArray());

                    
                    hook.block.whole = block.whole;
                }
            }

            if (!block.pattern.ContainsKey(type))
            {
                block.pattern.Add(type, hook.type);
            }
            else
            {
                if (block.chain > block.parameter.patternCount)
                {
                    block.pattern[type] = hook.type;
                    block.chain=0;
                }
            }

            if (!hook.block.pattern.ContainsKey(type))
            {
                hook.block.pattern.Add(type, hook.type);
            }
        
  
            var body = hook.arm.GetComponent<Rigidbody>();

            HookOn(body);
            
            hook.block.chain=block.chain+1;

            hook.block.connection = block.connection + 1;
        }

      

        void HookOn(Rigidbody body)
        {
           var joint=arm.gameObject.AddComponent<ConfigurableJoint>();

            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Free;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            joint.connectedBody = body;
            configurableJoints[0] = joint;

            hooking = true;
        }

        void Release()
        {
            Destroy(configurableJoints[0]);
            hooking = false;
        }
    }
}

public enum HookType
{
    a,b,c,d
}