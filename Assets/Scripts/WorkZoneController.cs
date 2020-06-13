using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkZoneController : MonoBehaviour
{
    public Transform[] workZones;

    private Dictionary<int, int> _workZoneWorkers;

    private void Start()
    {
        _workZoneWorkers = new Dictionary<int, int>(workZones.Length); ;
    }

    public Vector3 GetWorkZonePosition()
    {
        foreach (var workZone in workZones)
        {
            if (!_workZoneWorkers.ContainsValue(workZone.GetInstanceID()))
            {
                return workZone.position;
            }
        }
        
        Debug.LogError("Somehow all work zone is busy");
        return Vector3.zero;
    }

    public bool InWorkZone(int workZoneID, int workerID)
    {
        if (!_workZoneWorkers.ContainsValue(workZoneID))
        {
            _workZoneWorkers.Add(workerID, workZoneID);
            return true;
        }

        return false;
    }

    public void OutWorkZone(int workZoneID, int workerID)
    {
        if(!_workZoneWorkers.ContainsKey(workerID))
            return;
        if (_workZoneWorkers[workerID] == workZoneID)
        {
            _workZoneWorkers.Remove(workerID);
        }
    }
}
