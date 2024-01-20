using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class REACTIONQuestion : MonoBehaviour
{
    [SerializeField] private QuestionGeneric questionScript;
    private REACTIONAnswer[] answers;
    
    [SerializeField] private Vector2 flashDelays;
    [SerializeField] private Vector2 flashLengths = new Vector2(1.25f,2f);
    [SerializeField] private float flashSpeedOffset = 0.1f;
    [FormerlySerializedAs("flashDelayCurve")] [SerializeField] private AnimationCurve flashSpeedCurve;
    [SerializeField] private int questionLength;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        answers = GetComponentsInChildren<REACTIONAnswer>();
    }

    private void OnEnable()
    {
        questionScript.OnStart += StartQuestion;
    }

    public void StartQuestion()
    {
        currentCoroutine = StartCoroutine(PlayQuestion());
    }

    private IEnumerator PlayQuestion()
    {
        float duration = 0.0f;
        while (duration < questionLength)
        {
            float t = duration / questionLength;
            float randomT = Random.Range(t - flashSpeedOffset, t + flashSpeedOffset);
            float delay = Mathf.Lerp(flashDelays.x, flashDelays.y, flashSpeedCurve.Evaluate(randomT));
            yield return new WaitForSeconds(delay);

            // Choose activate a random answer if there is any
            if(answers.Any(answer => !answer.Activated))
                GetRandomInactiveAnswer().ActivateAnswer(Mathf.Lerp(flashLengths.y,flashLengths.x,flashSpeedCurve.Evaluate(randomT)), OnFail);

            duration += Time.deltaTime + delay;
        }

        // Wait until all answers and deactivated
        while (answers.Any(answer => answer.Activated && !answer.Delayed))
            yield return new WaitForSeconds(0.1f);
        
        // If they complete them all
        questionScript.GenericAnswerCorrect(fadeAnswers:true);
    }

    private REACTIONAnswer GetRandomInactiveAnswer()
    {
        REACTIONAnswer currentAnswer;
        int count = 0;
        do
        {
            count++;
            currentAnswer = answers[Random.Range(0, answers.Length)];
        } while (currentAnswer.Activated && count < answers.Length); 

        return currentAnswer;
    }

    private void OnFail()
    {
        StopCoroutine(currentCoroutine);
        
        // Stop all answers current fading
        foreach (REACTIONAnswer answer in answers)
            answer.Stop();
        
        questionScript.GenericAnswerWrong(fadeAnswers:true);
    }
}
