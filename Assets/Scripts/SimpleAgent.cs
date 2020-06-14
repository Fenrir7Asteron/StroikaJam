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
    
    public AudioSource workAudio;
    public AudioSource[] playerAudios;
    public AudioSource drinkAudio;
    public AudioSource screamAudio;
    public AudioSource openBottle;
    
    public Vector2 minPosForSleepPlace;
    public Vector2 maxPosForSleepPlace;
    
    protected bool _walking = false;
    protected float _workedTime = 0f;
    protected bool _atWork = false;
    protected bool _goToSleep = false;
    protected IEnumerator _currentWalk;
    protected String _currentWorkAnimation;
    protected bool _startSleepAnimatons;
    protected AudioSource _playerAudio;

    private void Start()
    {
        transform.position = getPath(Vector3.zero)[0]; // fix start position
        _playerAudio = playerAudios[0];
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
                workAudio.Stop();
                _atWork = false;
                _goToSleep = true;
                _currentWalk = Move(RandomSleepPosition());
                StartCoroutine(_currentWalk);
            }
        }

        if (!_startSleepAnimatons && !_walking && !_atWork)
        {
            _startSleepAnimatons = true;
            StartCoroutine(PlayDrinkAudio());
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
            if (!_playerAudio.isPlaying)
            {
                var index = Random.Range(0, playerAudios.Length);
                _playerAudio = playerAudios[index];
                _playerAudio.Play();
            }

            if (!screamAudio.isPlaying)
            {
                screamAudio.Play();
            }
            _workedTime = 0f;
            _startSleepAnimatons = false;
            drinkAudio.Stop();
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

    IEnumerator PlayDrinkAudio()
    {
        openBottle.Play();
        yield return new WaitForSeconds(1.5f);
        drinkAudio.Play();
    }

    protected virtual void StartWorkAnimation()
    {
    }
}