using UnityEngine;
using UnityEditor;

namespace RC17.BlockChain
{
    [CreateAssetMenu(menuName = "BlockChain/VisualManager")]
    public class VisualModeManager : ScriptableObject
    {
        public VisualModeColors[] visualModes;

    }
}
