using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class ColorPaletteForImage : MonoBehaviour
{
    [SerializeField] string paletteName = "UI";
    Colorpalette palette;
    [Range(0, 19)] [SerializeField] int colorlevel = 0;
    Image image;

    void OnValidate()
    {
        if (image == null || palette == null)
            Reset();
        if (colorlevel > palette.colors.Length - 1)
            colorlevel = 0;
        else if (colorlevel < 0)
            colorlevel = palette.colors.Length - 1;

        Color newColor = palette.colors[colorlevel];
        newColor.a = image.color.a;
        image.color = newColor;
    }

    private void Reset()
    {

        image = GetComponent<Image>();
        GetPalette();
    }

    void GetPalette()
    {
        Colorpalette pal = AssetDatabase.LoadAssetAtPath($"Assets/Scriptable Objects/Colorpalette/{paletteName}.asset", typeof(Colorpalette)) as Colorpalette;
        if (pal == null) return;
        palette = pal;
    }
}


