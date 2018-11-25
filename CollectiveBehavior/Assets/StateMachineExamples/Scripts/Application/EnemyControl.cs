using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {

    [SerializeField] KeyCode _forward;
    [SerializeField] KeyCode _backward;
    [SerializeField] KeyCode _left;
    [SerializeField] KeyCode _right;
    [SerializeField] float _stiffness = 0.5f;

    Vector3 position;

	// Use this for initialization
	void Start ()
    {
        position = transform.localPosition;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveToward(_left);
        MoveToward(_right);
        MoveToward(_forward);
        MoveToward(_backward);


        transform.localPosition = Vector3.Lerp(transform.localPosition, position, Time.deltaTime * _stiffness);

	}

    void MoveToward(KeyCode key)
    {
        position += KeyDirection(key);
    }

    Vector3 KeyDirection(KeyCode key)
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(_forward))
            dir= Vector3.forward;
        if (Input.GetKey(_backward))
            dir= Vector3.back;
        if (Input.GetKey(_left))
            dir= Vector3.left;
        if (Input.GetKey(_right))
            dir = Vector3.right;

        return dir;

    }
}
