using System.Collections.Generic;
using UnityEngine;

public class WorkZoneController : MonoBehaviour
{
    public Transform[] workZones;

    private Dictionary<int, int> _workZoneWorkers;
    private Dictionary<int, Transform> _idToTransform;
    private List<int> _freeZone;

    private void Start()
    {
        _workZoneWorkers = new Dictionary<int, int>(workZones.Length);
        _idToTransform = new Dictionary<int, Transform>(workZones.Length);
        _freeZone = new List<int>(workZones.Length);
        foreach (var workZone in workZones)
        {
            _idToTransform.Add(workZone.GetInstanceID(), workZone);
        }
        _freeZone.AddRange(_idToTransform.Keys);
    }

    public Vector3 GetWorkZonePosition()
    {
        int i = Random.Range(0, _freeZone.Count);
        if (_freeZone.Count <= 0) // TODO: костыль
        {
            return workZones[0].position;
        }
        return _idToTransform[_freeZone[i]].position;
    }

    public bool InWorkZone(int workZoneID, int workerID)
    {
        if (_freeZone.Contains(workZoneID))
        {
            _workZoneWorkers.Add(workerID, workZoneID);
            _freeZone.Remove(workZoneID);
            return true;
        }

        return false;
    }

    public void OutWorkZone(int workZoneID, int workerID)
    {
        if (!_workZoneWorkers.ContainsKey(workerID))
        {
            return;
        }
        if (_workZoneWorkers[workerID] == workZoneID)
        {
            _workZoneWorkers.Remove(workerID);
            _freeZone.Add(workZoneID);
        }
    }
}
