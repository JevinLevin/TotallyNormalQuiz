using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector; 

[CreateAssetMenu(fileName = "SequenceScriptableObject", menuName = "ScriptableObjects/Sequence")]
public class SequenceScriptableObject : ScriptableObject
{

    [ListDrawerSettings(ShowIndexLabels = true)]
    public SEQUENCEEntry[] sequence;


}
