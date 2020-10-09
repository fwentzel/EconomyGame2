using UnityEngine;

public class BuildUi : MonoBehaviour
{
	public static BuildUi instance { get; private set; }

	[SerializeField] GameObject elementPrefab = null;
	[SerializeField] GameObject[] prefabs = null;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}
	private void Start() {
		GameManager.instance.OnGameStart += GenerateBuildMenu;
	}
	public void GenerateBuildMenu()
	{
		Transform parent = transform.Find("BuildPanel");
		for (int i = 0; i < prefabs.Length; i++)
		{
			GameObject obj=Instantiate(elementPrefab, parent);
			obj.GetComponent<BuildUiElement>().Init(prefabs[i]);
		}
	}

}
