using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class CameraController : MonoBehaviour
{
    //--//--// > BEFORE USING THE SCRIPT PLEASE READ THE MESSAGE BELOW < \\--\\--\\

    // Everything that do something is commented on the code, you can change anything as you like
    // Ex: You dont like the scroll up and down with mouse scroll, then just go to line 106 and comment that code
    // I am commenting it because i think that some of people use Unity for fun and dont know how to code

    // Pré-requirements -> A camera in the scene
    // Tutorial on How to Use ->

    // STEP 1 - Put this script in any gameobject on your scene
    // Nice job, you are done

    // If you are trying to do an Online RTS you can use something like "if(!LocalPlayer) Destroy(gameObject)"
    // Hope you like

    // Editor Properties
    public Camera TheCamera;
    public LayerMask GroundLayer;
    Vector3 mainbuildingPos = Vector2.zero;

    // Camera Properties
    public bool useDefaultSettings;
    [Range(8, 32)]
    public float cameraSpeed;
    [Range(.8f, 3.2f)]
    public float cameraBorder;

    // Minimun and maxium distance from the detected ground the Camera can be
   
     float cameraMinHeight=5;
    [Range(.8f, 50f)]
    public float cameraMaxHeight;

    // Map properties
    float mapXSize;
    float mapZSize;

    // Properties that shold not change to make sure the camera will work
    private float _savedCameraSpeed;

    //for smoother Scrolling
    public float scrollspeed;
    public float scrollSensitivity;
    private float desiredScrollposition;

    private RaycastHit _rayHit;
    private Vector2 _leftMouseInitial;
    private Vector2 _leftMouseFinal;
    private Vector3 mouseInitial;

    Inputmaster controls;
    Keyboard keyboard;
    Mouse mouse;
    private void Awake()
    {
        
        keyboard = Keyboard.current;
        mouse = Mouse.current;
    }
    void Start()
    {
        controls=InputMasterManager.instance.inputMaster;
        CheckSettings();
        TheCamera.transform.eulerAngles = new Vector3(TheCamera.transform.eulerAngles.x, 0, 0);
        //controls.Camera.Move.performed+= ctx => Move(ctx.ReadValue<Vector2>());
        controls.Camera.Height.performed += ctx => Height(ctx.ReadValue<float>());
        controls.Camera.Enable();
        SetCameraHeight();
       
    }

    private void SetCameraHeight()
    {
        ColorToHeight[] cth = FindObjectOfType<MapGenerator>().colorToHeightMapping.colorHeightMappings;
        for (int i = 0; i < cth.Length; i++)
        {
          if(cth[i].vertexHight>=1){
               cameraMinHeight = cth[i].vertexHight+2;
               return;
          }  
        }

    }

    private void Height(float v)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        v /= 120;
        desiredScrollposition = Mathf.Clamp(desiredScrollposition - v * scrollSensitivity, cameraMinHeight, cameraMaxHeight); ;


    }

    private void FixedUpdate()
    {
        Move();
    }


    void CheckSettings()
    {
        if (useDefaultSettings)
        {
            TheCamera = Camera.main;
            cameraSpeed = 20;
            cameraBorder = 2f;
            cameraMinHeight = 2f;
            cameraMaxHeight = 20f;
            mapXSize = float.MaxValue;
        }
        desiredScrollposition = TheCamera.transform.position.y;
        _savedCameraSpeed = cameraSpeed;
        MapGenerator gen = FindObjectOfType<MapGenerator>();
        mapXSize = gen.xSize;
        mapZSize = gen.zSize;

    }

    void Move()
    {
        if(!controls.Camera.enabled) return;
        Vector3 position = TheCamera.transform.position;
        Vector3 rotation = new Vector3(TheCamera.transform.eulerAngles.x, TheCamera.transform.eulerAngles.y, 0);

        // // W, A, S, D Movement
        if (keyboard.wKey.isPressed)
        {
            position += new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * (cameraSpeed * Time.deltaTime);
        }
        if (keyboard.sKey.isPressed)
        {
            position -= new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * (cameraSpeed * Time.deltaTime);
        }
        if (keyboard.aKey.isPressed)
        {
            position -= new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * (cameraSpeed * Time.deltaTime);
        }
        if (keyboard.dKey.isPressed)
        {
            position += new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * (cameraSpeed * Time.deltaTime);
        }

        // Q, E, Alt Rotation
        if (keyboard.qKey.isPressed)
            rotation.y -= cameraSpeed * (Time.deltaTime * 32);
        if (keyboard.eKey.isPressed)
            rotation.y += cameraSpeed * (Time.deltaTime * 32);


        //Border Touch Movement
        // Vector2 mousePos=mouse.position.ReadValue();
        // if (mousePos.y >= Screen.height - cameraBorder)
        //    position += new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * (cameraSpeed * Time.deltaTime);
        // if (mousePos.y <= 0 + cameraBorder)
        //    position -= new Vector3(TheCamera.transform.forward.x, 0, TheCamera.transform.forward.z) * (cameraSpeed * Time.deltaTime);
        // if (mousePos.x >= Screen.width - cameraBorder)
        //    position += new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * (cameraSpeed * Time.deltaTime);
        // if (mousePos.x <= 0 + cameraBorder)
        //    position -= new Vector3(TheCamera.transform.right.x, 0, TheCamera.transform.right.z) * (cameraSpeed * Time.deltaTime);


        // Mouse Rotation
        // if (Input.GetKeyDown(KeyCode.Mouse2))
        //     mouseInitial = Input.mousePosition;
        // if (Input.GetKey(KeyCode.Mouse2))
        // {
        //     if (mouseInitial.x - Input.mousePosition.x > 100 || mouseInitial.x - Input.mousePosition.x < -100)
        //         rotation.y -= (mouseInitial.x - Input.mousePosition.x) / 60;
        //     if (mouseInitial.y - Input.mousePosition.y > 100 || mouseInitial.y - Input.mousePosition.y < -100)
        //         position += transform.up * -(mouseInitial.y - Input.mousePosition.y) / 480;
        // }


        // Shift Acelleration
        if (keyboard.shiftKey.isPressed)
            cameraSpeed = (_savedCameraSpeed * 2f);
        else
            cameraSpeed = _savedCameraSpeed;

        position = new Vector3(position.x, Mathf.Lerp(position.y, desiredScrollposition, Time.deltaTime * scrollspeed), position.z);

        //// Camera Collision Check
        //Ray ray = new Ray(position, TheCamera.transform.forward);
        //Physics.Raycast(ray, out _rayHit, 32, GroundLayer);

        // Don't allow the camera to leave the ground area
        position.x = Mathf.Clamp(position.x, cameraBorder, mapXSize- cameraBorder);
        // position.y = Mathf.Clamp(position.y, cameraMinHeight, cameraMaxHeight);
        position.z = Mathf.Clamp(position.z, -position.y/2, mapZSize - cameraBorder-position.y/2);

        //// Effects when camera hit the ground or the top surface
        //if (position.y <= _rayHit.point.y + cameraMinHeight + 1)
        //	rotation.x = 20;
        //else if (position.y >= _rayHit.point.y + cameraMaxHeight - 1)
        //	rotation.x = 70;

        // Save Changes
        TheCamera.transform.position = Vector3.Slerp(TheCamera.transform.position, position, .8f);
        if (keyboard.spaceKey.isPressed)
        {
            rotation.y = 0;
            TheCamera.transform.eulerAngles = new Vector3(TheCamera.transform.eulerAngles.x, 0, 0);
            if(mainbuildingPos!=Vector3.zero)
                MoveCamOverObjectAt(mainbuildingPos);
        }
        //TheCamera.transform.eulerAngles = Vector3.Slerp(TheCamera.transform.eulerAngles, rotation, .2f);
    }



    public void MoveCamOverObjectAt(Vector3 objectPos)
    {

        float y = TheCamera.transform.position.y;
        float rotation = TheCamera.transform.rotation.eulerAngles.x * Mathf.Deg2Rad;
        float offset = (y / Mathf.Tan(rotation));
        this.mainbuildingPos = objectPos;
        float newZ = objectPos.z - offset;
        TheCamera.transform.position = new Vector3(objectPos.x, y, newZ);

    }
}
