using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalCameraMovement : MonoBehaviour {

	private new Camera camera;

    public bool isMovementRestricted = false;

    public float panSpeed = 2;
    public float scrollSpeed = 150;
    public float rotationSpeed = 5;
    public float panBorderThickness = 10;
    public Vector2 panLimitMin = new Vector2(-150, -110);
    public Vector2 panLimitMax = new Vector2(120, 80);
    public float scrollMin = 40;
    public float scrollMax = 140;

    public float groundLevelY = 38;
    public float maxCameraMovePerSecond = 200;

    // Use this for initialization
    void Start () {
		camera = GetComponent<Camera>();
    }
 
    // Update is called once per frame
    void Update () {
        if (camera.enabled && !isMovementRestricted)
        {
            MoveCamera();
            RotateCamera();
            LockMouseWhenMoving();
        }
    }
 
    private void MoveCamera() {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0,0,0);
 
        movement = HandleHorizontalCameraMovement(movement);
        movement = Quaternion.Euler(new Vector3(0, camera.transform.eulerAngles.y, 0)) * movement;
        movement += camera.transform.TransformDirection(Vector3.forward * scrollSpeed * Input.GetAxis("Mouse ScrollWheel"));

        // calculate desired camera position based on received input
        Vector3 origin = camera.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        destination = ApplyLimits(destination);
 
        if(destination != origin) {
            camera.transform.position = Vector3.MoveTowards(origin, destination, maxCameraMovePerSecond);
        }
    }
 
    private void RotateCamera() {
        Vector3 rotationPoint = GetGroundRotationPoint();
        float rotateDegrees = 0f;

        // detect Right mouse button is down
        if(Input.GetMouseButton(2)) {
            rotateDegrees += Input.GetAxis("Mouse Y") * rotationSpeed;
            rotateDegrees += Input.GetAxis("Mouse X") * rotationSpeed;
            camera.transform.RotateAround(rotationPoint, Vector3.up, rotateDegrees);
        }
    }

    private Vector3 HandleHorizontalCameraMovement(Vector3 cameraPosition) {
        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= (Screen.height - panBorderThickness)) {
            cameraPosition.z += panSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness) {
            cameraPosition.z -= panSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= (Screen.width - panBorderThickness)) {
            cameraPosition.x += panSpeed;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness) {
            cameraPosition.x -= panSpeed;
        }

        if (Input.GetMouseButton(1)) {
            cameraPosition.z += Input.GetAxis("Mouse Y") * panSpeed;
            cameraPosition.x += Input.GetAxis("Mouse X") * panSpeed;
        }

        return cameraPosition;
    }

    private Vector3 ApplyLimits(Vector3 cameraPosition) {
        Vector3 groundCameraPoint = GetGroundRotationPoint();
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, panLimitMin.x, panLimitMax.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, scrollMin, scrollMax);
        cameraPosition.z = Mathf.Clamp(cameraPosition.z, panLimitMin.y, panLimitMax.y);

        return cameraPosition;
    }

    private Vector3 GetGroundRotationPoint() {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Plane hPlane = new Plane(Vector3.up, new Vector3(0, groundLevelY, 0));
        float distance = 0;

        if (hPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        } 
        else 
        {
            return Vector3.zero;
        }
    }

    private void LockMouseWhenMoving() {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {

        }

        if ((Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2)) && 
            !(Input.GetMouseButton(1) || Input.GetMouseButton(2))) {
        }
    }
}
