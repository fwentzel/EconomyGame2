
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{

	//Custom inspector with button to generate map in edit mode
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		MapGenerator myTarget = (MapGenerator)target;		

		//Display Button to activate the Mapgeneration in the Mapgenerator Script
		if (GUILayout.Button("Generate Map"))
		{
			myTarget.GenerateMap();
		}
	}

}
#endif
