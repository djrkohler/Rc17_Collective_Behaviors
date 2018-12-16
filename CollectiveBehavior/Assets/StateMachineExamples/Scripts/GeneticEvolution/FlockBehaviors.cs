using UnityEngine;
using System.Collections;
using System.Linq;

namespace GA
{
    [CreateAssetMenu(menuName = "Genetic/FlockBehaviors")]
    public class FlockBehaviors : Behavior
    {
      
        Vector3 toAvoid = Vector3.zero;
        Vector3 toAttack = Vector3.zero;
        public Color lookColor = Color.green;
        public float lookRange = 3f;


        bool flocking;
        public bool IsFlocking()
        {
            return flocking;
        }
      

        public override void UpdateBehavior(AgentBase agent, Enviroment en)
        {
            
            var p = agent.BodyParts[0].transform.forward * agent.speed * Time.deltaTime;

            agent.Move(p);
            LookAt(agent,LookDirection.forward);
            LookAt(agent, LookDirection.left);
            LookAt(agent, LookDirection.right);
            Flocking(agent, en);
            ChangeSpeed(agent);
        }

        

        void LookAt(AgentBase agent,LookDirection dir)
        {
            var head = agent.BodyParts[0].transform;

            Vector3 lookdir = Vector3.zero;
            //toAvoid = Vector3.zero;

            switch (dir)
            {
                case LookDirection.forward:
                    lookdir = head.forward;
                    break;
                case LookDirection.left:
                    lookdir = -head.right;
                    break;
                case LookDirection.right:
                    lookdir = head.right;
                    break;
                case LookDirection.back:
                    lookdir = -head.forward;
                    break;
            }

           
            //Gizmos.DrawSphere(head.position + head.forward * lookRange, 2f);

            RaycastHit hit;

            //bool b = Physics.Raycast(agent.BodyParts[0].transform.position, agent.BodyParts[0].transform.forward, out hit, lookRange);

            bool b = Physics.SphereCast(head.position, 2f, lookdir, out hit, lookRange);

            if (b == true)
            {
                var food = hit.transform.GetComponent<Food>();
                var fa = agent.GetComponent<FlockAgent>();

                if (food&&fa)
                {
                    fa.Eat(food);
                }

                Debug.DrawRay(head.position, lookdir * hit.distance, lookColor);

                if (hit.transform.CompareTag("Objects")&& ! agent.BodyParts.ToList().Contains(hit.transform.gameObject))
                {
                    var ai = hit.transform.GetComponent<AgentBase>();

                    if (ai && ai.dna.GetType() == agent.dna.GetType())
                    {
                        return;
                    }
                    else
                    {
                        if (ai)
                        {
                            var enemyValue = ai.properties.health * ai.behavior.GroupSize();
                            var agentValue = agent.properties.health * agent.behavior.GroupSize();

                            if (agentValue > enemyValue)
                            {
                                //toAttack = hit.point;//ai.BodyParts[0].transform.position - agent.BodyParts[0].transform.position;
                                Attack(agent, ai);
                            }
                            else
                            {
                                toAvoid =  - hit.point;
                            }
                        }
                        else
                        {
                            toAvoid =  - hit.point;
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(head.position, lookdir * lookRange, lookColor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        void Flocking(AgentBase agent, Enviroment en)
        {
           
            Vector3 centre = Vector3.zero;
            Vector3 avoid = Vector3.zero;
            float fSpeed = 1f;
            float nDistance;
            groupSize = 0;
            foreach (var a in en.GetAllFlock())
            {
                if (a == agent)
                {
                    continue;
                }

                nDistance = Vector3.Distance(a.BodyParts[0].transform.position, agent.BodyParts[0].transform.position);
                if (nDistance <= en._neighborDistance)
                {
                    if (a.dna.GetType() == agent.dna.GetType())
                    {
                        centre += a.BodyParts[0].transform.position;
                        groupSize++;

                        var v = a.speed;
                        fSpeed += v;

                        if (nDistance < en._avoidDistance)
                        {
                            avoid += (agent.BodyParts[0].transform.position - a.BodyParts[0].transform.position);
                        }
                    }
                    else
                    {
                        centre += (agent.BodyParts[0].transform.position - a.BodyParts[0].transform.position);
                    }
                }
            }
            if (groupSize > 0)
            {
                centre = (centre) / groupSize + en.GetFoodTarget(agent)+en.GetGoalPos(agent);// + toAttack;
               agent.speed = Mathf.Clamp(fSpeed / groupSize, 1f, agent.properties.speed);

                Vector3 dir = (centre + avoid) - agent.BodyParts[0].transform.position + toAvoid;

                if (dir != Vector3.zero)
                {
                    agent.Rotate(dir);
                    flocking = true;
                    Debug.DrawRay(agent.BodyParts[0].transform.position, dir.normalized * lookRange * 2f, Color.red);
                }
            }
            else
            {
                flocking = false;
                var d = toAvoid + en.GetFoodTarget(agent);//+ toAttack;
                agent.Rotate(d);
            }
        }

        /// <summary>
        /// change Speed by probability
        /// </summary>
        void ChangeSpeed(AgentBase agent)
        {
            if (Random.Range(0, 100) < 50)
            {
                agent.speed = Random.Range(0f, agent.properties.speed);
            }
        }

        void Attack(AgentBase agent, AgentBase enemy)
        {
            var h = agent.properties.harm;
            enemy.properties.score -= h;
            agent.properties.score += 0.5f * h;
        }

        //public void DrawLine(Vector3 point)
        //{
        //    var line = GetComponent<LineRenderer>();
        //    line.positionCount = linePointIndex + 1;
        //    line.SetPosition(linePointIndex, point);

        //    linePointIndex++;
        //}
    }
}

public enum LookDirection
{
    forward,back,left,right
}