using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticalCameraMovement : MonoBehaviour {

	private new Camera camera;
    // Use this for initialization
    void Start () {
		camera = GetComponent<Camera>();
    }
 
    // Update is called once per frame
    void Update () {
        if (camera.enabled)
        {
            MoveCamera();
            RotateCamera();
        }
    }
 
    private void MoveCamera() {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = new Vector3(0,0,0);
 
		if (!Input.GetMouseButton(1)) {
        	// horizontal camera movement
        	if(xpos >= 0 && xpos < ResourceManager.ScrollWidth) {
        	    movement.x -= ResourceManager.ScrollSpeed;
        	} else if(xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth) {
        	    movement.x += ResourceManager.ScrollSpeed;
        	}
	
        	// vertical camera movement
        	if(ypos >= 0 && ypos < ResourceManager.ScrollWidth) {
        	    movement.z -= ResourceManager.ScrollSpeed;
        	} else if(ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth) {
        	    movement.z += ResourceManager.ScrollSpeed;
        	}
		}

		//keyboard movement
        movement += GetBaseInput() * Time.deltaTime * ResourceManager.KeyboardScrollSpeed;

        // make sure movement is in the direction the camera is pointing
        // but ignore the vertical tilt of the camera to get sensible scrolling
        movement = camera.transform.TransformDirection(movement);
        movement.y = 0;
 
		// scroll to move closer
		movement += camera.transform.TransformDirection(Vector3.forward * Time.deltaTime * ResourceManager.ScrollZoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
 
        // calculate desired camera position based on received input
        Vector3 origin = camera.transform.position;
        Vector3 destination = origin;
        destination.x += movement.x;
        destination.y += movement.y;
        destination.z += movement.z;

        // limit away from ground movement to be between a minimum and maximum distance
        if(destination.y > ResourceManager.MaxCameraHeight) {
            destination.y = ResourceManager.MaxCameraHeight;
        } else if(destination.y < ResourceManager.MinCameraHeight) {
            destination.y = ResourceManager.MinCameraHeight;
        }
		if(destination.x > ResourceManager.MaxCameraRotationX) {
            destination.x = ResourceManager.MaxCameraRotationX;
        } else if(destination.x < ResourceManager.MinCameraRotationX) {
            destination.x = ResourceManager.MinCameraRotationX;
        }
		if(destination.z > ResourceManager.MaxCameraRotationZ) {
            destination.z = ResourceManager.MaxCameraRotationZ;
        } else if(destination.z < ResourceManager.MinCameraRotationZ) {
            destination.z = ResourceManager.MinCameraRotationZ;
        }
 
        // if a change in position is detected perform the necessary update
        if(destination != origin) {
            camera.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
        }
    }
 
    private void RotateCamera() {
        Vector3 origin = camera.transform.eulerAngles;
        Vector3 destination = origin;
 
        // detect Right mouse button is down
        if(Input.GetMouseButton(1)) {
            destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
            destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;

			// destination = new Vector3(Mathf.Clamp(destination.x, ResourceManager.MinCameraRotationX, ResourceManager.MaxCameraRotationX),
   	 		// 						  Mathf.Clamp(destination.y, ResourceManager.MinCameraRotationY, ResourceManager.MaxCameraRotationY),
   	 		// 						  destination.z);
        }
 
        // if a change in position is detected perform the necessary update
        if(destination != origin) {
            camera.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
        }
    }

    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W)){
            p_Velocity += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey(KeyCode.S)){
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A)){
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D)){
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
	}
}
