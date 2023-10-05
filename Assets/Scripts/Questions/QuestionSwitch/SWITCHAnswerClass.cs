using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[Serializable]
public class SWITCHAnswerClass
{
    public bool correct;
    public string text;
    public SWITCHAnswerClass(bool correct, string text)
    {
        this.correct = correct;
        this.text = text;
    }
}
