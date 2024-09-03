using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPointsManager : MonoBehaviour
{
    public List<GameObject> Points {  get => _points; }
    public List<WayPoint> WayPoints {  get => _wayPoints; }

    private List<GameObject> _points = new List<GameObject>();
    private List<WayPoint> _wayPoints = new List<WayPoint>();


    void Start()
    {
        foreach (Transform child in transform)
            _points.Add(child.gameObject);
        _points.OrderBy(obj => obj.name);

        CreateWayPoints();
    }

    private void CreateWayPoints()
    {
        int prev, next;

        for (int i = 0; i < _points.Count; i++)
        {
            AssignIndexes(i, out prev, out next);

            _wayPoints.Add(new WayPoint(_points[i].transform.position, _points[prev].transform.position, _points[next].transform.position));
            //Debug.Log(i + ": angle: " + _wayPoints[i].Angle + "\n");
        }
    }
    private void AssignIndexes(int index, out int prev, out int next)
    {
        prev = index - 1;
        next = index + 1;
        
        if (index == 0)
            prev = _points.Count - 1;
        else if (index == _points.Count - 1)
            next = 0;
    }
    public float GetAngle(int index)
        => _wayPoints[index].Angle;
}
