using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RC17.BlockChain
{
    public class Whole
    {
        public List<int> elements;

        public Whole()
        {
            elements = new List<int>();
        }

        public void AddElement(int e)
        {
            if (!elements.Contains(e))
            {
                elements.Add(e);

               // Debug.Log(e);
            }
        }
        public void AddElement(int[] e)
        {
            foreach(var i in e)
            {
                if (!elements.Contains(i))
                {
                    elements.Add(i);
                    //Debug.Log(i);
                }
            }
         
        }
    }
}