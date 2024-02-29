using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class QuestionGeneric : MonoBehaviour
{

    [Header("Components")]
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    [SerializeField] private CanvasGroup answerCanvasGroup;
    [SerializeField] private RectTransform titleTransform;
    [SerializeField] private RectTransform answerTransform;
    [SerializeField] private TextMeshProUGUI questionNumber;
    [SerializeField] private TextMeshProUGUI questionText;
    

    [Header("Attributes")]
    
    [SerializeField] private Color colourIncorrect;
    [SerializeField] private Color colourCorrect;
    [SerializeField] private bool randomisePlacements;
    
    // Events
    public Action OnReset;
    public Action OnStart;
    public Action OnFail;
    public Action OnWin;
    
    public bool IsActive { get; private set; }

    public List<AnswerGeneric> Answers { get; private set; }

    private AnswerGeneric clickedAnswer;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        Answers = GetComponentsInChildren<AnswerGeneric>(true).ToList();
    }

    void Start()
    {
        canvas.worldCamera = GameManager.Instance.QuestionCamera;
        
        Reset();
        
        IsActive = true;
    }

    void OnDestroy()
    {
        DOTween.Kill("questionTween");
    }

    public void SetInteractable(bool value)
    {
    
        canvasGroup.interactable = value;
        canvasGroup.blocksRaycasts = value;
    }

    private void ClickAnswerWrong(AnswerGeneric answer)
    {
        GameManager.FadeImageColor(colourIncorrect, 0.25f, answer.frontImage);
    }

    private void ClickAnswerCorrect(AnswerGeneric answer)
    {
        GameManager.FadeImageColor(colourCorrect, 0.25f, answer.frontImage);
    }

    public void ClickAnswerGenericEvent(AnswerGeneric answer)
    {
        ClickAnswerGeneric(answer, answer.GetCorrect());
    }

    public void ClickAnswerGeneric(AnswerGeneric answer, bool correct)
    {
        // Sets the clicked answer so it doesn't get fadd out
        clickedAnswer = answer;
        clickedAnswer.canvasGroup.ignoreParentGroups = true;
        clickedAnswer.canvasGroup.interactable = false;
        clickedAnswer.canvasGroup.blocksRaycasts = false;

        if(correct) 
        {
            ClickAnswerCorrect(answer);
            GenericAnswerCorrect();
        }
        else
        {
            ClickAnswerWrong(answer);
            GenericAnswerWrong();
        }
    }

    public void GenericAnswer()
    {
        // Prevents clicking and hovering
        SetInteractable(false);

        // Ensures the button animation doesnt overwrite any fading
        GameManager.Instance.restarting = true;

        // Fades the answer canvas slightly
        FadeAnswers();
        
        // Stop any button color animations
        DOTween.Kill("answerColorTween");
        
        IsActive = false;
    }

    public void GenericAnswerCorrect(bool fadeAnswers = false)
    {
        GenericAnswer();
        
        OnWin?.Invoke();

        Invoke("NextQuestion", 1.5f);

        if (!fadeAnswers) return;
        FlashAnswersFront(GameManager.FadeImageColor, colourCorrect, 0.25f);

    }

    public void GenericAnswerWrong(bool fadeAnswers = false)
    {
        GenericAnswer();

        //onFail?.Invoke();
        OnFail?.Invoke();

        StartCoroutine(RestartQuestion());
        
        if (!fadeAnswers) return;
        FlashAnswersFront(GameManager.FadeImageColor, colourIncorrect, 0.25f);
    }

    private void FadeAnswers()
    {
        answerCanvasGroup.DOFade(0.1f, 2f).SetId("endingFadeTween");
    }

    private void FadeQuestionOut()
    {
        if(clickedAnswer) clickedAnswer.canvasGroup.DOFade(0.0f,1.0f).SetEase(Ease.InSine).SetId("questionTween");
        canvasGroup.DOFade(0.0f,1.0f).SetEase(Ease.InSine).SetId("questionTween");
    }

    private void FadeQuestionIn()
    {
        if(clickedAnswer) clickedAnswer.canvasGroup.DOFade(1.0f,0.75f).SetEase(Ease.InCubic).SetId("questionTween");
        canvasGroup.DOFade(1.0f,0.75f).SetEase(Ease.InCubic).SetId("questionTween");
    }

    private IEnumerator RestartQuestion()
    {
        yield return new WaitForSeconds(0.5f);

        FadeQuestionOut();

        yield return new WaitForSeconds(1);

        Reset();
        OnReset?.Invoke();
        FadeQuestionIn();

        yield return new WaitForSeconds(0.75f);

        SetInteractable(true);
        IsActive = true;
        OnStart?.Invoke();

    }

    protected virtual void Reset()
    {
        DOTween.Kill("endingFadeTween");

        GameManager.Instance.restarting = false;

        // Randomises each answers placement
        if(randomisePlacements) RandomPlacements();
        
        DOTween.Complete("answerTween");

        foreach(AnswerGeneric answer in Answers)
        {
            answer.SetDefaultColors();
        }

        // Sets all answers to normal visibility again including previously clicked one
        if (clickedAnswer)
        {
            clickedAnswer.canvasGroup.ignoreParentGroups = false;
            clickedAnswer.canvasGroup.interactable = true;
            clickedAnswer.canvasGroup.blocksRaycasts = true;
            clickedAnswer.canvasGroup.DOFade(1,0.0f).SetId("answerTween");
        }
        answerCanvasGroup.DOFade(1,0.0f).SetId("answerTween");
    }

    public void RandomPlacements()
    {
        foreach(AnswerGeneric answer in Answers)
        {
            answer.RandomisePlacement(Answers.Count);
        }
    }

    private void NextQuestion()
    {
        answerTransform.DOLocalMoveX(-2000,0.75f).SetEase(Ease.InOutCubic).SetId("questionTween");
        titleTransform.DOLocalMoveX(-2000,0.75f).SetEase(Ease.InOutCubic).SetId("questionTween");
        SetInteractable(false);
        
        OnReset?.Invoke();
        OnStart?.Invoke();

        GameManager.Instance.NextQuestion();
    }

    public void NewQuestion()
    {
        SetInteractable(false);
        IsActive = false;

        Vector3 answerPosition = answerTransform.localPosition;
        Vector3 titlePosition = titleTransform.localPosition;

        // Move off screen, then onto screen
        answerTransform.localPosition = new Vector3(2000,answerPosition.y,answerPosition.z);
        titleTransform.localPosition = new Vector3(2000,titlePosition.y,titlePosition.z);
        answerTransform.DOLocalMoveX(0,0.75f).SetEase(Ease.InOutCubic).SetId("endingTween");
        titleTransform.DOLocalMoveX(0,0.75f).SetEase(Ease.InOutCubic).SetId("endingTween").OnComplete(() =>
        NewQuestionEnd());
    }

    private void NewQuestionEnd()
    {
        SetInteractable(true);
        IsActive = true;
        GameManager.Instance.RemoveOldQuestion();
    }

    public void SetQuestionTitle(string value)
    {
        questionText.text = value;
    }
    
    
    
    // I hate that this is the only way i could think of doing this
    protected void FlashAnswersBack(Func<Color,float,Image,Tween> flashEvent, Color flashColor, float flashLength)
    {
        foreach (AnswerGeneric answer in Answers)
        {
            flashEvent.Invoke(flashColor, flashLength, answer.backImage);
        }
    }
    protected void FlashAnswersBack(Func<Color,float,float,Image,Tween> flashEvent, Color flashColor, float flashInLength,float flashOutLength)
    {
        foreach (AnswerGeneric answer in Answers)
        {
            flashEvent.Invoke(flashColor, flashInLength, flashOutLength, answer.backImage);
        }
    }
    protected void FlashAnswersFront(Func<Color,float,Image,Tween> flashEvent, Color flashColor, float flashLength)
    {
        foreach (AnswerGeneric answer in Answers)
        {
            flashEvent.Invoke(flashColor, flashLength, answer.frontImage);
        }
    }
    protected void FlashAnswersFront(Func<Color,float,float,Image,Tween> flashEvent, Color flashColor, float flashInLength,float flashOutLength)
    {
        foreach (AnswerGeneric answer in Answers)
        {
            flashEvent.Invoke(flashColor, flashInLength, flashOutLength, answer.frontImage);
        }
    }


}
