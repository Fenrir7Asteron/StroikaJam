using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMove : MonoBehaviour
{
    public Transform target;
    void Start()
    {
        var agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }
    
}
