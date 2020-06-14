using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public float speed = 10f;
    public Animator animator;

    void Update()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        animator.SetBool("walk", move != Vector3.zero);
        RotatePlayer(move);
        transform.position += move * (speed * Time.deltaTime);
    }

    private void RotatePlayer(Vector3 nextPosition)
    {
        if (nextPosition != Vector3.zero)
        {
            var rotateVector = Vector3.zero;
            if (nextPosition.y < 0)
            {
                rotateVector += new Vector3(0f, 0f, -90f);
            }
            else if(nextPosition.y > 0)
            {
                rotateVector += new Vector3(0f, 0f, 90f);
            }

            if (nextPosition.x < 0)
            {
                rotateVector += new Vector3(0f, 0f, 180f);
            }

            Debug.Log(rotateVector);
            transform.eulerAngles = rotateVector;
        }
    }
}