using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCD : MonoBehaviour
{
    public event Action OnCDFinished = delegate { };

    // Start is called before the first frame update
    float cd = 5;

    float t = 0;
    float t0 = 0;
    float amount = 0;

    Button button;

    Image mask;
    private void Awake()
    {
        //in start so it can get cached in TradeElement Start method
        button = GetComponent<Button>();
        mask = transform.GetChild(0).GetComponent<Image>();
        mask.fillAmount = 0;
        enabled=false;
    }
    public void SetUp(float cd)
    {
        this.cd = cd;
        button.interactable = false;
        t0 = Time.time;
        amount = 1;
        enabled=true;
    }

    private void Update()
    {
        button.interactable = false;//hacky solution since Resource change triggers Updatecontext where only check against resourceamount
        t = Time.time - t0;
        amount = 1 - t / cd;
        mask.fillAmount = amount;

        if (amount <= 0)
        {
            mask.fillAmount = 0;
            button.interactable = true;
            OnCDFinished?.Invoke();
            enabled = false;
        }
    }
}
