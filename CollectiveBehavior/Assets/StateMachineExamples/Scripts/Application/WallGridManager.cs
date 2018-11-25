using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class WallGridManager : MonoBehaviour
{
    [SerializeField]private Transform wallPrefab;
    [SerializeField] private float _gapX = 1f;
    [SerializeField] private float _gapZ = 3f;
    [SerializeField] private int _countX = 10;
    [SerializeField] private int _countZ = 10;
    [SerializeField] private Transform[] sourcesToGive;
    [SerializeField] Transform[] _enemysToGive;
    [SerializeField] private bool _center;

    private List<Vector3> _savedPositions;

    private Dictionary<Transform, Vector3> _positionDic;
    private Dictionary<Transform, Quaternion> _rotationDic;

    private List<Transform> _allWalls;




	// Use this for initialization
	void Start ()
	{
	    if (_center)
	    {
	        float sizex = wallPrefab.localScale.x;
	        float sizez = wallPrefab.localScale.z;

            transform.position=new Vector3(-0.5f*_countX*(sizex+_gapX),0,-0.5f*_countZ*(sizez+_gapZ));
	    }
		CreateGrid(_countX,_countZ);
	}

    void CreateGrid(int countX, int countZ)
    {
        float sizeX = wallPrefab.localScale.x;
        float sizeZ = wallPrefab.localScale.z;

        _savedPositions=new List<Vector3>(_countX*_countZ);
        _allWalls=new List<Transform>(_countX*_countZ);
        _positionDic=new Dictionary<Transform, Vector3>();
        _rotationDic=new Dictionary<Transform, Quaternion>();

        for (int z = 0; z < _countZ; z++)
        {
            for (int x = 0; x < _countX; x++)
            {
                Vector3 p=new Vector3(x*(sizeX+_gapX),wallPrefab.localPosition.y,z*(sizeZ+_gapZ));

                _savedPositions.Add(p);
            }
        }

        foreach (var pos in _savedPositions)
        {
            var w = Instantiate(wallPrefab, transform);
            w.localPosition = pos;
            _allWalls.Add(w);
        }

        foreach (var w in _allWalls)
        {
            if (w.GetComponent<StateMachineController>())
            {
                w.GetComponent<StateMachineController>().SetSources(sourcesToGive);
                w.GetComponent<StateMachineController>().SetEnemy(_enemysToGive);
            }

            if (w.GetComponent<SimpleAI>())
            {
                int t = UnityEngine.Random.Range(0, 3);
                w.GetComponent<SimpleAI>().SetType(t);
            }
            if (w.GetComponent<SimpleAIFlock>())
            {
                int t = UnityEngine.Random.Range(0, 3);
                w.GetComponent<SimpleAIFlock>().SetType(t);
            }


            _positionDic.Add(w,w.localPosition);
            _rotationDic.Add(w,w.localRotation);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
