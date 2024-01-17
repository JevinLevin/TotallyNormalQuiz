using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class SEQUENCEManager : MonoBehaviour
{
    [SerializeField] QuestionGeneric questionScript;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] float fadeInTime;
    [SerializeField] float fadeOutTime;
    [SerializeField] private List<SEQUENCEAnswer> answers;
    [SerializeField,AssetsOnly] private SequenceScriptableObject sequenceScriptableObject;
    [SerializeField] private Button skipButton;
    private CanvasGroup skipButtonCanvas;
    private SEQUENCEEntry[] sequence;
    private int sequencePosition;
    private bool showSkip;

    public void Awake()
    {
        sequence = sequenceScriptableObject.sequence;
        skipButton.gameObject.SetActive(false);
        skipButtonCanvas = skipButton.GetComponent<CanvasGroup>();
    }

    public void ResetSequence()
    {
        titleText.DOFade(0.0f,0.0f).SetId("questionTween");
        foreach(SEQUENCEAnswer answer in answers)
        {
            answer.Reset();
        }

        if (showSkip) 
        {
            skipButton.gameObject.SetActive(true);
            skipButtonCanvas.ignoreParentGroups = false;
        }
        showSkip = true;
    }

    public void SkipSequence()
    {
        StopCoroutine("PlaySequence");
        EndSequence();
        foreach(SEQUENCEAnswer answer in answers)
        {
            answer.Reset();
        }
    }

    public void StartSequence()
    {
        skipButtonCanvas.ignoreParentGroups = true;
        sequencePosition = 0;
        StartCoroutine("PlaySequence");
        questionScript.SetInteractable(false);
        
    }

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(0.5f);
        foreach(SEQUENCEEntry entry in sequence)
        {
            GameManager.Instance.FadeImageColorInOut(answers[entry.answerNumber-1].GetColor(), fadeInTime / (entry.delay*2),fadeOutTime / (entry.delay*2),answers[entry.answerNumber-1].frontImage);
            yield return new WaitForSeconds(entry.delay);
        }
        yield return new WaitForSeconds(0.5f);

        EndSequence();
    }

    private void EndSequence()
    {
        skipButton.gameObject.SetActive(false);
        questionScript.SetInteractable(true);
        titleText.DOFade(1.0f,0.5f).SetId("questionTween");
    }

    public void OnClickAnswer(SEQUENCEAnswer answer)
    {
        if(answer.GetNumber() == sequence[sequencePosition].answerNumber)
        {
            sequencePosition++;
            if(sequencePosition == sequence.Length)
            {
                Win();
            }
            else
            {
                GameManager.Instance.FadeImageColorInOut(answer.GetColor(), 0.15f, 0.35f, answer.backImage);
            }
        }
        else
        {
            Fail();
        }
    }

    private void Fail()
    {
        foreach(AnswerGeneric answer in questionScript.Answers)
        {
            GameManager.Instance.FadeImageColor(GameManager.Instance.buttonRed, 0.25f, answer.backImage);
        }
        questionScript.GenericAnswerWrong();
    }

    private void Win()
    {
        for(int i = 0; i < answers.Count; i++)
        {
            answers[i].ResetFrontImage();
            GameManager.Instance.FlashImageColor(GameManager.Instance.buttonGreen, 0.25f, answers[i].backImage);
        }
        questionScript.GenericAnswerCorrect();
    }




}
