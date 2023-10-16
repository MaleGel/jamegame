using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightManager : MonoBehaviour
{
    [Header("Night and day object")]
    [SerializeField] private Light2D _dayObject;
    [SerializeField] private Light2D _nightObject;

    [Header("Day path")]
    [SerializeField] private Transform _p0d;
    [SerializeField] private Transform _p1d;
    [SerializeField] private Transform _p2d;
    [SerializeField] private Transform _p3d;

    [Header("Night path")]
    [SerializeField] private Transform _p0n;
    [SerializeField] private Transform _p1n;
    [SerializeField] private Transform _p2n;
    [SerializeField] private Transform _p3n;

    [SerializeField] float _duration = 5.0f;
    float startTime;
    private static float tDay = 0f;
    private static float tNight = 0f;

    public static bool IsDay => tDay >= 0 && tDay < 1;


    private void Awake()
    {
        float startTime = Time.time;
    }

    private void Update()
    {
        if (IsDay)
        {
            DayCycle();
        }
        else if (!IsDay)
        {
            NightCycle();
        }
           
    }

    private void DayCycle()
    {
        _dayObject.gameObject.SetActive(true);
        _nightObject.gameObject.SetActive(false);
        tDay = (Time.time - startTime) / _duration;
        tDay = Mathf.Clamp01(tDay);
        _dayObject.transform.position = Bezier.GetPoint(_p0d.position, _p1d.position, _p2d.position, _p3d.position, tDay);
        if (tDay >= 1)
        {
            tNight = 0f;
            startTime = Time.time;
        }        
    }

    private void NightCycle()
    {
        _dayObject.gameObject.SetActive(false);
        _nightObject.gameObject.SetActive(true);
        tNight = (Time.time - startTime) / _duration;
        tNight = Mathf.Clamp01(tNight);
        _nightObject.transform.position = Bezier.GetPoint(_p0n.position, _p1n.position, _p2n.position, _p3n.position, tNight);
        if (tNight >= 1)
        {
            tDay = 0f;
            startTime = Time.time;
        }
    }

    /// <summary>
    /// Draws bezier curve
    /// </summary>
    private void OnDrawGizmos()
    {
        int segmentsNumber = 20;
        Vector3 prevPoint = _p0d.position;

        for (int i = 0; i < segmentsNumber; i++)
        {
            float parameter = (float) i / segmentsNumber;
            Vector3 point = Bezier.GetPoint(_p0d.position, _p1d.position, _p2d.position, _p3d.position, parameter);
            Gizmos.DrawLine(prevPoint, point);
            prevPoint = point;
        }

    }
}
