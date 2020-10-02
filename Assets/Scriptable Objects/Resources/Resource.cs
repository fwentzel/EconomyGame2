using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "ScriptableObj/Resource")]
public class Resource : ScriptableObject
{
	public resource resource;
	public int defaultStartAmount;
	public Sprite sprite;
	public TMP_Text uiDisplay;
	
	public void SearchUiDisplay()
	{
		foreach (Transform child in GameObject.Find("CityResourcePanel").transform)
		{
			//First child hold icon
			if (child.childCount > 1)//workaround for Gametimer
			{
				if (child.GetChild(0).GetComponent<Image>().mainTexture.name.Equals(sprite.name))
				{
					//second child holds Text Component
					uiDisplay = child.GetChild(1).GetComponent<TMP_Text>();
				}
			}
		}
	}
}