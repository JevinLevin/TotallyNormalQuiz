using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "SequenceScriptableObject", menuName = "ScriptableObjects/Guitar")]
public class GUITARSequenceScriptableObject : ScriptableObject
{
    [ListDrawerSettings(ShowIndexLabels = true, DefaultExpandedState = true, NumberOfItemsPerPage = 50)]
    [SerializeField] private GUITARNote[] notes;

    public int GetLength()
    {
        return notes.Length;
    }

    public GUITARNote GetNote(int index)
    {
        return notes[index];
    }
}
