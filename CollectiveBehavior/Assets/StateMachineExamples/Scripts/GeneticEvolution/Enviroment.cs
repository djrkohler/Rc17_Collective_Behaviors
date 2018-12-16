using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SpatialSlur.Core;

namespace GA
{
    public class Enviroment : MonoBehaviour
    {

        [SerializeField] private AgentBase _prefabA;
        [SerializeField] private AgentBase _prefabB;
        [SerializeField] private AgentBase _prefabC;
        [SerializeField] private AgentBase _prefabD;
        [SerializeField] Food foodPrefab;

        [SerializeField] private float _rangeX = 10f;
        [SerializeField] private float _rangeY = 10f;
        [SerializeField] private float _rangeZ = 10f;

        [SerializeField] private int _flockCount = 100;
        [SerializeField] float _rebornRate = 60f;
        [SerializeField] Transform _tracePrefab;
        [SerializeField] Transform _goal;
        [SerializeField] float _targetSwitchRate = 3f;
        [SerializeField] int foodCount = 10;
        [SerializeField] int _maxCount = 100;

        float _targetTimer;
        List<Vector3> _goalPosition;

        int _currentGoal = 0;

        public Transform TraceHolder;

        [Header(" FlockParameter")]
        public float _maxSpeed = 20f;
        public float _minSpeed = 3f;
        public float _lookRange = 5f;
        public float _neighborDistance = 20f;
        public float _avoidDistance = 5f;
        public float _flockSpeed = 1f;
        public float _updateRate = 50f;
        public float _foodRange = 15f;
        public float _breedRate = 90f;

        float _timer = 0;
        bool _playing = true;
        bool _trace;

        private List<Vector3> _savedPositions;

        List<Transform> _traceList;
        List<Food> Foods;

        private Dictionary<AgentBase, Vector3> _positionDic;
        private Dictionary<AgentBase, Quaternion> _rotationDic;

        private List<AgentBase> _flock;


        public void SwitchPlay()
        {
            _playing = !_playing;
        }


        // Use this for initialization
        void Start()
        {
            CreateFlock();
            _traceList = new List<Transform>();
            _goalPosition = new List<Vector3>();

            ProduceFood();
        }

        void ProduceFood()
        {
            Foods = new List<Food>();
            for(int i = 0; i < foodCount; i++)
            {
                var f = Instantiate(foodPrefab);
                Foods.Add(f);
                f.gameObject.SetActive(false);

            }
        }

        void Feed()
        {
            int foodCount = 0;
            foreach(var f in Foods)
            {
                if (!f.isValid())
                {
                    f.gameObject.SetActive(false);
                }
                if (f.gameObject.activeSelf)
                    foodCount++;
            }

            if (foodCount < 2&&Random.Range(0,100f)>80f)
            {
               foreach(var f in Foods)
                {
                    if (f.gameObject.activeSelf)
                        continue;

                    f.gameObject.SetActive(true);
                    f.transform.position = Random.insideUnitSphere * _rangeX;
                    f.ResetFood();
                    break;
                }

            }
        }


