using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockAgent : MonoBehaviour
{
    float direction = 1f;
    float _speed = 10f;

    Vector3 toAvoid = Vector3.zero;

  

    public Color lookColor = Color.green;

   

    int linePointIndex = 0;

    bool flocking;
    public bool IsFlocking()
    {
        return flocking;
    }

    public float GetSpeed()
    {
        return _speed;
    }
    public float GetDirection()
    {
        return direction;
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

    public  void UpdateFlock(FlockManager manager)
    {
        int flag = 0;
      
        transform.localPosition += transform.forward * _speed * direction * Time.deltaTime;
        //check obstacle in forward 
        LookAt(transform.forward, transform.localScale.z + manager._lookRange);
        //check obstacle in back 
        LookAt(-transform.forward, transform.localScale.z + manager._lookRange);
        //check obstacle in right 
        LookAt(transform.right, transform.localScale.x + manager._lookRange);
        //check obstacle in left 
        LookAt(-transform.right, transform.localScale.x + manager._lookRange);

        if (Random.Range(0, 100) < manager._updateRate)
        {
            Flocking(manager);
            ChangeSpeed(manager);
        }
    }


    void LookAt(Vector3 dir,float range)
    {
       
       
        Debug.DrawRay(transform.position, dir*range,lookColor);

        RaycastHit hit;

        bool b = Physics.Raycast(transform.position,dir,out hit,range);

        if (b == true)
        {
            if (hit.transform.CompareTag("Objects")&&hit.transform!=transform)
            {
                var ai = hit.transform.GetComponent<FlockAgent>();

                if (ai && ai.GetType() == _type)
                {
                    return;
                }
                else
                {
                    toAvoid = hit.point;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="manager"></param>
    void Flocking(FlockManager manager)
    {
        Vector3 centre = Vector3.zero;
        Vector3 avoid = Vector3.zero;
        float fSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;
        foreach (var a in manager.GetAllFlock())
        {
            if (a == transform)
            {
                continue;
            }
            var agt = a.GetComponent<FlockAgent>();

            nDistance = Vector3.Distance(a.transform.position, transform.position);
            if (nDistance <= manager._neighborDistance)
            {
                if (agt.GetType() == _type)
                {
                    centre += a.transform.position;
                }
                else
                {
                    centre += (transform.position - a.position);
                }
                groupSize++;

                if (nDistance < manager._avoidDistance)
                {
                    avoid += (transform.position - a.position);
                }

                fSpeed += agt.GetSpeed();
            }
        }
        if (groupSize > 0)
        {
            centre = (centre) / groupSize+ (manager.GetGoalPos(this))+manager.GetTarget(this);
            _speed = fSpeed / groupSize;

            Vector3 dir = (centre + avoid) - transform.position+(-toAvoid);

            if (dir != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), manager._flockSpeed * Time.deltaTime);
                flocking = true;
                Debug.DrawRay(transform.position, dir.normalized * manager._lookRange * 2f, Color.red);
            }
        }
        else
        {
            flocking = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-toAvoid), manager._flockSpeed * Time.deltaTime);
        }
    }
    /// <summary>
    /// change Speed by probability
    /// </summary>
    void ChangeSpeed(FlockManager manager)
    {
        if (Random.Range(0, 100) < 20)
        {
            _speed = Random.Range(manager._minSpeed, manager._maxSpeed);
        }
    }

  public  void DrawLine(Vector3 point)
    {
        var line = GetComponent<LineRenderer>();
        line.positionCount = linePointIndex + 1;
        line.SetPosition(linePointIndex,point);

        linePointIndex++;
    }

  
}
