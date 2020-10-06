using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harbour : Building
{
    public override void OnBuild(bool subtractResource = true)
    {
        if(PlacementController.instance==null) return;//Only occurs during mapgeneration
        Vector2 pos = new Vector2(transform.position.x, transform.position.z);
        transform.rotation=Quaternion.Euler(0,0,0);
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
    public override void CheckCanBuild(Collider other, bool onEnter)
    {
        //only distance check
        if (other == null)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.z);
            float objHeight = PlacementController.instance.GetMeanHeightSurrounding(pos);
            if (objHeight == 0)//is on ground/coast
            {
                //check if there is water tile surrounding
                if (PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.up) < 0 ||
                    PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.down) < 0 ||
                    PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.left) < 0 ||
                    PlacementController.instance.GetMeanHeightSurrounding(pos + Vector2.right) < 0)
                {
                    PlacementController.instance.SetCanBuild(true);
                    return;
                }

            }
            PlacementController.instance.SetCanBuild(false);
            return;
        }
        if (other.CompareTag("Ground")) return;

        //entered a collieder, so disable build
        if (onEnter)
        {
            PlacementController.instance.SetCanBuild(false);
        }
        else
        {

            PlacementController.instance.SetCanBuild(true);
        }
    }
}
