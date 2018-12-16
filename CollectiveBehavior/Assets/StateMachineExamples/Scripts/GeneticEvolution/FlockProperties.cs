using UnityEngine;
using System.Collections;


namespace GA
{
    [CreateAssetMenu(menuName = "Genetic/FlockProperty")]
    public class FlockProperties : Properties
    {
        public override void SetProperties(AgentBase agent)
        {
            var head = agent.BodyParts[0].transform.localScale;
            var body = agent.BodyParts[1].transform.localScale;
            var tail = agent.BodyParts[2].transform.localScale;

            health = agent.nature.healthBase + body.x + body.y + body.z;
            strength = agent.nature.strengthBase + body.x + body.y + body.z;
            speed = agent.nature.speedBase + tail.z + tail.y - tail.x + 0.1f * strength;
            flexibility = agent.nature.flexibilityBase + Mathf.Clamp(head.z + head.y - body.z - body.x,-(agent.nature.flexibilityBase-1), agent.nature.flexibilityBase*0.5f);
            harm = agent.nature.harmBase + 0.1f * strength;
            score = 0f;

            set = true;
        }
    }
}