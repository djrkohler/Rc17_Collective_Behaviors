using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class FlockManager2d : MonoBehaviour
{
    [SerializeField] private Transform wallPrefab;
    [SerializeField] private float _rangeX = 10f;
    [SerializeField] private float _rangeY = 10f;
    [SerializeField] private float _rangeZ = 10f;

    [SerializeField] private int _flockCount = 100;
    [SerializeField] float _traceRate = 10f;
    [SerializeField] Transform _tracePrefab;

    public  Transform TraceHolder;

    [Header( " FlockParameter")]
    public float _maxSpeed = 20f;
    public float _minSpeed = 3f;
    public float _lookRange = 5f;
    public float _neighborDistance = 20f;
    public float _avoidDistance = 5f;
    public float _flockSpeed = 1f;
    public float _updateRate = 50f;

    float _timer = 0;
    bool _playing = true;
    bool _trace;

    private List<Vector3> _savedPositions;

    List<Transform> _traceList;

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
        _traceList = new List<Transform>();
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
            if (w.GetComponent<FlockAgent2d>())
            {
                int t = UnityEngine.Random.Range(0, 3);
                w.GetComponent<FlockAgent2d>().SetType(t);
            }
            _positionDic.Add(w, w.localPosition);
            _rotationDic.Add(w, w.localRotation);
        }
    }

    public List<Transform> GetAllFlock()
    {
        return _flock;
    }

   public Vector3 GetGoalPos(FlockAgent2d agent)
    {
        Vector3 gol = Vector3.zero;

        foreach(var a in _flock)
        {
            var ai = a.GetComponent<FlockAgent2d>();

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
                var ai = a.GetComponent<FlockAgent2d>();

                ai.UpdateFlock(this);
            }
            if (Time.time > _timer + _traceRate && _trace)
            {
                foreach (var a in _flock)
                {
                    
                    var t = Instantiate(_tracePrefab, TraceHolder);
                    t.transform.position = a.transform.position;
                    t.rotation = a.transform.rotation;
                    var s = t.localScale;
                    s.z = _traceRate * 15f;
                    t.localScale = s;
                   // t.GetComponent<MeshRenderer>().material.color = a.GetComponent<MeshRenderer>().material.color;
                    _traceList.Add(t);
                }
                _timer = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchPlay();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            _trace = !_trace;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            foreach(var a in _flock)
            {
                a.GetComponent<MeshRenderer>().enabled = !a.GetComponent<MeshRenderer>().enabled;
            }
        }


    }
}
