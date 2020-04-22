
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public GameObject settingsUI;

	public void OpenSettings()
	{
		settingsUI.SetActive( true);
	}

	public void ExitApplication()
	{
		print("QUIT!");
		Application.Quit();
	}
}
