using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "MultiPhaseScriptableObject", menuName = "ScriptableObjects/Multi")]
public class MultiPhaseScriptableObject : ScriptableObject
{

        [ListDrawerSettings(ShowIndexLabels = true)]
        public List<MultiAnswer> answers;

        public void SetupIndex()
        {
                for(int i = 0; i < answers.Count; i++)
                {
                        answers[i].originalIndex = i;
                }
        }


}
