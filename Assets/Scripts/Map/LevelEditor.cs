using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class LevelEditor : MonoBehaviour
{
    Texture2D mapTexture;
    Texture2D tempMap;
    [SerializeField] RawImage image= default;
    Vector3 imagePos;
    [SerializeField] TMP_InputField mapNameInput = default;
    [SerializeField] TMP_InputField dimenionsInput = default;

    [SerializeField] Vector2Int dimensions = new Vector2Int(50, 50);
    Vector2Int imageDimensions;
    Inputmaster input;
    inputmodes inputmode = inputmodes.brush;

    Mouse mouse;
    Keyboard keyboard;
    [Range(1, 10)]
    [SerializeField] int brushSize = 3;

    [SerializeField] ColorToHeightMapping colorToHeightMapping = default;
    [SerializeField] ColorToObjectMapping colorToObjectMapping = default;

    [SerializeField] Color paintColor;

    string mapPath => "Assets/Textures/Maps/";
    private void Awake()
    {
        dimenionsInput.text = $"{dimensions.x},{dimensions.y}";
        string[] arr = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t:texture2D", new[] { "Assets/Textures/Maps" })[0]).Split('/');
        mapNameInput.text = arr[arr.Length - 1].Split('.')[0];
        SetDimensions();
        SetUpNewTexture();
        SetUpInput();
        SetUpImage();
        paintColor = Color.black;
        Debug.Log("<color=red>IMPORTANT:</color> Atleast one house per team is necessary!");
    }

    private void Update()
    {
        if (keyboard.digit1Key.wasPressedThisFrame) ChangeTeam(1);
        if (keyboard.digit2Key.wasPressedThisFrame) ChangeTeam(2);
        if (keyboard.digit3Key.wasPressedThisFrame) ChangeTeam(3);
        if (keyboard.digit4Key.wasPressedThisFrame) ChangeTeam(4);

    }

    private void SetUpImage()
    {
        RectTransform imageRect = image.GetComponent<RectTransform>();
        imagePos = imageRect.position;
        imageDimensions = new Vector2Int((int)imageRect.rect.width, (int)imageRect.rect.height);
        image.texture = mapTexture;
    }

    private void SetUpInput()
    {
        input = InputMasterManager.instance.inputMaster;
        input.UI.Click.started += _ => Paint();
        input.LevelEditor.Drag.started += _ => InvokeRepeating("Paint", 0, 0.01f);
        input.LevelEditor.Drag.canceled += _ => CancelInvoke();
        input.UI.Enable();
        input.LevelEditor.Enable();
        mouse = Mouse.current;
        keyboard = Keyboard.current;
    }

    private void SetUpNewTexture()
    {
        mapTexture = new Texture2D(dimensions.x, dimensions.y);
        mapTexture.filterMode = FilterMode.Point;
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                mapTexture.SetPixel(x, y, Color.black);

            }
        }
        mapTexture.Apply();
    }

    private void SetDimensions(Texture2D textureToCopy = null)
    {
        if (textureToCopy != null)
        {
            dimensions = new Vector2Int(textureToCopy.width, textureToCopy.height);
            return;
        }
        String[] arr = dimenionsInput.text.Split(',');
        if (arr.Length == 2)
        {
            dimensions = new Vector2Int(int.Parse(arr[0]), int.Parse(arr[1]));
        }
        else
        {
            print("wrong format, using previous valuies");
        }
    }

    public void ChangeBuilding(string name)
    {
        //green for Objects
        foreach (var item in colorToObjectMapping.colorObjectMappings)
        {
            if (item.name == name)
            {
                paintColor.g = item.value / 255f;
                paintColor.b = 0;
                inputmode = inputmodes.single;

                if (item.value < 100)//OBJECTS
                {
                    paintColor.r = 0;

                    inputmode = item.name == "Forest" ? inputmodes.brush : inputmodes.single;
                }
                else if (paintColor.r == 0)
                {
                    paintColor.r = 1/255f;//set defualt value for team
                }

                print("Painting: " + name);
                return;
            }
        }
        print("couldnt find " + name);
        paintColor.g = 0;
    }

    public void ChangeHeight(string name)
    {
        inputmode = inputmodes.height;
        //red for height
        if (name.Length == 0)
        {
            print("resetting to ground " + name);
            goto afterLoop;
        }
        foreach (var item in colorToHeightMapping.colorHeightMappings)
        {
            if (item.name == name)
            {
                paintColor.b = item.value / 255f;
                paintColor.g = 0;
                paintColor.r = 0;

                print("Painting: " + name + " with value: " + item.value);
                return;
            }
        }
    afterLoop:
        print("couldnt find " + name);
        paintColor.b = 0;
        paintColor.g = 0;
        paintColor.r = 0;

    }
    public void Save()
    {
        String path = GetMapPath();
        CopyTexture(mapTexture, out tempMap);
        if (!string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(path)))
        {
            print("overwriting " + path);
            AssetDatabase.DeleteAsset(path);
        }

        AssetDatabase.CreateAsset(tempMap, path);
    }
    public void Load()
    {
        String path = GetMapPath();

        Texture2D _mapTexture = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
        if (_mapTexture == null)
        {
            print("couldnt load. Didnt find " + path);
            New();
            return;
        }

        CopyTexture(_mapTexture, out mapTexture);
        dimensions = new Vector2Int(mapTexture.width, mapTexture.height);

        SetUpImage();
    }

    public void New()
    {
        String path = GetMapPath();
        Texture2D _mapTexture = AssetDatabase.LoadAssetAtPath(mapPath + "newMap.asset", typeof(Texture2D)) as Texture2D;
        if (_mapTexture != null)
        {
            print("map already exists. Overwriting newmap.asset!");
        }
        SetDimensions();
        SetUpNewTexture();
        SetUpImage();

        image.texture = mapTexture;

    }
    private void CopyTexture(Texture2D _mapTexture, out Texture2D outMapTexture)
    {
        SetDimensions(_mapTexture);
        Texture2D newMap = new Texture2D(dimensions.x, dimensions.y);
        newMap.filterMode = FilterMode.Point;

        Graphics.CopyTexture(_mapTexture, newMap);
        outMapTexture = newMap;
    }


    String GetMapPath()
    {
        return mapPath + mapNameInput.text + ".asset";
    }
    public void ChangeTeam(float _team)
    {
        int team = Mathf.RoundToInt(_team);
        print("changed to Team: " + team);
        paintColor.r = team / 255f;
    }
    public void ChangeBrushsize(float _size)
    {
        int size = Mathf.RoundToInt(_size);
        print("changed Brushsize to: " + size);
        brushSize = size;
    }


    private void Paint()
    {

        Vector2Int pos = PosInImage();
        if (pos.x < 0)
            return;
        int tmpSize = inputmode == inputmodes.single ? 1 : brushSize;
        int size = tmpSize / 2;
        if (size == 0)
        {
            mapTexture.SetPixel(pos.x, pos.y, paintColor);
            mapTexture.Apply();
        }

        for (int x = -size; x < size; x++)
        {
            for (int y = -size; y < size; y++)
            {
                if (pos.x + x >= 0 && pos.y + y >= 0 &&
                   pos.x + x < dimensions.x && pos.y + y < dimensions.y)
                {
                    mapTexture.SetPixel(pos.x + x, pos.y + y, paintColor);
                }
            }
        }
        mapTexture.Apply();

    }

    private Vector2Int PosInImage()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 inImagePos = new Vector2(mousePos.x - imagePos.x, mousePos.y - imagePos.y);

        if (inImagePos.x >= 0 && inImagePos.x < imageDimensions.x &&
            inImagePos.y >= 0 && inImagePos.y < imageDimensions.y)
        {
            // The mouse click is inside the image, so calculate a normalised value for the click.
            Vector2 normalizedPos = new Vector2(inImagePos.x / imageDimensions.x, inImagePos.y / imageDimensions.y);
            normalizedPos *= dimensions;
            Vector2Int normalizedIntPos = new Vector2Int(Mathf.FloorToInt(normalizedPos.x), Mathf.FloorToInt(normalizedPos.y));
            return normalizedIntPos;
        }
        return new Vector2Int(-10, -10);
    }
}

public enum inputmodes
{
    height,
    brush,
    single
}