using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public Team team;
    public bool occupied=false;

    public virtual void OnBuild(){}
}
