using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public GameObject settingsUI;
	public void OpenLevel()
	{
		SceneManager.LoadScene("Level");
	}

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
