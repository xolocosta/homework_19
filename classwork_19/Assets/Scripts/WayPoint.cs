public class WayPoint
{
    public float Angle { get => _angle; }
    private float _angle;

    private UnityEngine.Vector3 _dirForNextPoint;
    private UnityEngine.Vector3 _dirFromPrevPoint;

    private UnityEngine.Vector3 _myPos;
    private UnityEngine.Vector3 _prevPos;
    private UnityEngine.Vector3 _nextPos;
    public WayPoint(UnityEngine.Vector3 myPos, UnityEngine.Vector3 prevPos, UnityEngine.Vector3 nextPos)
    {
        _myPos = myPos;
        _prevPos = prevPos;
        _nextPos = nextPos;

        CalculateAngles();
    }
    private void CalculateAngles()
    {
        _dirFromPrevPoint = _prevPos - _myPos;
        _dirForNextPoint = _myPos - _nextPos;
        _angle = System.Math.Abs(UnityEngine.Vector3.Angle(_dirForNextPoint, _dirFromPrevPoint));
    }
}
