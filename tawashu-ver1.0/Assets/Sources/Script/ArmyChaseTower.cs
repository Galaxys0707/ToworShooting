using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyChaseTower : MonoBehaviour
{

    public GameObject _Target;
    public float speed = 0.1f;
    private Vector3 vec;

    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_Target.transform.position - transform.position), 0.3f);
        transform.position += transform.forward * speed;
    }

}
