using UnityEngine;
using UnityEditor;

namespace GA
{
    public class Food:MonoBehaviour
    {
        public float foodValue = 10f;


        public float Eaten()
        {
            float v = 1f;
            if (foodValue > 0)
            {
                foodValue -= v;
            }
            else
            {
                v = 0f;
            }

            return v;
        }

        public void ResetFood()
        {
            foodValue = 10f;
        }

        public bool isValid()
        {
            return foodValue > 0f;
        }

    }
}