using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{ 
    private static GameManager _instance;

    public static GameManager Instance 
    { 
        get { return _instance; } 
    } 


    [Header("Questions")]
    public List<GameObject> questionList;
    public int questionStartingNumber;
    [HideInInspector] public int questionNumber;
    private QuestionGeneric currentQuestion;
    private QuestionGeneric oldQuestion;
    public bool restarting;

    [Header("Colors")]
    public Color buttonHoverColor;
    public Color buttonRed;
    public Color buttonBlue;
    public Color buttonGreen;
    public Color buttonYellow;
    public Color buttonDarkBlue;
    public Color buttonPink;
    public Color buttonOrange;
    public Color buttonAqua;
    public Color buttonPurple;


    
    private Sequence flashSequence;


    private void Awake() 
    { 
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

       if (GameObject.FindWithTag("Question") == null)
       {
        questionNumber = questionStartingNumber;
        currentQuestion = CreateQuestion(questionNumber);
       }
       else
       {
        currentQuestion = GameObject.FindWithTag("Question").GetComponent<QuestionGeneric>();
       }

    } 

    public QuestionGeneric CreateQuestion(int questionNumber)
    {
        return Instantiate(questionList[questionNumber], Vector3.zero, Quaternion.identity).GetComponent<QuestionGeneric>();
    }

    public void NextQuestion()
    {
        oldQuestion = currentQuestion;
        
        questionNumber++;

        // Create new question
        currentQuestion = CreateQuestion(questionNumber);
        currentQuestion.NewQuestion();
    }

    public void RemoveOldQuestion()
    {
        DOTween.Complete("endingTween");
        Destroy(oldQuestion.gameObject);
    }



    public void FadeImageColorInOut(Color fadeColor, float fadeInTime, float fadeOutTime, Image image)
    {
        DOTween.Complete(image);
        Color defaultColor = image.color;

        image.DOColor(fadeColor, fadeInTime).SetId("answerTween").OnComplete(() =>
        image.DOColor(defaultColor, fadeOutTime).SetEase(Ease.InQuad).SetId("answerTween"));

    }

    public void FadeImageColor(Color fadeColor, float fadeInTime, Image image)
    {
        image.DOColor(fadeColor, fadeInTime).SetId("answerTween");

    }

    public void FlashImageColor(Color fadeColor, float fadeTime, Image image)
    {
        DOTween.Complete(image);
        Color defaultColor = image.color;

        flashSequence = DOTween.Sequence();
        flashSequence.Append(image.DOColor(fadeColor, fadeTime).SetId("answerTween").SetEase(Ease.OutQuad));
        flashSequence.Append(image.DOColor(defaultColor, fadeTime).SetId("answerTween").SetEase(Ease.InSine));

        flashSequence.SetId("answerTween").SetLoops(-1);
        flashSequence.Play();

    }
}