        void AddGoalPosition()
        {
            var x = UnityEngine.Random.Range(-_rangeX, _rangeX);
            var y = UnityEngine.Random.Range(-_rangeY, _rangeY);
            var z = UnityEngine.Random.Range(-_rangeZ, _rangeZ);

            _goalPosition.Add(new Vector3(x, y, z));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public Vector3 GetFoodTarget(AgentBase agent)
        {
            var vf = GetValidFood().ToList();

            if (vf.Count> 0)
            {
                var food = GetValidFood().SelectMin(f =>
                {
                    float dis = float.MaxValue;
                    var d = Vector3.Distance(agent.BodyParts[0].transform.position, f.transform.position);

                    if (d < _foodRange)
                    {
                        dis = d;
                    }

                    return dis;
                });
                return food.transform.position-agent.BodyParts[0].transform.position;
            }
            else
            {
                return Vector3.zero;
            }
        }

        void Breed(RaceType type)
        {
            var agents = GetTypeAgent(type).ToList();

            if (agents.Count == 0)
                return;

            var d0 = agents.SelectMax(a =>
            {
                return a.properties.score;
            });
            agents.Remove(d0);
            var d1 = agents.SelectMax(a =>
            {
                return a.properties.score;
            });

            foreach(var a in _flock)
            {
                if (a == d0 || a == d1||a.dna.GetType()!=type)
                {
                    return;
                }

                a.Born(d0, d1);
            }
        }

        AgentBase GetPrefab(RaceType type)
        {
            switch (type)
            {
                case RaceType.a:
                    return _prefabA;
                case RaceType.b:
                    return _prefabB;
                case RaceType.c:
                    return _prefabC;
                case RaceType.d:
                    return _prefabD;
                default:
                    return _prefabA;
            }
        }

        IEnumerable<AgentBase> GetTypeAgent(RaceType type)
        {
            foreach(var a in _flock)
            {
                if (a.dna.GetType() == type)
                    yield return a;
            }
        }

        IEnumerable<AgentBase> GetTypeDead(RaceType type)
        {
            foreach (var a in _flock)
            {
                if (a.dna.GetType() == type && a.IsDead())
                    yield return a;
                break;
            }
        }

        IEnumerable<Food> GetValidFood()
        {
            foreach (var f in Foods)
            {
                if (f.gameObject.activeSelf)
                {
                    yield return f;
                }
            }
        }

        void CreateFlock()
        {
            _savedPositions = new List<Vector3>(_flockCount);
            _flock = new List<AgentBase>(_flockCount);

            _positionDic = new Dictionary<AgentBase, Vector3>();
            _rotationDic = new Dictionary<AgentBase, Quaternion>();

            for (int i = 0; i < _flockCount; i++)
            {
                var x = UnityEngine.Random.Range(-_rangeX, _rangeX);
                var y = UnityEngine.Random.Range(-_rangeY, _rangeY);
                var z = UnityEngine.Random.Range(-_rangeZ, _rangeZ);
                Vector3 p = new Vector3(x, y, z);

                _savedPositions.Add(p);
            }

            foreach (var pos in _savedPositions)
            {
                int t = UnityEngine.Random.Range(0, 4);

                AgentBase prefab;

                switch (t)
                {
                    case 0:
                        prefab = _prefabA;
                        break;
                    case 1:
                        prefab = _prefabB;
                        break;
                    case 2:
                        prefab = _prefabC;
                        break;
                    case 3:
                        prefab = _prefabD;
                        break;
                    default:
                        prefab = _prefabA;
                        break;

                }

                var a = Instantiate(prefab, transform);
                a.transform.localPosition = pos;
                a.Born();

                _flock.Add(a);
            }

            foreach (var a in _flock)
            {
                _positionDic.Add(a, a.transform.localPosition);
                _rotationDic.Add(a, a.transform.localRotation);
            }
        }
        public List<AgentBase> GetAllFlock()
        {
            return _flock;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public Vector3 GetGoalPos(AgentBase agent)
        {
            Vector3 gol = Vector3.zero;

            foreach (var a in _flock)
            {
                var ai = a.GetComponent<AgentBase>();

                if (ai.IsGrouping() || ai.dna.GetType() == agent.dna.GetType())
                {
                    continue;
                }
                else
                {
                    gol = a.BodyParts[0].transform.position-agent.BodyParts[0].transform.position;
                    break;
                }
            }
            return gol;

        }

        // Update is called once per frame
        void Update()
        {
            if (_playing)
            {
                foreach (var a in _flock)
                {
                    if (!a.IsDead())
                    {
                        a.UpdateAgent(this);
                    }
                }
                if (Time.time > _timer + _rebornRate)
                {
                    Breed(RaceType.a);
                    Breed(RaceType.b);
                    Breed(RaceType.c);
                    Breed(RaceType.d);

                    _timer = Time.time;
                }

                if (Time.time > _targetTimer + _targetSwitchRate)
                {
                    _targetTimer = Time.time;
                }

                if (_goalPosition.Count > 0)
                    _goal.transform.position = Vector3.Lerp(_goal.transform.position, _goalPosition[_currentGoal], Time.deltaTime * 3f);
            }

            Feed();

            if (Random.Range(0, 100f) < _breedRate)
            {
                Breed(RaceType.a);
            }
            if (Random.Range(0, 100f) < _breedRate)
            {
                Breed(RaceType.b);
            }
            if (Random.Range(0, 100f) < _breedRate)
            {
                Breed(RaceType.c);
            }
            if (Random.Range(0, 100f) < _breedRate)
            {
                Breed(RaceType.d);
            }


            if (Input.GetKeyDown(KeyCode.Space))
            {
                SwitchPlay();

                foreach(var a in _flock)
                {
                    Debug.Log(a.properties.health);
                }
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                _trace = !_trace;
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                foreach (var a in _flock)
                {
                    a.GetComponent<MeshRenderer>().enabled = !a.GetComponent<MeshRenderer>().enabled;
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddGoalPosition();
            }
        }
    }
}