using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{ 
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance 
    { 
        get { return _instance; } 
    }

    private void CreateSingleton()
    {
        if (_instance != null && _instance != this) 
        { 
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion
    

    [Header("Components")] 
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] public Camera QuestionCamera;

    [SerializeField] private QuitAnimation quitAnimation;
    [Header("Objects")] 
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Questions")]
    [AssetsOnly] public List<GameObject> questionList;
    public int questionStartingNumber;
    [HideInInspector] public int questionNumber;
    private QuestionGeneric currentQuestion;
    private QuestionGeneric oldQuestion;
    public bool restarting;

    [Header("Colors")]
    [SerializeField] private Color buttonHoverColor;
    public static Color ButtonHoverColor;
    [SerializeField] private Color buttonRed;
    public static Color ButtonRed;
    [SerializeField] private Color buttonBlue;
    public static Color ButtonBlue;
    [SerializeField] private Color buttonGreen;
    public static Color ButtonGreen;
    [SerializeField] private Color buttonYellow;
    public static Color ButtonYellow;
    [SerializeField] private Color buttonDarkBlue;
    public static Color ButtonDarkBlue;
    [SerializeField] private Color buttonPink;
    public static Color ButtonPink;
    [SerializeField] private Color buttonOrange;
    public static Color ButtonOrange;
    [SerializeField] private Color buttonAqua;
    public static Color ButtonAqua;
    [SerializeField] private Color buttonPurple;
    public static Color ButtonPurple;

    [Header("Values")] 

    [ClearOnReload]
    public static bool paused;
    [ClearOnReload(valueToAssign:true)]
    public static bool canPause = true;

    public static float volume;
    

    void Update()
    {
        if (canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }
    }


    private void Awake() 
    { 
        CreateSingleton();

        float currentVolume;
       audioMixer.GetFloat("MasterVolume", out currentVolume);
       volume = NormaliseVolume(currentVolume);
       
       // Set colors
       ButtonHoverColor = buttonHoverColor;
       ButtonRed = buttonRed;
       ButtonBlue = buttonBlue;
       ButtonGreen = buttonGreen;
       ButtonYellow = buttonYellow;
       ButtonDarkBlue = buttonDarkBlue;
       ButtonPink = buttonPink;
       ButtonOrange = buttonOrange;
       ButtonAqua = buttonAqua;
       ButtonPurple = buttonPurple;

    }

    private void Start()
    {
        if (GameObject.FindWithTag("Question") == null)
        {
            questionNumber = questionStartingNumber;
            currentQuestion = CreateQuestion(questionNumber);
        }
        else
        {
            currentQuestion = GameObject.FindWithTag("Question").GetComponent<QuestionGeneric>();
        }
        
        currentQuestion.OnReset?.Invoke();
        currentQuestion.OnStart?.Invoke();
    }

    public void OnPause()
    {
        paused = !paused;
        
        pauseMenu.OnPause(paused);
    }

    public QuestionGeneric CreateQuestion(int setQuestionNumber)
    {
        return Instantiate(questionList[setQuestionNumber], Vector3.zero, Quaternion.identity).GetComponent<QuestionGeneric>();
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



    public static Tween FadeImageColorInOut(Color fadeColor, float fadeInTime, float fadeOutTime, Image image)
    {
        DOTween.Complete(image);
        Color defaultColor = image.color;

        return image.DOColor(fadeColor, fadeInTime).SetId("answerTween").OnComplete(() =>
        image.DOColor(defaultColor, fadeOutTime).SetEase(Ease.InQuad).SetId("answerTween"));

    }

    public static Tween FadeImageColor(Color fadeColor, float fadeInTime, Image image)
    {
        return image.DOColor(fadeColor, fadeInTime).SetId("answerTween");

    }

    public static Tween FadeImageColor(Color fadeColor, float fadeInTime, TextMeshProUGUI text)
    {
        return text.DOColor(fadeColor, fadeInTime).SetId("answerTween");

    }

    public static Tween FlashImageColor(Color fadeColor, float fadeTime, Image image)
    {
        DOTween.Complete(image);
        Color defaultColor = image.color;

        return DOTween.Sequence()
            .Append(image.DOColor(fadeColor, fadeTime).SetId("answerTween").SetEase(Ease.OutQuad))
            .Append(image.DOColor(defaultColor, fadeTime).SetId("answerTween").SetEase(Ease.InSine))
            .SetId("answerTween")
            .SetLoops(-1)
            .Play();

    }

    public void SetVolume(Single value)
    {
        volume = value;
        audioMixer.SetFloat(("MasterVolume"), UnormaliseVolume(value));

    }

    private float NormaliseVolume(float value)
    {
        return (value + 80) / 80;
    }
    
    private float UnormaliseVolume(float value)
    {
        return -80 - (value * -80);
    }

    public void QuitGame()
    {
        quitAnimation.gameObject.SetActive(true);
        StartCoroutine(quitAnimation.PlayAnimation());
    }
}
