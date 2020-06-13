using System;
using System.Collections;
using System.Collections.Generic;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SimpleAgent : MonoBehaviour
{
    public Tilemap wallTilemap;
    public WorkZoneController workZoneController;
    public float delayBetweenStep = 0.3f;
    public float timeBeforeTired = 5f;
    
    public Vector2 minPosForSleepPlace;
    public Vector2 maxPosForSleepPlace;
    
    private bool _walking = false;
    private float _workedTime = 0f;
    private bool _atWork = false;
    private bool _goToSleep = false;
    private IEnumerator _currentWalk;

    private void Start()
    {
        transform.position = getPath(Vector3.zero)[0]; // fix start position
    }

    private void Update()
    {
        if (!_walking && !_atWork && _workedTime < timeBeforeTired)
        {
            _currentWalk = Move(workZoneController.GetWorkZonePosition());
            StartCoroutine(_currentWalk);
        }

        if (!_walking && _atWork)
        {
            _workedTime += Time.deltaTime;
            if (_workedTime > timeBeforeTired)
            {
                _atWork = false;
                _goToSleep = true;
                _currentWalk = Move(RandomSleepPosition());
                StartCoroutine(_currentWalk);
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WorkZone"))
        {
            bool isFree = workZoneController.InWorkZone(other.transform.GetInstanceID(), GetInstanceID());
            if (!isFree)
            {
                StopCoroutine(_currentWalk);
                _currentWalk = Move(workZoneController.GetWorkZonePosition());
                StartCoroutine(_currentWalk);
            }
            else
            {
                _atWork = true;
            }
        }
        
        if (other.CompareTag("Player"))
        {
            _workedTime = 0f;
            if (!_walking || _goToSleep)
            {
                if (_goToSleep)
                {
                    StopCoroutine(_currentWalk);
                    _goToSleep = false;
                }

                _currentWalk = Move(workZoneController.GetWorkZonePosition());
                StartCoroutine(_currentWalk);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("WorkZone"))
        {
            workZoneController.OutWorkZone(other.transform.GetInstanceID(), GetInstanceID());
        }
    }

    private List<Vector3> getPath(Vector3 to)
    {
        return AStar.FindPathClosest(wallTilemap, transform.position, to);
    }

    private IEnumerator Move(Vector3 to)
    {
        _walking = true;
        List<Vector3> path = getPath(to);
        foreach (var nextPos in path)
        {
            while (transform.position != nextPos)
            {
                yield return new WaitForSeconds(delayBetweenStep);
                transform.position += GetDirection(transform.position, nextPos);
            }
        }

        _walking = false;
    }

    private Vector3 GetDirection(Vector3 currentPosition, Vector3 nextPosition)
    {
        if (Equals(currentPosition.x, nextPosition.x))
        {
            if (currentPosition.y > nextPosition.y)
            {
                return Vector3.down;
            }

            return Vector3.up;
        }

        if (currentPosition.x > nextPosition.x)
        {
            return Vector3.left;
        }

        return Vector3.right;
    }
    
    private Vector3 RandomSleepPosition()
    {
        float x = Random.Range(minPosForSleepPlace.x, maxPosForSleepPlace.x);
        float y = Random.Range(minPosForSleepPlace.y, maxPosForSleepPlace.y);
        return new Vector3(x, y, 0);
    }
}