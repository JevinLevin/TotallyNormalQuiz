using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class MultiAnswer
{
    public string answer;
    public bool correct;
    public int originalIndex;

    public MultiAnswer(string answer, bool correct, int originalIndex)
    {
        this.answer = answer;
        this.correct = correct;
        this.originalIndex = originalIndex;
    }
}
