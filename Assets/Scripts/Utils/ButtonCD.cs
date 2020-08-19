using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCD : MonoBehaviour
{
    // Start is called before the first frame update
   float cd=5;

   float t=0;
   float amount=0;

   Button button => GetComponent<Button>();

    public void SetUp(float cd){
        this.cd=cd;
        this.button.interactable=false;
        t=0;
        amount=0;
    }

   private void Update()
	{
        t+=Time.deltaTime;
        amount =  t/cd;
        button.image.fillAmount = amount;

        if (amount >= 1)
        {
            button.image.fillAmount = 1;
            button.interactable=true;
            enabled=false;
        }
		
	}
}
