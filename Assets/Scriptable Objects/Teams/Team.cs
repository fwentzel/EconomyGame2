using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObj/Teams")]
public class Team : ScriptableObject
{
	public int teamID;
	public Color color;
	public string teamName=> teamID.ToString();

    public override string ToString()
    {
        return $"Team {teamID}";
    }
}
