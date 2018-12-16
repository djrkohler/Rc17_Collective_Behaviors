using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpatialSlur.Core;
using UnityEngine.UI;

namespace RC17.BlockChain
{
    public class BlockManager : MonoBehaviour
    {
        [SerializeField] private float _gapX = 1f;
        [SerializeField] private float _gapZ = 3f;
        [SerializeField] private int _countX = 10;
        [SerializeField] private int _countZ = 10;
        [SerializeField] Transform _holder;
        [SerializeField] VisualModeManager visManager;
        [SerializeField] Text visualText;
        public Block blockPrefab;

        int visualMode = 0;


        List<Block> Blocks;
        List<Vector3> Positions;
        List<Whole> wholes;
        // Use this for initialization
        void Start()
        {
            CreateGrid(_countX, _countZ);
            wholes = new List<Whole>();
            visualText.text = visManager.visualModes[visualMode].modeName;
        }

        // Update is called once per frame
        void Update()
        {
            Visualize();
            KeyPress();
        }

        void Visualize()
        {
            foreach (var b in Blocks)
            {
                float t = 0f;
                if (visualMode == 0)
                {
                     t = (float)b.ChainCount() / (float)MaxChainCount();
                   
                }
                if (visualMode == 1)
                {
                     t = (float)b.chain / (float)MaxChainOrder();
                }
                if (visualMode == 2)
                {
                    t = b.CurrentForce() / MaxForce();
                }
                if (visualMode == 3)
                {
                    t = b.CurrentTorque() / MaxTorque();
                }

                b.UpdateColor(t, visManager.visualModes[visualMode].a, visManager.visualModes[visualMode].b);
            }
        }
        void KeyPress()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                if (visualMode < visManager.visualModes.Length-1)
                    visualMode++;
                else
                    visualMode = 0;

                visualText.text = visManager.visualModes[visualMode].modeName;
                print(visManager.visualModes[visualMode].modeName);
            }
        }

        int MaxChainCount()
        {
            var max = Blocks.SelectMax(b =>
            {
                return b.ChainCount();
            });

            int count = 1;

            if (max.ChainCount() != 0)
            {
                count = max.ChainCount();
            }

            return count;
        }
        int MaxChainOrder()
        {
            var max = Blocks.SelectMax(b =>
            {
                return b.chain;
            });

            int count = 1;

            if (max.chain != 0)
            {
                count = max.chain;
            }

            return count;
        }

        float MaxForce()
        {
            var max = Blocks.SelectMax(b =>
            {
                return b.CurrentForce();
            });

            float force = 1f;

            if (max.CurrentForce() != 0f)
            {
                force = max.CurrentForce();
            }

            return force;
        }
        float MaxTorque()
        {
            var max = Blocks.SelectMax(b =>
            {
                return b.CurrentTorque();
            });

            float torque = 1f;

            if (max.CurrentTorque() != 0f)
            {
                torque = max.CurrentTorque();
            }

            return torque;
        }


        void CreateGrid(int countX, int countZ)
        {
          

          
            Blocks = new List<Block>(_countX * _countZ);
            Positions = new List<Vector3>(_countX * _countZ);
         

            for (int z = 0; z < _countZ; z++)
            {
                for (int x = 0; x < _countX; x++)
                {
                    Vector3 p = new Vector3(x *  _gapX, blockPrefab.transform.localPosition.y, z *  _gapZ);

                    Positions.Add(p);
                }
            }

            foreach (var pos in Positions)
            {
                var w = Instantiate(blockPrefab, _holder);
                w.index = Blocks.Count;
                w.Initialize();
                w.transform.localPosition = pos;
                Blocks.Add(w);
            }
        }
    }
}