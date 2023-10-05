using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "CountdownPhaseScriptableObject", menuName = "ScriptableObjects/Countdown")]
public class COUNTDOWNPhaseScriptableObject : ScriptableObject
{


        [ListDrawerSettings(ShowIndexLabels = true)]
        public List<COUNTDOWNAnswer> answers;


}
