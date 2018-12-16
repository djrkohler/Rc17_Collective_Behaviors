using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    [SerializeField] Transform A;
    [SerializeField] Transform B;
    [SerializeField] Transform C;

    [SerializeField] Transform Target;

    Vector3 position;
    Quaternion rotation;
    Rigidbody body;
    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        position += transform.forward * Time.deltaTime * 3f;
        rotation= Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Target.position - transform.position), Time.deltaTime * 5f);

        var p = Vector3.Lerp(transform.position, position, Time.deltaTime * 3f);

        body.MovePosition(p);
        body.MoveRotation(rotation);

        //B.position = Vector3.Lerp(B.position, A.position, Time.deltaTime * 0.5f);
        //C.position = Vector3.Lerp(C.position, B.position, Time.deltaTime * 0.5f);
    }
}
