using System.Collections.Generic;
using UnityEngine;

public class WorkZoneController : MonoBehaviour
{
    public Transform[] workZones;

    private Dictionary<int, int> _workZoneWorkers;

    private void Start()
    {
        _workZoneWorkers = new Dictionary<int, int>(workZones.Length);
    }

    public Vector3 GetWorkZonePosition()
    {
        int i = Random.Range(0, workZones.Length);
        return workZones[i].position;
    }

    public bool InWorkZone(int workZoneID, int workerID)
    {
        if (!_workZoneWorkers.ContainsKey(workZoneID))
        {
            _workZoneWorkers.Add(workZoneID, workerID);
            return true;
        }

        return false;
    }

    public void OutWorkZone(int workZoneID, int workerID)
    {
        if (_workZoneWorkers[workZoneID] == workerID)
        {
            _workZoneWorkers.Remove(workZoneID);
        }
    }
}
