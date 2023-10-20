using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogsTrigger : MonoBehaviour
{
    [SerializeField] DialogManager _dialogManager;
    [SerializeField] GameObject _dialogManagerObj;

    public void StartDialog(string name)
    {
        _dialogManagerObj.SetActive(true);
        _dialogManager.DialogueStart(name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartDialog("test");
        }
    }
}
