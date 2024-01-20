using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;

public class GUITARManager : MonoBehaviour
{

    public ObjectPool<GUITARInput> inputPool;

    [Header("Components")] 
    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private GameObject inputObject;
    [SerializeField] private GUITARSequenceScriptableObject sequence;
    [SerializeField] private GUITARAnswer[] answers;
    [SerializeField] private GUITARLives livesScript;
    [Header("Attributes")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private int lives;

    private int currentLives;
    private bool active;
    private int totalInputs;

    void Awake()
    {
        livesScript.Setup(lives);
    }
    
    void Start()
    {
        inputPool = new ObjectPool<GUITARInput>(
            () => Instantiate(inputObject).GetComponent<GUITARInput>(), 
            input => { input.gameObject.SetActive(true); }, 
            input => { input.gameObject.SetActive(false); },
            input =>
            {
                print("cum"); Destroy(input.gameObject); },
            false,
            5
        );
    }

    private void OnEnable()
    {
        questionScript.OnReset += OnReset;
        questionScript.OnStart += OnStart;
    }


    public void OnReset()
    {        
        totalInputs = sequence.GetLength();
        currentLives = lives;
        livesScript.ResetLives();
        ReturnToPool();
    }
    
    public void OnStart()
    {
        active = true;
        
        StartCoroutine(nameof(PlaySequence));
    }

    // Stores all active inputs from answers into list and returns them to poll
    private void ReturnToPool()
    {
        List<GUITARInput> inputs = new();
        foreach (GUITARAnswer answer in answers)
        {
            inputs.AddRange(answer.GetInputs());
            
            // Clears the answers input list too
            answer.ClearInputs();
        }
        foreach (GUITARInput input in inputs)
        {
            inputPool.Release(input);
        }
    }

    // Loops through every entry in the sequence, waits, then spawns it
    public IEnumerator PlaySequence()
    {
        int inputCount = 0;

        while (inputCount < sequence.GetLength())
        {
            // Get current note info
            GUITARNote currentNote = sequence.GetNote(inputCount);
            
            // Delay the spawning of the next note
            yield return new WaitForSeconds(currentNote.delay);
            
            // Grabs input and assigns it to the correct answer
            GUITARInput newInput = inputPool.Get();
            answers[currentNote.buttonNumber-1].SetInput(newInput, this);

            inputCount++;
        }
    }


    // If an input is missed of mistimed
    public void FailInput()
    {

        // Checks if that was the final life
        currentLives--;
        if (currentLives == 0)
        {
            Fail();
        }
        
        // Visuals
        livesScript.LoseLife(currentLives);
    }

    // If the player runs out of lives
    private void Fail()
    {
        // Stops inputs from moving
        active = false;
        
        // Restart question
        questionScript.GenericAnswerWrong();
        
        // Stop spawning inputs
        StopCoroutine(nameof(PlaySequence));
    }

    // Check if all inputs are gone
    public void CheckWin()
    {
        if (totalInputs <= 0)
        {
            Win();
        }
    }

    // If all inputs are gone and at least 1 life remains
    private void Win()
    {
        livesScript.Win();
        
        // Next question
        questionScript.GenericAnswerCorrect();
    }
    
    // Removes input from object pool
    public void RemoveInput(GUITARInput input)
    {
        
        inputPool.Release(input);
        --totalInputs;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public bool IsActive()
    {
        return active;
    }

    
}
