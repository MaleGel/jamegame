using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogsTrigger : MonoBehaviour
{
    [SerializeField] DialogManager dialogManager;

    public void StartDialog(string name)
    {
        dialogManager.DialogueStart(name);
    }
}
