using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class BUFFERINGManager : MonoBehaviour
{

    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private BUFFERINGAnswer[] answers;
    [SerializeField] private GameObject skip;
    [SerializeField] private CanvasGroup skipCanvasGroup;
    [SerializeField] private Image skipImage;
    private bool buffering;
    void Update()
    {
        if(!InternetAvailability.IsInternetAvailable() && buffering)
            {
                DownloadFailed();
                buffering = false;
            }
 
    }

    public void OnReset()
    {
        skip.SetActive(false);
        skipCanvasGroup.alpha = -0;
        buffering = true;
        foreach(BUFFERINGAnswer answer in answers)
        {
            answer.Reset();
        }        
    }

    private void DownloadFailed()
    {
        skip.SetActive(true);
        StartCoroutine(RevealSkip());
        questionScript.SetQuestionTitle("DOWNLOAD FAILED");
        foreach(BUFFERINGAnswer answer in answers)
        {
            answer.NoConnection();
        }
    }

    private IEnumerator RevealSkip()
    {
        yield return new WaitForSeconds(1);

        skipCanvasGroup.DOFade(1.0f,1.5f).SetEase(Ease.OutSine);
    }

    public void ClickAnswer()
    {
        GameManager.Instance.FadeImageColor(GameManager.Instance.buttonGreen, 0.25f, skipImage);

        questionScript.GenericAnswerCorrect();
    }
}
