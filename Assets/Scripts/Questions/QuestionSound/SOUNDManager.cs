using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOUNDManager : MonoBehaviour
{
    [SerializeField] private QuestionGeneric questionScript;
    [SerializeField] private AudioSource audioPlayer;

    public void PlaySound()
    {
        audioPlayer.PlayOneShot(audioPlayer.clip);
    }

    public void ClickAnswer(AnswerGeneric answer)
    {
        questionScript.ClickAnswerGeneric(answer, GameManager.volume == 0);
    }
}
