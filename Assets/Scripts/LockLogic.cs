using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class LockLogic : MonoBehaviour
{
    // Text of interaction example: press e to start breaking lock
    [SerializeField] private TMP_Text _apearInteraction;
    [SerializeField] private Slider _lockHealthSlider;
    [SerializeField] private Transform _lockGameobject;
    [SerializeField] private Transform _PolicemanObject;


    private bool _playerInLockArea = false;
    private bool _breaking = false;

    [SerializeField]
    [Range(0f, 1f)] private float _breakStrength = 0.5f;

    private float _lockCurrentHealth = 10f;
    private const float LockHealthMax = 10f;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E) && _playerInLockArea)
        {
            StartBreakLock();
        }
        
        if (Input.GetMouseButtonDown(0) && _breaking)
        { 
            GetDamage(_breakStrength);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement>())
        {
            _apearInteraction.transform.LeanScale(Vector3.one, 1f).setEaseInOutExpo();
            _playerInLockArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Movement>())
        {
            _apearInteraction.transform.LeanScale(Vector3.zero, 1f).setEaseInOutExpo();
            _playerInLockArea = false;
            EndBreakLock();
        }
    }

    private void StartBreakLock()
    {
        _breaking = true;
        _apearInteraction.transform.LeanScale(Vector3.zero, 0.5f);      
    }

    private void EndBreakLock()
    {
        _breaking = false;     
    }

    private void GetDamage(float damage)
    {    
        float maxDistanceWherePoliceCanHear = 4.5f;     

        if (Vector2.Distance(_lockGameobject.position, _PolicemanObject.position) < maxDistanceWherePoliceCanHear)
        {
            NoticePlayerWeCannotHitLock();
            return;
        }

        _lockCurrentHealth -= damage;
        UpdateHealthBar(_lockCurrentHealth, LockHealthMax);

        if (_lockCurrentHealth <= 0)
        {
            Debug.LogError("LOCK IS DEAD PLEASE WRITE A CODE ANIM OR SOMETHING ELSE");
        }    
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        _lockHealthSlider.value = currentHealth / maxHealth;
    }

    private void NoticePlayerWeCannotHitLock()
    {
        Debug.Log("WE CANT HIT LOCK BECAUSE POLICE CAN HEAR");
    }

}
