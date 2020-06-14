using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public Transform target;
    private Vector3 prevTargetPos;
    public float speed;
    public float nextWaypointDistance = Mathf.Epsilon;
    public Animator animator;

    private Vector3 dir;

    private Tween curMove;
    private bool needWaypoint = true;

    private Vector3 myPrevPos;

    Seeker seeker;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        prevTargetPos = target.transform.position;
        myPrevPos = transform.position;

        //InvokeRepeating("UpdatePath", 0.0f, 0.5f);
        //UpdatePath();
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (p.error)
            return;

        var endPos = (Vector2)p.vectorPath[p.vectorPath.Count - 1];
        var dist = Vector2.Distance(endPos, (Vector2)target.position);

        if (dist < 0.001f)
        {
            path = p;
            currentWaypoint = 0;
            needWaypoint = true;
        }
    }

    void Update()
    {
        var nextDir = (Vector2)(transform.position - myPrevPos);
        Quaternion rot = transform.localRotation;
        if (nextDir.magnitude >= 0.00001f)
        {
            nextDir.Normalize();
            float[] dirs = new float[4];
            dirs[0] = (nextDir - Vector2.right).magnitude;
            dirs[1] = (nextDir - Vector2.up).magnitude;
            dirs[2] = (nextDir - Vector2.left).magnitude;
            dirs[3] = (nextDir - Vector2.down).magnitude;

            int minIndex = 0;
            for (int i = 1; i < 4; i++)
                if (dirs[minIndex] > dirs[i])
                    minIndex = i;

            if (minIndex == 0)
                rot = Quaternion.Euler(0, 0, 0);
            else if (minIndex == 1)
                rot = Quaternion.Euler(0, 0, 90);
            else if (minIndex == 2)
                rot = Quaternion.Euler(0, 0, 180);
            else if (minIndex == 3)
                rot = Quaternion.Euler(0, 0, 270);
        }

        myPrevPos = transform.position;
        transform.localRotation = rot;

        if ((target.transform.position - prevTargetPos).magnitude > 0.0001f)
        {
            UpdatePath();
        }

        prevTargetPos = target.transform.position;

        if (path == null)
        {
            animator.SetBool("walk", false);
            return;
        }

        if (currentWaypoint + 1 == path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            needWaypoint = false;
            animator.SetBool("walk", false);
            return;
        }
        else
        {
            animator.SetBool("walk", true);
            reachedEndOfPath = false;
        }

        if (!reachedEndOfPath && needWaypoint)
        {
            currentWaypoint++;
            needWaypoint = false;
            Debug.Log("CWP: " + currentWaypoint);
            var dest = path.vectorPath[currentWaypoint];
            dest.z = transform.position.z;
            var duration = (dest - transform.position).magnitude / speed;
            curMove = DOTween.To(
                    () => transform.position,
                    x => transform.position = x,
                    dest, duration)
                .SetEase(Ease.Linear)
                .OnKill(() => { needWaypoint = true; });
            Debug.Log("DOne");
        }
    }
}