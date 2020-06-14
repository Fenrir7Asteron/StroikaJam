using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    public float speed = 10f;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            new Vector3(target.position.x, target.position.y, transform.position.z), Time.deltaTime * speed);
    }
}
