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
    public Animator animator;
    
    public Vector2 minPosForSleepPlace;
    public Vector2 maxPosForSleepPlace;
    
    private bool _walking = false;
    private float _workedTime = 0f;
    private bool _atWork = false;
    private bool _goToSleep = false;
    private IEnumerator _currentWalk;
    private String _currentWorkAnimation;

    private void Start()
    {
        transform.position = getPath(Vector3.zero)[0]; // fix start position
        Debug.Log($"{gameObject.name} is {GetInstanceID()}");
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
                animator.SetBool(_currentWorkAnimation, false);
                _atWork = false;
                _goToSleep = true;
                _currentWalk = Move(RandomSleepPosition());
                StartCoroutine(_currentWalk);
            }
        }

        if (!_walking && !_atWork)
        {
            animator.SetBool("sleep", true);
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
                StartWorkAnimation();
                _atWork = true;
            }
        }
        
        if (other.CompareTag("Player"))
        {
            _workedTime = 0f;
            animator.SetBool("sleep", false);
            if (!_atWork && (!_walking || _goToSleep))
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_atWork && other.CompareTag("Player"))
        {
            _workedTime = 0f;
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
        animator.SetBool("isWalking", true);
        
        List<Vector3> path = getPath(to);
        while (path == null) // TODO: костыль
        {
            path = getPath(RandomSleepPosition());
        }
        
        foreach (var nextPos in path)
        {
            while (transform.position != nextPos)
            {
                yield return new WaitForSeconds(delayBetweenStep);
                transform.position += GetDirection(transform.position, nextPos);
            }
        }
        
        animator.SetBool("isWalking", false);
        _walking = false;
    }

    private Vector3 GetDirection(Vector3 currentPosition, Vector3 nextPosition)
    {
        if (Equals(currentPosition.x, nextPosition.x))
        {
            if (currentPosition.y > nextPosition.y)
            {
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
                return Vector3.down;
            }
            
            transform.eulerAngles = new Vector3(0f, 0f, 90f);
            return Vector3.up;
        }

        if (currentPosition.x > nextPosition.x)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 180f);
            return Vector3.left;
        }
        
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        return Vector3.right;
    }
    
    private Vector3 RandomSleepPosition()
    {
        float x = Random.Range(minPosForSleepPlace.x, maxPosForSleepPlace.x);
        float y = Random.Range(minPosForSleepPlace.y, maxPosForSleepPlace.y);
        return new Vector3(x, y, 0);
    }

    private void StartWorkAnimation()
    {
        var type = Random.Range(1, 3);
        _currentWorkAnimation = type == 1 ? "working1" : "working2";
        animator.SetBool(_currentWorkAnimation, true);
    }
}