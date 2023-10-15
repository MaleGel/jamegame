using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using TMPro;

public class DialogManager : MonoBehaviour
{
    private string folder = "Dialogs";

    private string fileName, lastName;
    private List<Dialogue> node;
    private Dialogue dialogue;
    private List<RectTransform> buttons = new List<RectTransform>();
    private static DialogManager _internal;

    [SerializeField] private AudioSource audioManager;
    [SerializeField] private AudioClip textSound;

    [SerializeField] private TextMeshProUGUI _speekerRightName;
    [SerializeField] private TextMeshProUGUI _speekerLeftName;
    [SerializeField] private TextMeshProUGUI _speakerText;
    [SerializeField] private GameObject _leftSpeakerNameObj;
    [SerializeField] private GameObject _rightSpeakerNameObj;

    private bool talking;

    [SerializeField] private GameObject _speakerTextObj;
    [SerializeField] private GameObject _textBG;
    [SerializeField] private GameObject _rightSpriteObj;
    [SerializeField] private GameObject _leftSpriteObj;
    [SerializeField] private Button answerButton;

    public void DialogueStart(string name)
    {
        if (name == string.Empty) return;
        fileName = name;
        Load();
        Debug.Log("Started comix ID: " + name);
    }

    public static DialogManager Internal
    {
        get { return _internal; }
    }

    void Awake()
    {
        _internal = this;
    }
    
    void Load()
    {
        dialogue = null;
        node = new List<Dialogue>();

        try
        {
            TextAsset binary = Resources.Load<TextAsset>(folder + "/" + fileName);
            XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));

            int index = 0;
            while (reader.Read())
            {
                if (reader.IsStartElement("node"))
                {
                    dialogue = new Dialogue();
                    int id;
                    if (int.TryParse(reader.GetAttribute("id"), out id)) dialogue.nodeID = id; else dialogue.nodeID = 0;
                    int toNodeId;
                    if (int.TryParse(reader.GetAttribute("toNode"), out toNodeId)) dialogue.toNode = toNodeId; else dialogue.toNode = 0;
                    bool type;
                    if (bool.TryParse(reader.GetAttribute("player"), out type)) dialogue.character = type; else dialogue.character = false;
                    dialogue.talkerText = reader.GetAttribute("talkerText");
                    dialogue.rightTalkerName = reader.GetAttribute("rightTalkerName");
                    dialogue.rightPerson = reader.GetAttribute("rightPerson");
                    dialogue.leftTalkerName = reader.GetAttribute("leftTalkerName");
                    dialogue.leftPerson = reader.GetAttribute("leftPerson");
                    bool result;
                    if (bool.TryParse(reader.GetAttribute("exit"), out result)) dialogue.exit = result; else dialogue.exit = false;
                    node.Add(dialogue);

                    index++;
                }
            }
            _speakerTextObj.SetActive(true);
            _textBG.SetActive(true);
            answerButton.gameObject.SetActive(true);
            lastName = fileName;
            reader.Close();
        }
        catch (System.Exception error)
        {
            Debug.LogError(this + "--- ERROR WHILE TRY TO READ COMICS FILE: " + folder + "\\" + fileName + ".xml >> Error: " + error.Message + "---");
            lastName = string.Empty;
        }

        BuildDialogue(0);
    }

    void AddToList(int nodeID, string rightTalkerName, string rightPerson, string leftTalkerName, string leftPerson, bool type)
    {
        BuildElement(nodeID, rightTalkerName, rightPerson, leftTalkerName, leftPerson, type);
    }

    void BuildElement(int nodeID, string rightTalkerName, string rightPerson, string leftTalkerName, string leftPerson, bool type)
    {
        ChangeText(nodeID);

        _speakerTextObj.SetActive(true);
        _textBG.SetActive(true);
        answerButton.gameObject.SetActive(true);
        answerButton.interactable = true;
        answerButton.GetComponent<Button>().enabled = true;
        BuildButtons(nodeID);
    }

    void BuildButtons(int id)
    {
        //answerButton.onClick.AddListener(() => StopPrinting(id));

        if (node[id].exit)
        {
            SetExitDialogue(answerButton.GetComponent<Button>(), node[id].nodeID);
            talking = false;
        }
        else if (!node[id].exit)
        {
            try
            {
                SetNextDialogue(answerButton, node[id].toNode);
            }
            catch
            {
                SetExitDialogue(answerButton.GetComponent<Button>(), node[id].nodeID);
            }
                
            talking = false;
        }
    }

    void StopPrinting(int id)
    {
        if (talking)
        {
            StopAllCoroutines();
            _speakerText.text = node[id].talkerText;
            talking= false;
        }
    }

    void ClearDialogue()
    {
        foreach (RectTransform b in buttons)
        {
            Destroy(b.gameObject);
        }
    }

    void SetNextDialogue(Button button, int id)
    {
        button.onClick.AddListener(() => BuildDialogue(id));
    }

    void SetExitDialogue(Button button, int id)
    {
        button.onClick.AddListener(() => CloseDialogue());
    }

    void CloseDialogue()
    {
        ClearDialogue();
        _speakerTextObj.SetActive(false);
        _textBG.SetActive(false);
        answerButton.gameObject.SetActive(false);
    }

    void BuildDialogue(int current)
    {
        if (!talking)
            AddToList(node[current].nodeID, node[current].rightTalkerName, node[current].rightPerson, node[current].leftTalkerName, node[current].leftPerson, node[current].character);
    }
   
    void ChangeText(int current)
    {
        _speakerText.text = "";
        Debug.Log(node[current]);
        StopAllCoroutines();
        if (node[current].talkerText != null)
            StartCoroutine(TextCoroutine(node[current].talkerText));
    }

    IEnumerator TextCoroutine(string text)
    {
        talking = true;
        for (int i = 0; i < text.Length + 1; i++)
        {
            string currText = text.Substring(0, i);

            _speakerText.text = currText;
            audioManager.PlayOneShot(textSound, 0.4f);
            yield return new WaitForSeconds(.02f);
        }
        talking = false;
    }
}

class Dialogue
{
    public int nodeID;
    public int toNode;
    public string talkerText;
    public string rightTalkerName;
    public string leftTalkerName;
    public string rightPerson;
    public string leftPerson;
    public bool character;
    public bool exit;
}