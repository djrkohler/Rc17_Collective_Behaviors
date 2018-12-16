using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GA
{
    public class FlockAgent : AgentBase
    {
        public GameObject head { get; set; }
        public GameObject body { get; set; }
        public GameObject tail { get; set; }

        public override void Born()
        {
            InitialDna();

            for (int i = 0; i < BodyParts.Length; i++)
            {
                BodyParts[i].transform.localScale = dna.GetCode()[i];
            }
            head = BodyParts[0];
            body = BodyParts[1];
            tail = BodyParts[2];

            behavior = Instantiate(behaviorPrefab);
            properties = Instantiate(propPrefab);

            properties.SetProperties(this);
            initialHealth = properties.health;
            SetActive(true);

        }

        public override void SetActive(bool active)
        {
            isActive = active;
            foreach(var p in BodyParts)
            {
                p.SetActive(active);
            }
        }
        public override void Born(AgentBase d0, AgentBase d1)
        {
            nature = d0.nature;
            InitialDna(d0.dna, d1.dna);

            for (int i = 0; i < BodyParts.Length; i++)
            {
                BodyParts[i].transform.localScale = dna.GetCode()[i];
            }
            head = BodyParts[0];
            body = BodyParts[1];
            tail = BodyParts[2];
            properties.SetProperties(this);
            initialHealth = properties.health;
            SetActive(true);
           
        }

        public override void UpdateAgent(Enviroment en)
        {
            behavior.UpdateBehavior(this, en);
           
        }

        public override void Move(Vector3 vector)
        {
            var rig = head.GetComponent<Rigidbody>();

            rig.MovePosition(head.transform.position + vector);

      
        }

        public override void Rotate(Vector3 dir)
        {
            var rig = head.GetComponent<Rigidbody>();

            var q = Quaternion.Slerp(head.transform.rotation, Quaternion.LookRotation(dir), properties.flexibility * Time.deltaTime);

          
            rig.MoveRotation(q);
        }

        public void Eat(Food food)
        {
            properties.score += food.Eaten();
        }
    }
}