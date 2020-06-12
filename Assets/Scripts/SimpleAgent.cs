using UnityEngine;
using UnityEngine.AI;

public class SimpleAgent : MonoBehaviour
{
    public Transform workTarget;
    public float timeBeforeTired = 5f;
    public Vector2 minPosForSleepPlace;
    public Vector2 maxPosForSleepPlace;

    private NavMeshAgent _agent;

    private float _workedTime = 0f;
    private bool _freezeWorkedTime = false;
    private bool _atWork = true;
    private Vector3 _sleepPlace;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _sleepPlace = newRandomPos();
    }

    private void Update()
    {
        //Debug.Log(_workedTime);
        if (!_freezeWorkedTime)
        {
            _workedTime += Time.deltaTime;
            if (_workedTime >= timeBeforeTired)
            {
                _freezeWorkedTime = true;
                _agent.SetDestination(_sleepPlace);
                _sleepPlace = newRandomPos();
                _atWork = false;
            }
        }

        CheckIfWorkerStopped();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _atWork = true;
            _workedTime = 0f;
            _agent.SetDestination(workTarget.position);
            
            if (transform.rotation.z > Mathf.Epsilon) 
            {
                transform.Rotate(0f, 0f, -90f, Space.World); // sleep animation off
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_freezeWorkedTime && other.CompareTag("Player"))
        {
            _workedTime = 0f;
            _freezeWorkedTime = true;
        }
    }

    private Vector3 newRandomPos()
    {
        float x = Random.Range(minPosForSleepPlace.x, maxPosForSleepPlace.x);
        float y = Random.Range(minPosForSleepPlace.y, maxPosForSleepPlace.y);
        return new Vector3(x, y, 0);
    }

    private void CheckIfWorkerStopped()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (Mathf.Abs (_agent.velocity.sqrMagnitude) < Mathf.Epsilon)
            {
                if (_atWork) 
                {
                    _freezeWorkedTime = false;
                }
                else 
                {
                    if (transform.rotation.z < Mathf.Epsilon) 
                    {
                        transform.Rotate(0f, 0f, 90f, Space.World); // enable sleep animation
                    }
                }
            }
        }
        else
        {
            if (transform.rotation.z > Mathf.Epsilon) // somehow sleep animations start before move
            {
                transform.Rotate(0f, 0f, -90f, Space.World); // sleep animation off
            }
        }
    }

}