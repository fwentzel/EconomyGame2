using UnityEngine;
using System.Collections;
using System;

public class ResourceUiManager : MonoBehaviour
{
    public static ResourceUiManager instance { get; private set; }
    public ResourceManager activeResourceMan { get => currentRessouceManagerToShow; set => SetRM(value); }

    [SerializeField] ResourceStartvalue res = null;
    [SerializeField] GameObject resourceUiPrefab = null;
    private Color defaultTextColor;

    ResourceManager currentRessouceManagerToShow;
    // Use this for initialization
    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        SetupResourceUi();
        defaultTextColor = res.startValues[0].uiText.color;
    }

    private void SetupResourceUi()
    {
        Transform resourceUiParent = transform.Find("CityResourcePanel");
        foreach (Resource resource in res.startValues)
        {
            GameObject obj = Instantiate(resourceUiPrefab, resourceUiParent);
            resource.Setup(obj);
        }

        //move timer object to last position
        resourceUiParent.transform.GetChild(0).SetAsLastSibling();
        

    }

    public void UpdateRessourceUI()
    {

        foreach (Resource res in res.startValues)
        {
            if (res.resource == resource.loyalty)
                res.uiText.color = activeResourceMan.isLoyaltyDecreasing ? Color.red : defaultTextColor;
            res.uiText.text = currentRessouceManagerToShow.GetAmountUI(res.resource);
        }
    }

    public void UpdateRessourceUI(resource resourceType)
    {

        Resource _res = res.startValues.Find(x => x.resource == resourceType);
        if (_res.resource == resource.loyalty)
            _res.uiText.color = activeResourceMan.isLoyaltyDecreasing ? Color.red : defaultTextColor;
        _res.uiText.text = currentRessouceManagerToShow.GetAmountUI(_res.resource);

    }

    void SetRM(ResourceManager rm)
    {
        if (currentRessouceManagerToShow != null)
            //Stop listening to old RM Event
            currentRessouceManagerToShow.OnResourceChange -= UpdateRessourceUI;

        //Set new Manager and start listening to new Event
        currentRessouceManagerToShow = rm;
        currentRessouceManagerToShow.OnResourceChange += UpdateRessourceUI;
    }

}
