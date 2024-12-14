using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float rate = 5.0f;
    public bool rotate = true;
    GameObject room;

    InputAction moveAction;
    InputAction clickAction;
    InputAction tiltAction;
    InputAction zoomAction;
    InputAction escAction;

    public GameObject menu;

    private bool menuOpen = false;
    public float tiltFactor = 0;

    public Camera camera;
    //private double zoomFactor = 3.5f;

    bool ignoreTiltInput = false;


    void OnEnable()
    {
        //moveAction.Enable(); // Input Action aktivieren
        //clickAction.Enable();
    }

    void OnDisable()
    {
        //moveAction.Disable(); // Input Action deaktivieren
        //clickAction.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        room = gameObject;
        moveAction = InputSystem.actions.FindAction("Move");
        clickAction = InputSystem.actions.FindAction("Click");
        tiltAction = InputSystem.actions.FindAction("Tilt");
        zoomAction = InputSystem.actions.FindAction("Zoom");
        escAction = InputSystem.actions.FindAction("Open Menu");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moved = moveAction.ReadValue<Vector2>();
        float clicked = clickAction.ReadValue<float>();
        //float tiltClicked = tiltAction.ReadValue<float>();
        float zoom = zoomAction.ReadValue<Vector2>().y;
        bool reading = escAction.ReadValue<float>() > 0;

        if (clicked > 0 && !menuOpen)
        {
            room.transform.eulerAngles = new Vector3(
                room.transform.eulerAngles.x,
                room.transform.eulerAngles.y - moved.x,
                room.transform.eulerAngles.z
            );
            tiltFactor = Math.Clamp(tiltFactor + moved.y, -35, 5);
        }
        
        gameObject.transform.position = new Vector3(
            gameObject.transform.position.x,
            tiltFactor,
            gameObject.transform.position.z
        );
        //= tiltFactor;

        if (zoom != 0)
        {
            // TODO
            //zoomFactor = Math.Clamp(zoom + zoomFactor, 1.0, 128.0);
            //Debug.Log(zoomFactor);

            //camera.orthographicSize += (zoom / 10);
            //camera.orthographicSize %= 35;
        }

        if (escAction.triggered) {
            menuOpen = !menuOpen;
        }
        menu.SetActive(menuOpen);
    }

    public void Quit() {
        Application.Quit();
    }
}
