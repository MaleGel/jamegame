using System.Collections;
using System.Net.WebSockets;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class DiceGame : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerText;
    [SerializeField] private TMP_Text _opponentText;
    [SerializeField] private TMP_Text _result;


    private Color[] colors = new Color[] {
        new Color(255, 0, 0), 
        new Color(0, 255, 0), 
        new Color(255, 192, 203),
        new Color(100, 0, 200),
        new Color(255, 255, 0),
        new Color(255, 127, 80)
    };

    private int _playerScore = 0;
    private int _opponentScore = 0;

    private const float DelayBetweenTurns = 3f;

    private const float Turns = 3f;

    private const float ScaleTime = 1.5f;
    private const float ElasticDelay = 1f;

    private void Awake()
    {
        if (DelayBetweenTurns < ScaleTime + ElasticDelay)
            throw new System.Exception("Delay between turns shoud be bigger than scale time + elastic delay");
    }


    private void Start()
    {
        StartCoroutine(StartDicing());
    }

    private IEnumerator StartDicing()
    {
        for(int i = 1; i <= Turns; i++)
        {
            Dice();
            yield return new WaitForSeconds(DelayBetweenTurns);
        }

        ShowFancyResult(_result);
    }

    private void Dice()
    {
        int score = Random.Range(1, 6);
        var playerResult = 6 - score >= 3? score + Random.Range(1, 3): score + 1;

        if (score < 6)
        {
            _playerScore += playerResult;
            _opponentScore += score;    
        }

        ShowFancyScore(_opponentText, score.ToString());
        ShowFancyScore(_playerText, playerResult.ToString());      
    }

    private void ShowFancyScore(TMP_Text textMeshPro, string score)
    {
        textMeshPro.transform.localScale = Vector3.zero;
        textMeshPro.color = colors[Random.Range(0, colors.Length - 1)];
        textMeshPro.text = score;
        textMeshPro.transform.LeanScale(Vector3.one, ScaleTime).setEaseOutElastic().delay = ElasticDelay;   
    }

    private void ShowFancyResult(TMP_Text result)
    {
        result.transform.localScale = Vector3.zero;
        result.color = new Color(173, 216, 230);
        result.text = $"You are winner!\n Your score {_playerScore} and opponent's {_opponentScore}";    
        result.transform.LeanScale(Vector3.one, 1f).setEaseOutBounce().delay = 0.5f;
    }
}
