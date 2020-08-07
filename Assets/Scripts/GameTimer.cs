using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
	Image timerImage;
	
	public float fillAmount = 0;
	public float dayLength = 2.0f;
	public bool isRunning = false;
	private void Awake()
	{
		timerImage = GetComponent<Image>();
	}
	
	public void StartTimer(float dayLength)
	{
		this.dayLength = dayLength;
		isRunning = true;
	}
	
	void Update()
	{
		//TODO verhindern, dass Position überschrieben wird.
		if (isRunning)
		{//Reduce fill amount 
			fillAmount += 1.0f / dayLength * Time.deltaTime;
			if (fillAmount >= 1)
				fillAmount = 0;
		}
		timerImage.fillAmount = fillAmount;
		
	}
}
