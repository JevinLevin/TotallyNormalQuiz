using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class POWERManager : MonoBehaviour
{
    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private POWERAnswer[] powerAnswers;
    [SerializeField] private TextMeshProUGUI title;

    private string originalQuestion;

    public bool powered;
    private int poweredCount;
    
    private string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";


    private void OnEnable()
    {
        questionScript.OnStart += OnStart;
    }

    public void OnWin()
    {
        StopCoroutine(nameof(TitleFlash));
        title.gameObject.SetActive(true);
        title.text = originalQuestion;

        powered = true;

        foreach (POWERAnswer answer in powerAnswers)
        {
            answer.PowerAnimation(GameManager.ButtonGreen, 0.75f, 0.4f, 0.3f,1.5f);
            answer.FadeOutProgress(0.5f);
        }
        
        GameManager.FadeImageColor(GameManager.ButtonGreen, 0.25f, title);
        
        questionScript.GenericAnswerCorrect();
    }

    public void OnStart()
    {
        originalQuestion = title.text;

        StartCoroutine(nameof(TitleFlash));
    }

    public void OnPoweredStart()
    {
        poweredCount++;
        
        CheckWin();
    }

    public void OnPoweredEnd()
    {
        poweredCount--;
        
        CheckWin();
    }

    private void CheckWin()
    {
        if (poweredCount == 4)
        {
            OnWin();
        }
    }

    private void ScrambleQuestion()
    {
        char[] newQuestion = title.text.ToCharArray();
        for (int i = 0; i < newQuestion.Length; i++)
        {
            if (!char.IsWhiteSpace(newQuestion[i]))
            {
                newQuestion[i] = letters[Random.Range(0, letters.Length)];
            }
        }
        title.text = newQuestion.ArrayToString();
    }

    private IEnumerator TitleFlash()
    {
        while (!powered)
        {
            ScrambleQuestion();
            title.gameObject.SetActive(true);

            yield return new WaitForSeconds(Random.Range(0.2f, 0.35f));
        
            title.gameObject.SetActive(false);
        
            if (Random.Range(0, 2) == 1)
            {
                ScrambleQuestion();
                yield return new WaitForSeconds(Random.Range(0.125f, 0.2f));
                
                title.gameObject.SetActive(true);
                
                yield return new WaitForSeconds(Random.Range(0.2f, 0.35f));
                
                title.gameObject.SetActive(false);
            }
            
            yield return new WaitForSeconds(Random.Range(0.75f, 1f));
        }
    }

}
