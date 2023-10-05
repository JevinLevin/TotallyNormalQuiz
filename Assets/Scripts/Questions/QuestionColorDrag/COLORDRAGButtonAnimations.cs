using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COLORDRAGButtonAnimations : ButtonAnimations
{
    
    public override void Start()
    {
        base.Start();

        hoverButtonColor = new Color(buttonColor.r - 0.2f, buttonColor.g - 0.2f, buttonColor.b - 0.2f, buttonColor.a);  
    }

}
