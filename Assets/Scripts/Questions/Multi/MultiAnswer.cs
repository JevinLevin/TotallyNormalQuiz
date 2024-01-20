using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class MultiAnswer
{
    public string answer;
    public bool correct;

    public MultiAnswer(string answer, bool correct)
    {
        this.answer = answer;
        this.correct = correct;
    }
}
