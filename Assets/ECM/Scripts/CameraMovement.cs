using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {

    private Vector3 up;
    private Vector3 side;
    private float currentZoom;
    public enum CameraMode {perspective, orthographic};
    public CameraMode cameraMode = CameraMode.orthographic;
    public float zoomMin = 5f;
    public float zoomMax = 15f;
    public float speed = 5f;
    public float rotateSpeed = 50f;
    public float mouseSensitivity = 5f;
    public float zoomSpeed = 1f;
    private bool follow = true;
    public bool center = false;
    public GameObject followToggle;
    // The gamer can change the speed of all the different axis
    Camera cameraComponent;
    private GameObject selectionManager;
    private Character selectedCharacter;
    public Vector3 offset = new Vector3 (40.8f, 0, -16.8f);
    public GameObject cameraParent;
    public GameObject camFirstObject;

    private float onPause;

	// Use this for initialization
	void Start () {
        cameraParent = this.transform.parent.gameObject;
        camFirstObject = cameraParent.transform.parent.gameObject;
        cameraComponent = GetComponent<Camera>();
        currentZoom = cameraComponent.orthographicSize;
        side = transform.right; // Vector used to pan the camera sideways
        up = Vector3.Cross(side, Vector3.up); // Vector used to pan the camera upwards
        cameraComponent.orthographic = cameraMode == CameraMode.orthographic;
        selectionManager = GameObject.Find("SelectionManager");
    }

    // Update is called once per frame
    void Update()
    {
        //follow = GameObject.Find("CharacterInfoCanvas").GetComponent<TabController>().follow;
        center = false;
        if (Input.GetButtonDown("Follow") && selectedCharacter!=null)
        {
            follow = !follow;
            center = true;
        }

        Vector3 move = Vector3.zero;
        // To be sure that Vector3 is a vector of zeros

        float canUseMouse = EventSystem.current.IsPointerOverGameObject() ? 0 : 1;
        float mouseMovement = Input.GetMouseButton(1) ? mouseSensitivity * canUseMouse : 0;

        float forwardMotion = Input.GetAxis("CameraVertical") - Input.GetAxis("Mouse Y") * mouseMovement / Time.unscaledDeltaTime / 100;
        float sideMotion = Input.GetAxis("CameraHorizontal") - Input.GetAxis("Mouse X") * mouseMovement / Time.unscaledDeltaTime / 100;

        float rotateMotion = Input.GetAxis("CameraRotation");

        // We have set three new axis (CameraVertical, CameraHorizontal, CameraZoom) in Unity (Edit -> Project settings -> Input)
        selectedCharacter = selectionManager.GetComponent<SelectionManager>().selectedCharacter;

        cameraParent.transform.position += (cameraParent.transform.forward * forwardMotion + cameraParent.transform.right * sideMotion) * Time.unscaledDeltaTime * speed * currentZoom;

 

        if (rotateMotion != 0)
        {
            transform.LookAt(cameraParent.transform);
            cameraParent.transform.Rotate(new Vector3 (0,1,0) * rotateMotion * Time.unscaledDeltaTime * rotateSpeed);
        }

        if (selectedCharacter != null && follow == true) //follow a character
        {
            camFirstObject.transform.position = new Vector3( selectedCharacter.transform.position.x , selectedCharacter.transform.position.y , selectedCharacter.transform.position.z);
            if (center)
            {
                cameraParent.transform.position = camFirstObject.transform.position ;
            }
        }

        float zoom = Input.GetAxis("CameraZoom") - Input.GetAxis("Mouse ScrollWheel") * mouseSensitivity * 40 * canUseMouse;
        

        if (zoom != 0) 
        {
            float newZoomValue;
            if (cameraMode == CameraMode.orthographic)
            {
                newZoomValue = cameraComponent.orthographicSize * (1 +zoom * zoomSpeed * Time.unscaledDeltaTime);
            }
            else
            {
                newZoomValue = cameraComponent.fieldOfView * (1 + zoom * zoomSpeed * Time.unscaledDeltaTime);
            }
            if (zoomMin <= newZoomValue && newZoomValue <= zoomMax)
            {
                if (cameraMode == CameraMode.orthographic)
                {
                    cameraComponent.orthographicSize = newZoomValue;
                }
                else
                {
                    cameraComponent.fieldOfView = newZoomValue;
                }
                currentZoom = newZoomValue;
            }
        }
    }

    public void ChangeFollowToggle()
    {
        
    }

}
