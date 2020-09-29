using UnityEngine;

public class Forest : MonoBehaviour
{

    public void OnBuild()
    {
         transform.RotateAround(transform.position,Vector3.up, Random.Range(0, 360));

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i).transform;
            t.RotateAround(t.position,Vector3.up, Random.Range(0, 360));

            t.localScale += new Vector3(0, 0,(t.localScale.y * .3f)* Random.Range(-1,1));
            MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();

            _propBlock.SetVector("Pos", new Vector4(t.position.x + Random.Range(-5, 5), t.position.z + Random.Range(-5, 5), 0, 0));
            // Apply the edited values to the renderer.
            t.GetComponent<Renderer>().SetPropertyBlock(_propBlock);
        }
    }
}
