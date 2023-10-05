using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
    public class SEQUENCEEntry
    {
        
        [Title("Answer Number"),HideLabel,PropertyRange(1,9)]
        public int answerNumber = 4;
        [Title("Delay"),HideLabel,PropertyRange(0, 0.5f)]
        public float delay;
    }
