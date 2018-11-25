using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    float direction = 1f;
    float directionUp = 1f;
    float rotate = 0f;
    float rotateRight = 0f;

    public float _rotateSpeed = 50f;
    public float _moveSpeed = 10f;

    public float _followSpeed = 5f;

    public float _lookRange = 5f;

    public Color lookColor = Color.green;

    Transform toFollow;

    public bool isFollowing(Transform t)
    {
        bool follow = false;
        if (toFollow == t)
        {
            follow = true;
        }

        return follow;
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

       
        
            transform.localPosition += transform.forward * _moveSpeed * direction * Time.deltaTime;
            transform.localPosition += transform.up * _moveSpeed * directionUp * Time.deltaTime;
            transform.Rotate(Vector3.up * _rotateSpeed * rotate * Time.deltaTime);
            transform.Rotate(Vector3.right * _rotateSpeed * rotateRight * Time.deltaTime);

            //check obstacle in forward 
            if (lookAt(transform.forward, transform.localScale.z + _lookRange))
            {
                rotateRight++;
                if (direction == 1f)
                {
                    direction = -1f;
                }
                flag++;
            }
            //check obstacle in back 
            if (lookAt(-transform.forward, transform.localScale.z + _lookRange))
            {
                rotateRight--;
                if (direction == -1f)
                {
                    direction = 1f;
                }
                flag++;
            }
            //check obstacle in up 
            if (lookAt(transform.up, transform.localScale.y + _lookRange))
            {
                if (directionUp == 1f)
                {
                    directionUp = -1f;
                }
                flag++;
            }

            //check obstacle in down 
            if (lookAt(-transform.up, transform.localScale.y + _lookRange))
            {
                if (directionUp == -1f)
                {
                    directionUp = 1f;
                }
                flag++;
            }

            //check obstacle in right 
            if (lookAt(transform.right, transform.localScale.x + _lookRange))
            {
                rotate++;
                flag++;
            }
            else
            {
                //RemainState;
            }
            //check obstacle in left 
            if (lookAt(-transform.right, transform.localScale.x + _lookRange))
            {
                rotate--;
                flag++;
            }

            if (flag == 0)
            {
                rotate = 0f;
                rotateRight = 0f;
            }
        
       


    }


    bool lookAt(Vector3 dir, float range)
    {

        bool sawSomething = false;

        Debug.DrawRay(transform.position, dir * range, lookColor);

        RaycastHit hit;

        bool b = Physics.Raycast(transform.position, dir, out hit, range);

        if (b == true)
        {
            if (hit.transform.CompareTag("Objects") && hit.transform != transform)
            {
                sawSomething = true;
            }
            if (hit.transform.GetComponent<SimpleAI>())
            {
                var ai = hit.transform.GetComponent<SimpleAI>();
                if (ai.GetType() == _type)
                {
                    toFollow = hit.transform;
                }
            }

        }


        return sawSomething;
    }
}


public enum BlockTypes
{
    Type_A, Type_B, Type_C
}