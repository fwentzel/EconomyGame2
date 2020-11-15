using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Harbour : Building
{
    protected static new Dictionary<Team, List<Vector2>> possibleDefaultPlacements;
    public override void OnBuild(bool subtractResource = true)
    {
        if (PlacementController.instance == null) return;//Only occurs during mapgeneration
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.up) < 0)
        {
            transform.RotateAround(transform.position, Vector3.up, 90);
        }
        else if (PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.down) < 0)
        {
            transform.RotateAround(transform.position, Vector3.up, -90);
        }
        else if (PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.right) < 0)
        {
            transform.RotateAround(transform.position, Vector3.up, 180);
        }

        base.OnBuild();

    }
    private void Awake()
    {
        UseMaxPlacementRange = false;
        //not initialized yet
        if (possibleDefaultPlacements == null)
            possibleDefaultPlacements = new Dictionary<Team, List<Vector2>>();

        if (!possibleDefaultPlacements.ContainsKey(team))
        {
            possibleDefaultPlacements[team] = new List<Vector2>();
            if (GameManager.instance.dayIndex == 0)//Game not started
            {
                GameManager.instance.OnGameStart += SetupPossiblePlacements;
            }
            else
            {
                //Game running, callback wont be called
                SetupPossiblePlacements();
            }

        }
    }

    //TODO bei anderen Placements nicht komplett Start anpassen
    private void Start()
    {

    }

    public static new List<Vector2> GetPossibleBuildSpots(Team t)
    {
        return possibleDefaultPlacements[t];
    }

    protected override void SetupPossiblePlacements()
    {
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        for (int x = 0; x < mapGenerator.xSize; x++)
        {
            for (int z = 0; z < mapGenerator.zSize; z++)
            {
                Vector2 pos = new Vector2(x, z);
                if (PlacementController.instance.CheckSurroundingTiles(pos, 0, h => h < 0))
                {
                    possibleDefaultPlacements[team].Add(new Vector2(x, z));
                }
            }
        }
    }

    //TODO
    public override void CheckCanBuild(Collider other, bool onEnter)
    {

        //only distance check
        if (other == null)
        {
            //TODO HACKYYYYYYY
            if (!possibleDefaultPlacements.ContainsKey(team))
            {
                possibleDefaultPlacements[team] = new List<Vector2>();
                SetupPossiblePlacements();
            }
            PlacementController.instance.SetCanBuild(possibleDefaultPlacements[team].Contains(new Vector2(transform.position.x, transform.position.z)));
            return;
        }

        //entered a collieder, so disable build
        if (onEnter)
        {
            PlacementController.instance.SetCanBuild(false);
        }
        else
        {
            //TODO
            PlacementController.instance.SetCanBuild(true);
        }
    }




}
