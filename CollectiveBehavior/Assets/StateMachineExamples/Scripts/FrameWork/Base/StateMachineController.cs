using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public  class StateMachineController : MonoBehaviour {

	// Use this for initialization
    public StateMachineState currentState;
    public Parameter parameter;
    public StateMachineState remainState;
    public bool isActive;
    public Transform []sources;
    public Transform[] enemys;



    [HideInInspector] private float stateTimer;


	// Update is called once per frame
	void FixedUpdate ()
	{
	    if (isActive)
	    {
            currentState.UpdateState(this);
	    }
	}

    public void TransitionToState(StateMachineState nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    void OnExitState()
    {
        stateTimer = 0;
    }

    public void OnActionDone()
    {
        stateTimer = 0;
    }

    public bool CoolDownTime(float duration)
    {
        stateTimer += Time.deltaTime;
        return (stateTimer >= duration);
    }

    public void SetSources(Transform[] src)
    {
        sources = src;
    }

    public void SetEnemy(Transform[] eny)
    {
        enemys = eny;
    }

    void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.stateColor;
            Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh,transform.position,transform.rotation,transform.localScale);
        }
    }


    public Transform GetClosetEnemy()
    {
        float minDis = float.MaxValue;
        Transform ce = null;
        foreach(var e in enemys)
        {
            float d = Vector3.Distance(e.position, transform.position);

            if (d < minDis)
            {
                minDis = d;
                ce = e;
            }
        }
        return ce;
    }
}
