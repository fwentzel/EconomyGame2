using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamScoreElement : MonoBehaviour
{
    TMP_Text teamText;
    TMP_Text moneyText;
    TMP_Text loyaltyText;
    TMP_Text citizensText;
    TMP_Text foodText;
    TMP_Text stoneText;

    ResourceManager rem;

    private void Awake()
    {
        teamText = transform.Find("TeamText").GetComponent<TMP_Text>();
        moneyText = transform.Find("MoneyText").GetComponent<TMP_Text>();
        loyaltyText = transform.Find("LoyaltyText").GetComponent<TMP_Text>();
        citizensText = transform.Find("CitizenText").GetComponent<TMP_Text>();
        foodText = transform.Find("FoodText").GetComponent<TMP_Text>();
        stoneText = transform.Find("StoneText").GetComponent<TMP_Text>();
    }

    private void Start()
    {
        GameManager.instance.OnGameStart += setResourceManager;
        GameManager.instance.OnCalculateIntervall += UpdateScore;
    }

    private void setResourceManager()
    {
        print("set");
        Transform parent = transform.parent;
        for (int i = 0; i < CityResourceLookup.instance.resourceManagers.Length; i++)
        {
            //+1 to account for Header Child
            if (parent.GetChild(i + 1) == this.transform)
            {
                rem = CityResourceLookup.instance.resourceManagers[i];
                if (GameManager.instance.localPlayer.mainbuilding == rem.mainbuilding)
                {
                    //Found local player
                    GetComponent<Image>().color=new Color(255,255,255,255);
                }
            }
        }

        if (rem == null)
        {
            //not needed since Playernumber < #TeamscoreElemts
            GameManager.instance.OnGameStart -= setResourceManager;
            GameManager.instance.OnCalculateIntervall -= UpdateScore;
            Destroy(gameObject);
        }
    }

    public void UpdateScore()
    {
        if(rem==null)
            return;
        moneyText.text = rem.GetAmount(resource.money).ToString();
        loyaltyText.text = rem.GetAmount(resource.loyalty).ToString();
        citizensText.text = rem.GetAmount(resource.citizens).ToString();
        foodText.text = rem.GetAmount(resource.food).ToString();
        stoneText.text = rem.GetAmount(resource.stone).ToString();
        teamText.text = rem.mainbuilding.team.ToString();
    }

}
