using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAIFlock : MonoBehaviour
{
    float direction = 1f;
    float directionUp = 1f;
    float rotate = 0f;
    float rotateRight = 0f;
    float rotateFwd = 0f;

    public float _rotateSpeed = 50f;
    public float _moveSpeed = 10f;
    public float _followSpeed = 5f;
    public float _lookRange = 5f;
    public float followRate = 0.6f;
    public float _followTime = 15f;
    public float _neighborDistance = 20f;
    public float _avoidDistance = 5f;
    public float _flockSpeed = 1f;

    public Color lookColor = Color.green;

    bool isLeader;

    public void SetLeader(bool leader)
    {
        isLeader = leader;
    }

    List<Transform> _sameType;

    bool flocking;
    SimpleAIFlock hitAI;

    public float GetSpeed()
    {
        return _moveSpeed;
    }
    public float GetDirection()
    {
        return direction;
    }

    private void Start()
    {
        _sameType = new List<Transform>();
    }


    BlockTypes _type;

    public void SetType(int t)
    {
        switch (t)
        {
            case 0:
                _type = BlockTypes.Type_A;

                GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case 1:
                _type = BlockTypes.Type_B;
                GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
            case 2:
                _type = BlockTypes.Type_C;
                GetComponent<MeshRenderer>().material.color = Color.green;
                break;
        }
    }

    public BlockTypes GetType()
    {
        return _type;
    }

    private void Update()
    {
        int flag = 0;
        Flocking();
        transform.localPosition += transform.forward * _moveSpeed * direction * Time.deltaTime;
        bool hitElse = false;

        //check obstacle in forward 
        bool hitFwd;
        if (lookAt(transform.forward, transform.localScale.z + _lookRange,out hitFwd))
        {

            if (hitFwd) hitElse = true;
            if (!flocking||hitFwd)
            {
                rotateRight++;
                if (direction == 1f)
                {
                    direction = -1f;
                }
            }

            flag++;
        }
        //check obstacle in back 
        bool hitBak;
        if (lookAt(-transform.forward, transform.localScale.z + _lookRange, out hitBak))
        {
            if (hitBak) hitElse = true;
            if (!flocking || hitBak)
            {
                rotateRight--;
                if (direction == -1f)
                {
                    direction = 1f;
                }
            }
            flag++;
        }

        //check obstacle in right 
        bool hitRight;
        if (lookAt(transform.right, transform.localScale.x + _lookRange, out hitRight))
        {
            if (hitRight) hitElse = true;
            if (!flocking || hitRight)
            {
                rotate++;
            }
            flag++;
        }

        //check obstacle in left 
        bool hitLeft;
        if (lookAt(-transform.right, transform.localScale.x + _lookRange, out hitLeft))
        {
            if (hitLeft) hitElse = true;
            if (!flocking || hitLeft)
            {
                rotate--;
            }
            flag++;
        }

        if (flag == 0)
        {
            rotate = 0f;
            rotateRight = 0f;
        }

        if (!flocking||hitElse)
        {
            transform.Rotate(Vector3.up * _rotateSpeed * rotate * Time.deltaTime);
            transform.Rotate(Vector3.right * _rotateSpeed * rotateRight * Time.deltaTime);
        }
    }


    bool lookAt(Vector3 dir,float range,out bool hitSomethingElse)
    {
        hitSomethingElse = false;
        bool sawSomething = false;

        Debug.DrawRay(transform.position, dir*range,lookColor);

        RaycastHit hit;

        bool b = Physics.Raycast(transform.position,dir,out hit,range);

        if (b == true)
        {
            if (hit.transform.CompareTag("Objects")&&hit.transform!=transform)
            {
                sawSomething = true;
            }
            if (hit.transform.GetComponent<SimpleAIFlock>())
            {
                var ai = hit.transform.GetComponent<SimpleAIFlock>();
                //if (ai.GetType() == _type&&!ai.isFollowing(transform)&&!ai.following&&Time.time>_followTime)
                //{
                //    toFollow = hit.transform;
                //}
                if (ai.GetType() == _type)
                {
                    if (!_sameType.Contains(hit.transform))
                        _sameType.Add(hit.transform);
                }
                else
                {
                    hitSomethingElse = true;
                }
            }
            else
            {
                hitSomethingElse = true;
            }
        }
        return sawSomething;
    }
    void Flocking()
    {
        //flocking = false;
        if (_sameType.Count > 1)
        {
            Vector3 centre = Vector3.zero;
            Vector3 avoid = Vector3.zero;
            float fSpeed = 0.01f;
            float fDirection = 0f;
            float nDistance;
            int groupSize = 0;
            foreach(var a in _sameType)
            {
                nDistance = Vector3.Distance(a.transform.position, transform.position);

                if (nDistance <= _neighborDistance)
                {
                    centre += a.transform.position;
                    groupSize++;

                    if (nDistance < _avoidDistance)
                    {
                        avoid = avoid + (transform.position - a.transform.position);
                    }

                    fSpeed += a.GetComponent<SimpleAIFlock>().GetSpeed();
                    fDirection += a.GetComponent<SimpleAIFlock>().GetDirection();
                }
            }
            if (groupSize > 0)
            {
                centre = centre / groupSize;
                _moveSpeed = fSpeed / groupSize;

              

                Vector3 dir = (centre + avoid) - transform.position;

                if (dir != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), _flockSpeed * Time.deltaTime);

                    if (fDirection != 0)
                        direction = fDirection / Mathf.Abs(fDirection);

                    flocking = true;
                    Debug.DrawRay(transform.position, dir.normalized * _lookRange*2f,Color.red);
                }
            }
            else
            {
                flocking = false;
            }

        }
    }
}
