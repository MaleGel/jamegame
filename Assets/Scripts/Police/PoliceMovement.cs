using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PoliceMovement : MonoBehaviour
{
    [SerializeField] private Transform[] _pointsPath;

    private int _pointIndexToMove = 0;
    
    private Rigidbody2D _rb;

    private float _durationFromOneToAnotherPoint = 3f;

    private float _startTime;
    private void Awake()
    {
        _startTime = Time.time;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        var t = (Time.time - _startTime) / _durationFromOneToAnotherPoint;
        t = Mathf.Clamp01(t);
        if (_pointIndexToMove < _pointsPath.Length)
        {
            _rb.MovePosition(Vector2.Lerp(_rb.transform.position, _pointsPath[_pointIndexToMove].position, t * Time.fixedDeltaTime));
           
            if (Vector2.Distance(_rb.position, _pointsPath[_pointIndexToMove].position) < 1f)
            {
                _startTime = Time.time; 
                _pointIndexToMove++;
            }
        }
        else
        {
            _pointIndexToMove = 0;
        }
    }
}
