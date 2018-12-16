using UnityEngine;
using UnityEditor;

namespace RC17.BlockChain
{
    [CreateAssetMenu(menuName = "BlockChain/ChainParameter")]
    public class ChainParameter : ScriptableObject
    {
        public int patternCount = 10;
        public int MaxConnection = 6;
        public float MaxSpeed = 50f;
        public float MinSpeed = -50f;
        public float MaxForce = 100f;
        public float MinForce = 10f;

        public float GetVelocity()
        {
            return Random.Range(MinSpeed, MaxSpeed);
        }
        public float GetForce()
        {
            return Random.Range(MinForce, MaxForce);
        }
    }
}