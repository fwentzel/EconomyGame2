using UnityEngine;
using UnityEngine.UI;

public class GameTimerDisplay : MonoBehaviour
{
	Image timer;
	float dayLength = 2.0f;

	// Update is called once per frame
	private void Start()
	{
		timer = GetComponent<Image>();
		dayLength = GameManager.instance.calcResourceIntervall;
	}
	void Update()
	{
		//Reduce fill amount 
		timer.fillAmount += 1.0f / dayLength * Time.deltaTime;
		if (timer.fillAmount >= 1)
			timer.fillAmount = 0;
	}
}
