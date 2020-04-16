using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class GameTimer : NetworkBehaviour
{
	Image timerImage;
	
	public float fillAmount = 0;
	[SyncVar]
	public float dayLength = 2.0f;
	[SyncVar]
	public bool isRunning = false;
	Vector3 pos;
	private void Awake()
	{
		pos = transform.position;
		timerImage = GetComponent<Image>();
	}

	[Server]
	public void StartTimer(float dayLength)
	{
		this.dayLength = dayLength;
		isRunning = true;
	}
	
	void Update()
	{
		//TODO verhindern, dass Position überschrieben wird.
		transform.position = pos;
		if (isRunning)
		{//Reduce fill amount 
			fillAmount += 1.0f / dayLength * Time.deltaTime;
			if (fillAmount >= 1)
				fillAmount = 0;
		}
		timerImage.fillAmount = fillAmount;
		
	}
}
