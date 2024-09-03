using UnityEngine;

public class CameraSwapper : MonoBehaviour
{
    [SerializeField] private GameObject[] _targets;
    [SerializeField] private int _current;

    [SerializeField] private Vector3 _offset;

    [SerializeField] private float _speed;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(80, 0, 0);
        _offset = new Vector3(-0.5f, 17f, 0f);

        //transform.rotation = Quaternion.Euler(40, 0, 0);
        //_offset = new Vector3(-3.5f, 5f, 0f);

        _targets = GameObject.FindGameObjectsWithTag("Car");
        _current = 0;
    }
    private void FixedUpdate()
    {
        //Rotate();
        Vector3 destPoint = _targets[_current].transform.position + _offset;
        float movementSpeed = _speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, destPoint, movementSpeed);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Swap();
        }
    }

    private void Swap()
    {
        _current++;
        if (_current >= _targets.Length)
            _current = 0;
    }
    private void Rotate()
    {
        Transform camera = transform;
        Transform target = _targets[_current].transform;

        transform.rotation = Quaternion.RotateTowards(camera.rotation, target.rotation, 360);
        transform.rotation = Quaternion.Euler(camera.rotation.x + 40f, camera.rotation.y, camera.rotation.z);
    }
}
