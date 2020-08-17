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
        teamText = transform.Find("TeamText").GetComponentInChildren<TMP_Text>();
        moneyText = transform.Find("MoneyText").GetComponentInChildren<TMP_Text>();
        loyaltyText = transform.Find("LoyaltyText").GetComponentInChildren<TMP_Text>();
        citizensText = transform.Find("CitizenText").GetComponentInChildren<TMP_Text>();
        foodText = transform.Find("FoodText").GetComponentInChildren<TMP_Text>();
        stoneText = transform.Find("StoneText").GetComponentInChildren<TMP_Text>();
    }

    private void OnEnable()
    {
        if (rem == null)
        {
            setResourceManager();
        }
    }
    private void setResourceManager()
    {
        if (CityResourceLookup.instance == null)
            return;
        Transform parent = transform.parent;
        ResourceManager[] resourceManagers = CityResourceLookup.instance.resourceManagers;
        if (resourceManagers.Length > parent.childCount - 1)
        {//-1 to account for header
            Instantiate(this, parent);
        }
        bool found = false;
        for (int i = 0; i < resourceManagers.Length; i++)
        {
            //+1 to account for Header Child
            if (parent.GetChild(i + 1) == this.transform)
            {
                rem = resourceManagers[i];
                float newVal = i % 2 == 0 ? 125 : 150;
                //convert to colorspace 0-1
                newVal /= 255;
                Color color= new Color(newVal, newVal, newVal, 255);
                foreach (Image image in GetComponentsInChildren<Image>())
                {
                    image.color=color;
                }
                if (GameManager.instance.localPlayer.mainbuilding == rem.mainbuilding)
                {
                    //Found local player

                }
                break;
            }
        }
        GameManager.instance.OnCalculateIntervall += UpdateScore;
        UpdateScore();
    }

    public void UpdateScore()
    {
        moneyText.text = rem.GetAmount(resource.money).ToString();
        loyaltyText.text = rem.GetAmount(resource.loyalty).ToString();
        citizensText.text = rem.GetAmount(resource.citizens).ToString();
        foodText.text = rem.GetAmount(resource.food).ToString();
        stoneText.text = rem.GetAmount(resource.stone).ToString();
        teamText.text = rem.mainbuilding.team.ToString();
    }

}
