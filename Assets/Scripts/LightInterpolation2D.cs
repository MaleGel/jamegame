using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class LightInterpolation2D : MonoBehaviour
{

    [Header("Objects with Light 2D script that represents shadow")]
    [SerializeField] private Light2D[] _shadows;

    [Header("Angles to change shadow ")]
    [SerializeField]
    [Range(0f, 180f)] private float[] _angles;

    // Objet that represents sun light
    [SerializeField] private Light2D _sun;

    private float _delayToCheckSunAngleInSeconds = 1f;

    private void Awake()
    {   
        if (_shadows.Length != _angles.Length)
        {
            throw new Exception("Number of shadows shoud be equal number of angles");
        }

        _shadows[0].gameObject.SetActive(true);

        foreach (var shadow in _shadows[1..])
        {
            shadow.gameObject.SetActive(false);
        }

        StartCoroutine(CheckSunAngle());
    }

    IEnumerator CheckSunAngle()
    {
        
        while (true)
        {
            if (DayNightManager.IsDay)
            {
                Vector2 playerVector = gameObject.transform.position;
                Vector2 targetVector = _sun.transform.position;
                float angle = Vector2.SignedAngle(playerVector, targetVector);
                int should_cast_shadow = 0;
                for (int i = 0; i < _angles.Length - 1; i++)
                {
                    if (angle < _angles[i] && angle > _angles[i + 1])
                    {
                        should_cast_shadow = i;
                        break;
                    }
                    if (angle < _angles[^1])
                    {
                        should_cast_shadow = _angles.Length - 1;
                        break;
                    }
                }

                for (int i = 0; i < _shadows.Length; i++)
                {
                    if (should_cast_shadow == i)
                    {
                        _shadows[i].gameObject.SetActive(true);
                        continue;
                    }

                    _shadows[i].gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (var shadow in _shadows)
                {
                    shadow.gameObject.SetActive(false);
                }
            }
            
            yield return new WaitForSeconds(_delayToCheckSunAngleInSeconds);
        }
    }

    void Update()
    {
        
    }
}
