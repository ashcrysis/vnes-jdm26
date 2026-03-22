using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [Serializable]
    public class WaypointData
    {
        public Transform target;
        public float delayAfterArriving;
    }

    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float speed;
    [SerializeField] private List<WaypointData> waypoints;
    
    private int _currentWaypoint = 0;

    private void Start()
    {
        transform.position = waypoints[_currentWaypoint].target.position;
        NextWaypoint();
    }

    private IEnumerator Move()
    {
        Vector2 target = waypoints[_currentWaypoint].target.position;
        
        while (Vector2.Distance(transform.position, target) > threshold)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        
        transform.position = target;
        
        yield return new WaitForSeconds(waypoints[_currentWaypoint].delayAfterArriving);
        
        NextWaypoint();
    }

    private void NextWaypoint()
    {
        _currentWaypoint++;
        if (_currentWaypoint >= waypoints.Count)
        {
            _currentWaypoint = 0;
        }
        
        StartCoroutine(Move());
    }
}
