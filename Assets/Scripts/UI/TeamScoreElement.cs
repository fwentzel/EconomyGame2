using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamScoreElement : MonoBehaviour
{
    TMP_Text teamText;
    TMP_Text goldText;
    TMP_Text loyaltyText;
    TMP_Text citizensText;
    TMP_Text foodText;
    TMP_Text stoneText;

    ResourceManager rem;

    private void Awake()
    {
        teamText = transform.Find("TeamText").GetComponentInChildren<TMP_Text>();
        goldText = transform.Find("GoldText").GetComponentInChildren<TMP_Text>();
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
        if (CitysMeanResource.instance == null)
            return;
        Transform parent = transform.parent;
        ResourceManager[] resourceManagers = CitysMeanResource.instance.resourceManagers;

        bool isMeanElement = false;
        if (resourceManagers.Length > parent.childCount - 2)//-2 for Mean element and Header
        {//-1 to account for header
            Instantiate(this, parent);
        }
        else
        {
            isMeanElement = true;
            teamText.text = "Mean";
        }

        if (!isMeanElement)
        {
            for (int i = 0; i < resourceManagers.Length; i++)
            {
                //+1 to account for Header Child
                if (parent.GetChild(i + 1) == this.transform)
                {
                    rem = resourceManagers[i];
                    float newVal = i % 2 == 0 ? 125 : 150;
                    //convert to colorspace 0-1
                    newVal /= 255;
                    Color color = rem.mainbuilding.team.color;
                    foreach (Image image in GetComponentsInChildren<Image>())
                    {
                        
                        image.color =  color;
                    }
                    break;
                }
            }
            GameManager.instance.OnCalculateIntervall += UpdateScore;
            UpdateScore();
        }
        else
        {
            
            //convert to colorspace 0-1
           float newVal =50/ 255f;
            Color color = new Color(newVal, newVal, newVal, 255);
            foreach (Image image in GetComponentsInChildren<Image>())
            {
                image.color=color;
            }
            
            GameManager.instance.OnCalculateIntervall += UpdateMean;
            UpdateMean();
        }

    }

    void UpdateScore()
    {
        goldText.text = rem.GetAmount(resource.gold).ToString();
        loyaltyText.text = rem.GetAmount(resource.loyalty).ToString();
        citizensText.text = rem.GetAmount(resource.citizens).ToString();
        foodText.text = rem.GetAmount(resource.food).ToString();
        stoneText.text = rem.GetAmount(resource.stone).ToString();
        teamText.text = rem.mainbuilding.team.teamName;

        if (rem.mainbuilding.gameOver)
        {
            SetTeamGameOver();
            return;
        }
    }
    void UpdateMean()
    {
        goldText.text = CitysMeanResource.instance.resourseMeanDict[resource.gold].ToString();
        loyaltyText.text = CitysMeanResource.instance.resourseMeanDict[resource.loyalty].ToString();
        citizensText.text = CitysMeanResource.instance.resourseMeanDict[resource.citizens].ToString();
        foodText.text = CitysMeanResource.instance.resourseMeanDict[resource.food].ToString();
        stoneText.text = CitysMeanResource.instance.resourseMeanDict[resource.stone].ToString();
    }

    public void SetTeamGameOver()
    {
        GameManager.instance.OnCalculateIntervall -= UpdateScore;
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.color = Color.red;
        }
    }

}
