using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class QuestionGeneric : MonoBehaviour
{

    [Header("Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup answerCanvasGroup;
    [SerializeField] private RectTransform titleTransform;
    [SerializeField] private RectTransform answerTransform;
    [SerializeField] private TextMeshProUGUI questionNumber;
    [SerializeField] private TextMeshProUGUI questionText;
    

    [Header("Attributes")]
    
    [SerializeField] private Color colourIncorrect;
    [SerializeField] private Color colourCorrect;
    [SerializeField] private bool randomisePlacements;
    
    [Header("Events")]
    [SerializeField] private UnityEvent onReset;
    [SerializeField] private UnityEvent onStart;
    [SerializeField] private UnityEvent onFail;

    [Header("Misc")]
    [SerializeField] public List<AnswerGeneric> answers;

    private AnswerGeneric clickedAnswer;

    void Awake()
    {
        onReset?.Invoke();
        onStart?.Invoke();
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {
        
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
        GameManager.Instance.FadeImageColor(colourIncorrect, 0.25f, answer.frontImage);
    }

    private void ClickAnswerCorrect(AnswerGeneric answer)
    {
        GameManager.Instance.FadeImageColor(colourCorrect, 0.25f, answer.frontImage);
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
    }

    public void GenericAnswerCorrect()
    {
        GenericAnswer();

        Invoke("NextQuestion", 1.5f);
    }

    public void GenericAnswerWrong()
    {
        GenericAnswer();

        onFail?.Invoke();

        StartCoroutine(RestartQuestion());
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

    public IEnumerator RestartQuestion()
    {
        yield return new WaitForSeconds(0.5f);

        FadeQuestionOut();

        yield return new WaitForSeconds(1);

        Reset();
        onReset?.Invoke();
        FadeQuestionIn();

        yield return new WaitForSeconds(0.75f);

        SetInteractable(true);
        onStart?.Invoke();

    }

    private void Reset()
    {
        DOTween.Kill("endingFadeTween");

        GameManager.Instance.restarting = false;

        // Randomises each answers placement
        if(randomisePlacements) RandomPlacements();
        
        DOTween.Complete("answerTween");

        foreach(AnswerGeneric answer in answers)
        {
            answer.SetDefaultColors();
        }

        // Sets all answers to normal visibility again including previously clicked one
        if(clickedAnswer) clickedAnswer.canvasGroup.ignoreParentGroups = false;
        if(clickedAnswer) clickedAnswer.canvasGroup.DOFade(1,0.0f).SetId("answerTween");
        answerCanvasGroup.DOFade(1,0.0f).SetId("answerTween");
    }

    public void RandomPlacements()
    {
        foreach(AnswerGeneric answer in answers)
        {
            answer.RandomisePlacement(answers.Count);
        }
    }

    private void NextQuestion()
    {
        answerTransform.DOLocalMoveX(-2000,0.75f).SetEase(Ease.InOutCubic).SetId("questionTween");
        titleTransform.DOLocalMoveX(-2000,0.75f).SetEase(Ease.InOutCubic).SetId("questionTween");
        SetInteractable(false);

        GameManager.Instance.NextQuestion();
    }

    public void NewQuestion()
    {
        SetInteractable(false);

        // Move off screen, then onto screen
        answerTransform.localPosition = new Vector3(2000,answerTransform.localPosition.y,answerTransform.localPosition.z);
        titleTransform.localPosition = new Vector3(2000,titleTransform.localPosition.y,titleTransform.localPosition.z);
        answerTransform.DOLocalMoveX(0,0.75f).SetEase(Ease.InOutCubic).SetId("endingTween");
        titleTransform.DOLocalMoveX(0,0.75f).SetEase(Ease.InOutCubic).SetId("endingTween").OnComplete(() =>
        NewQuestionEnd());
    }

    private void NewQuestionEnd()
    {
        SetInteractable(true);
        GameManager.Instance.RemoveOldQuestion();
    }

    public void SetQuestionTitle(string value)
    {
        questionText.text = value;
    }


}
