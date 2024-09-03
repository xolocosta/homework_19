using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _boostSpeed;
    [SerializeField] private WayPointsManager _pointsManager;

    [SerializeField] float[] _distance;
    [SerializeField] private List<GameObject> _points;
    private List<GameObject> _predictionPoints;
    
    private Vector3 _genesisPoint;
    private Vector3 _targetPoint;
    private int _currentPointIndex;
    private int _nextPointIndex;

    private float _totalAngle;

    private bool isBoost = default;

    private void Start()
    {
        _points = _pointsManager.Points;

        _genesisPoint = transform.position;
        _targetPoint = GetNearest().transform.position;
    }
    private void FixedUpdate()
    {
        MoveToPoint();
        if (Vector3.Distance(transform.position, _targetPoint) < 0.5f) NextPoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        RoadController roadController = collision.gameObject.GetComponent<RoadController>();
        if (roadController != null )
        {
            Rigidbody rb = roadController.GetComponent<Rigidbody>();

            rb.AddForce(collision.GetContact(0).point * 4.5f);
            //Destroy(roadController);
        }
    }
    private GameObject GetNearest()
    {
        _distance = new float[_points.Count];

        int size = _distance.Length;
        //float minDistance;
        MethodOne(size);

        int resultIndex = 0;
        MethodTwo(size, out resultIndex);

        return _points[resultIndex];
    }
    private void MethodOne(int size)
    {
        for (int i = 0; i < size; i++)
            _distance[i] = Vector3.Distance(transform.position, _points[i].transform.position);
    }
    private void MethodTwo(int size, out int resultIndex)
    {
        resultIndex = 0;
        //minDistance = distance[0];
        for (int i = 1; i < size; i++)
            if (_distance[resultIndex] > _distance[i])
                resultIndex = i;
        Debug.Log(resultIndex);
        _nextPointIndex = resultIndex;
    }
    private void MoveToPoint()
    {
        transform.LookAt(new Vector3(_targetPoint.x, transform.position.y, _targetPoint.z));

        if (isBoost)
            transform.position += transform.forward * _boostSpeed * Time.deltaTime;
        else 
            transform.position += transform.forward * _speed * Time.deltaTime;
    }
    private void NextPoint()
    {
        _currentPointIndex = _nextPointIndex;
        _nextPointIndex++;

        if (_nextPointIndex >= _points.Count)
        {
            _nextPointIndex = 0;
        }
        _targetPoint = _points[_nextPointIndex].transform.position;

        isStraightRoad();
    }
    private void isStraightRoad()
    {
        int[] predictionIndexes = GetPredictionsPointsIndexes();
        float[] angles = new float[predictionIndexes.Length];

        _totalAngle = CalculateTotalAngle(predictionIndexes, out angles);

        CalculateIfStraight(predictionIndexes);
    }
    private float CalculateTotalAngle(int[] predictionIndexes, out float[] angles)
    {
        float totalAngle = 0;
        angles = new float[predictionIndexes.Length];

        for (int i = 0; i < predictionIndexes.Length; i++)
        {
            angles[i] = _pointsManager.WayPoints[predictionIndexes[i]].Angle;
            totalAngle += angles[i];
        }
        return totalAngle;
    }
    private void CalculateIfStraight(int[] predictionIndexes)
    {
        if (_totalAngle > 15.0f)
        {
            if (_predictionPoints != null)
            {
                //_points.Find(obj => obj == _points[pointer]);
                //predictionPoints.Contains(_points[pointer]);
        
                if (_predictionPoints.Contains(_points[_nextPointIndex]) == true)
                    isBoost = true;
                else
                    isBoost = false;
            }
            else
                isBoost = false;
        }
        else
        {
            isBoost = true;
            _predictionPoints = new List<GameObject>
            {
                _points[predictionIndexes[0]],
                _points[predictionIndexes[1]],
                _points[predictionIndexes[2]],
                _points[predictionIndexes[3]]
            };
        }
    }
    private int[] GetPredictionsPointsIndexes()
    {
        int[] predictionIndexes = new int[4];
        int pointer = _currentPointIndex;

        for (int i = 0; i < 4; i++) 
        {
            if (pointer >= _points.Count)
                pointer = 0;

            predictionIndexes[i] = pointer;
            pointer++;
        }

        return predictionIndexes;
    }
}
