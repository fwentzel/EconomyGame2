using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSpotsManager : MonoBehaviour
{

    public static PlacementSpotsManager instance { get; private set; }
    public static Dictionary<Type, List<BuildingPlacementInfo>> spots = new Dictionary<Type, List<BuildingPlacementInfo>>();

    public ColorToObjectMapping com;
    private void Start()
    {
      GameManager.instance.OnGameStart+= GetPlacements;
    }

    void GetPlacements()
    {       
        foreach (var item in com.colorObjectMappings)
        {
            Building building = item.placeable.GetComponent<Building>();
            if(building==null)continue;
            foreach (Player player in GameManager.instance.players)
            {
                building.GetPossibleBuildSpots(player.team);
            }
        }
    }
}

public class BuildingPlacementInfo
{
    public Team team;
    public List<Vector2> possibleSpots;


    public BuildingPlacementInfo(Team team, List<Vector2> possibleSpots)
    {
        this.team = team;
        this.possibleSpots = possibleSpots;
    }
}
