using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToClick : MonoBehaviour
{
    public Camera cam;

    public Vector3 offset;

    public bool active = false;

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            var midPos = cam.ScreenToWorldPoint(Input.mousePosition);
            midPos.x = Mathf.Round(midPos.x);
            midPos.y = Mathf.Round(midPos.y);
            midPos.z = transform.position.z;

            var offPos = new Vector3(0, 0, 0);
            offPos.x = Mathf.Sign(midPos.x) * offset.x;
            offPos.y = Mathf.Sign(midPos.y) * offset.y;

            transform.position = midPos - offPos;

        }
        
    }
}
