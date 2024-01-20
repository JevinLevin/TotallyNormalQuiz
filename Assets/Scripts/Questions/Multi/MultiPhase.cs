using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MultiPhaseScriptableObject", menuName = "ScriptableObjects/Multi")]
public class MultiPhase : ScriptableObject
{
    public List<MultiAnswer> answers;
}
