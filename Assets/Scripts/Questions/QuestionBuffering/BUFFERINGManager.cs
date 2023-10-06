using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BUFFERINGManager : MonoBehaviour
{

    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private BUFFERINGAnswer[] answers;
    [SerializeField] private GameObject skip;
    [SerializeField] private CanvasGroup skipCanvasGroup;
    private bool buffering;
    void Update()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable && buffering)
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
        questionScript.GenericAnswerCorrect();
    }
}
