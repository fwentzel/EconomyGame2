using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSpotsManager : MonoBehaviour
{

    public static PlacementSpotsManager instance { get; private set; }
    public static Dictionary<buildSpotType, Dictionary<Team,HashSet<Vector2>>> spotsForBuildingTypeAndTeam = new Dictionary<buildSpotType, Dictionary<Team, HashSet<Vector2>>>();

    public ColorToObjectMapping com;
    private void Start()
    {
      GameManager.instance.OnGameStart+= GetPlacements;
    }

    void GetPlacements()
    {       
        // foreach (var item in com.colorObjectMappings)
        // {
        //     Building building = item.placeable.GetComponent<Building>();
        //     if(building==null)continue;
        //     foreach (Player player in GameManager.instance.players)
        //     {
        //         building.GetPossibleBuildSpots(player.team);
        //     }
        // }
    }
}

public enum buildSpotType{
    harbour,
    mine,
    normal
}