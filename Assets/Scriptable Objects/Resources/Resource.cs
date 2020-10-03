using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[CreateAssetMenu(menuName = "ScriptableObj/Resource")][Serializable]
public class Resource : ScriptableObject
{
    public resource resource;
    public int defaultStartAmount;
    public Sprite sprite;
    public TMP_Text uiText;

    public void Setup(GameObject obj)
    {
        //second child holds Text Component
        uiText = obj.GetComponentInChildren<TMP_Text>();
        obj.transform.GetChild(0).GetComponent<Image>().sprite = sprite;

    }
	
}