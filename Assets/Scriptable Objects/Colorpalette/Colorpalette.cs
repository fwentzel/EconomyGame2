using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/Colorpalette")]
public class Colorpalette : ScriptableObject
{
    public Color[] colors;
    public int[] levelMapping;

    private void OnValidate() {
        ColorPaletteForImage.RefreshAll();
    }
    
}
