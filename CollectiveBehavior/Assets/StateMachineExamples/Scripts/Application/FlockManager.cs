using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    [SerializeField] private Transform wallPrefab;
    [SerializeField] private float _rangeX = 10f;
    [SerializeField] private float _rangeY = 10f;
    [SerializeField] private float _rangeZ = 10f;

    [SerializeField] private int _flockCount = 100;

    [Header( " FlockParameter")]
    public float _maxSpeed = 20f;
    public float _minSpeed = 3f;
    public float _lookRange = 5f;
    public float _neighborDistance = 20f;
    public float _avoidDistance = 5f;
    public float _flockSpeed = 1f;
    public float _updateRate = 50f;

    bool _playing = true;

    private List<Vector3> _savedPositions;

    private Dictionary<Transform, Vector3> _positionDic;
    private Dictionary<Transform, Quaternion> _rotationDic;

    private List<Transform> _flock;

    public void SwitchPlay()
    {
        _playing = !_playing;
    }


    // Use this for initialization
    void Start()
    {

        CreateFlock();
    }

    void CreateFlock()
    {
        float sizeX = wallPrefab.localScale.x;
        float sizeZ = wallPrefab.localScale.z;

        _savedPositions = new List<Vector3>(_flockCount);
        _flock = new List<Transform>(_flockCount);
        _positionDic = new Dictionary<Transform, Vector3>();
        _rotationDic = new Dictionary<Transform, Quaternion>();

        for (int i = 0; i < _flockCount; i++)
        {

            var x = UnityEngine.Random.Range(-_rangeX, _rangeX);
            var y = UnityEngine.Random.Range(-_rangeY, _rangeY);
            var z = UnityEngine.Random.Range(-_rangeZ, _rangeZ);

            Vector3 p = new Vector3(x,y,z);

                _savedPositions.Add(p);

        }

        foreach (var pos in _savedPositions)
        {
            var w = Instantiate(wallPrefab, transform);
            w.localPosition = pos;
            _flock.Add(w);
        }

        foreach (var w in _flock)
        {
            if (w.GetComponent<FlockAgent>())
            {
                int t = UnityEngine.Random.Range(0, 3);
                w.GetComponent<FlockAgent>().SetType(t);
            }
            _positionDic.Add(w, w.localPosition);
            _rotationDic.Add(w, w.localRotation);
        }
    }

    public List<Transform> GetAllFlock()
    {
        return _flock;
    }

   public Vector3 GetGoalPos(FlockAgent agent)
    {
        Vector3 gol = Vector3.zero;

        foreach(var a in _flock)
        {
            var ai = a.GetComponent<FlockAgent>();

            if (ai.IsFlocking()||ai.GetType()==agent.GetType())
            {
                continue;
            }
            else
            {
                gol = a.position;

                break;
            }

        }
        if (gol == Vector3.zero)
        {
            return gol;
        }
        else
        {
            return gol - agent.transform.position;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_playing)
        {
            foreach (var a in _flock)
            {
                var ai = a.GetComponent<FlockAgent>();

                ai.UpdateFlock(this);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchPlay();
        }
     
    }
}
