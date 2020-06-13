﻿using System.Collections;
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

    private Vector3 dir;

    private Tween curMove;
    private bool needWaypoint = true;

    Seeker seeker;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        prevTargetPos = target.transform.position;

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
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            needWaypoint = true;
        }
    }

    void Update()
    {
        if ((target.transform.position - prevTargetPos).magnitude > 0.0001f)
        {
            UpdatePath();
        }

        prevTargetPos = target.transform.position;

        if (path == null)
            return;

        if (currentWaypoint + 1 == path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            needWaypoint = false;
            return;
        } else
        {
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
                ()=> transform.position,
                x=> transform.position = x,
                dest, duration)
                .SetEase(Ease.Linear)
                .OnKill(() => {
                    needWaypoint = true;
                });
            Debug.Log("DOne");
        }
    }
}