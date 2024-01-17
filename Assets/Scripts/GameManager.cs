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
    private static GameManager _instance;

    public static GameManager Instance 
    { 
        get { return _instance; } 
    }

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

    [Header("Values")] 

    [ClearOnReload]
    public static bool paused;
    [ClearOnReload(valueToAssign:true)]
    public static bool canPause = true;

    public static float volume;


    private Sequence flashSequence;

    void Update()
    {
        if (canPause && Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }
    }


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

       float currentVolume;
       audioMixer.GetFloat("MasterVolume", out currentVolume);
       volume = NormaliseVolume(currentVolume);

    }

    private void Start()
    {
        currentQuestion.onReset?.Invoke();
        currentQuestion.onStart?.Invoke();
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



    public Tween FadeImageColorInOut(Color fadeColor, float fadeInTime, float fadeOutTime, Image image)
    {
        DOTween.Complete(image);
        Color defaultColor = image.color;

        return image.DOColor(fadeColor, fadeInTime).SetId("answerTween").OnComplete(() =>
        image.DOColor(defaultColor, fadeOutTime).SetEase(Ease.InQuad).SetId("answerTween"));

    }

    public Tween FadeImageColor(Color fadeColor, float fadeInTime, Image image)
    {
        return image.DOColor(fadeColor, fadeInTime).SetId("answerTween");

    }

    public Tween FadeImageColor(Color fadeColor, float fadeInTime, TextMeshProUGUI text)
    {
        return text.DOColor(fadeColor, fadeInTime).SetId("answerTween");

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
