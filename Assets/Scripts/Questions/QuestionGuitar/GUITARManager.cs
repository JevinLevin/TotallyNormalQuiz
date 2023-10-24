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
        totalInputs = sequence.GetLength();
    }
    
    void Start()
    {
        inputPool = new ObjectPool<GUITARInput>(() =>
            { // If no available inputs
            return Instantiate(inputObject).GetComponent<GUITARInput>();
            }, // If available input
            input => {
            input.gameObject.SetActive(true);
            }, // If removing input
            input => {
            input.gameObject.SetActive(false); 
            },
            input => { 
                Destroy(input.gameObject); 
            },
            false,
            5
        );
    }
    

    public GUITARInput SpawnInput()
    {
        var input = inputPool.Get();
        input.SetManager(this);
        return input;
    }

    public void OnReset()
    {        
        currentLives = lives;
        livesScript.ResetLives();
        foreach (GUITARAnswer answer in answers)
        {
            answer.ClearInputs();
        }
    }
    
    public void OnStart()
    {
        StartCoroutine(nameof(PlaySequence));
        active = true;
    }

    public IEnumerator PlaySequence()
    {
        int inputCount = 0;

        while (inputCount < sequence.GetLength())
        {
            // Get current note info
            GUITARNote currentNote = sequence.GetNote(inputCount);
            
            // Delay the spawning of the next note
            yield return new WaitForSeconds(currentNote.delay);
            
            // Spawn the input object from the object pool
            GUITARInput newInput = SpawnInput();
            
            // Set the input to the correct answer
            answers[currentNote.buttonNumber-1].SetInput(newInput);

            inputCount++;
        }
    }



    public void FailInput()
    {
        livesScript.LoseLife(currentLives);
        currentLives--;
        if (currentLives == 0)
        {
            Fail();
        }
    }

    private void Fail()
    {
        active = false;
        
        questionScript.GenericAnswerWrong();
        
        StopCoroutine(nameof(PlaySequence));
    }

    public void CheckWin()
    {
        if (totalInputs <= 0)
        {
            Win();
        }
    }

    private void Win()
    {
        livesScript.Win();
        
        questionScript.GenericAnswerCorrect();
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public int GetLives()
    {
        return lives;
    }

    public bool IsActive()
    {
        return active;
    }

    public void RemoveInput()
    {
        --totalInputs;
    }
    
}
