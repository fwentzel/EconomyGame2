using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCD : MonoBehaviour
{
    // Start is called before the first frame update
   float cd=5;

   float t=0;
   float t0 =0;
   float amount=0;

   Button button;
   //Hacky solution to keep Tradeelemt non-interactable after cd
   TradeElement tradeElement = null;
   TradeVehicle vehicle;
   private void Awake() {
        //in start so it can get cached in TradeElement Start method
         button = GetComponent<Button>();
         tradeElement=transform.parent.GetComponent<TradeElement>();
        
   }
    public void SetUp(float cd , TradeVehicle vehicle = null){
        this.cd=cd;
        this.vehicle=vehicle;
        button.interactable=false;
        t0=Time.time;
        amount=0;
    }

   private void Update()
	{
        button.interactable=false;//hacky solution since Resource change triggers Updatecontext where only check against resourceamount
        t=Time.time-t0;
        amount =  t/cd;
        button.image.fillAmount = amount;
 
        if (amount >= 1 )
        {
            button.image.fillAmount = 1;
            button.interactable=true;

            //Hacky solution to keep Tradeelemt non-interactable after cd
            if(tradeElement!=null){
                tradeElement.checkInteractable();
            }
            if (vehicle!=null)
            {
                ContextUiManager.instance.UpdateContextUi(vehicle);
            }
            
            enabled=false;
        }
		
	}
}
