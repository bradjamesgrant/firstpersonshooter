using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform FocusOn;
    public float Distance = 5f;
    public float MouseSensitivityX;
    public float MouseSensitivityY;

    private float _CurrentX = 0f;
    private float _CurrentY = 0f;
    private float _CurrentDistance;
    private Quaternion _CurrentQuaternion;

    private LayerMask _LayerMask = 1 << 11;


    private void Start()
    {
		_LayerMask = ~_LayerMask;

    }
    private void Update()
    {
        CameraZooming();
        CameraMovement();
    }

    private void CameraMovement()
    {

            float mouseX = Input.GetAxis("Mouse X") * MouseSensitivityX;
            float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivityY;
                _CurrentX += mouseX;
                _CurrentY -= mouseY;

        _CurrentY = Mathf.Clamp(_CurrentY, -89, 89); //so the y axis does not clip through ground
                                                     //Debug.Log(_CurrentX + ":" + _CurrentY);

        Vector3 dir = new Vector3(0, 0, -_CurrentDistance); //set distance between LookAt and camera
        Quaternion rotation;
            rotation = Quaternion.Euler(_CurrentY, _CurrentX, 0);
            _CurrentQuaternion = rotation;
        
        transform.position = FocusOn.position + transform.forward; //rotate camera around player
        //transform.LookAt(FocusOn); //camera face player
        transform.Translate(new Vector3(_CurrentDistance / Distance, _CurrentDistance / Distance, 0f)); //move the camera slightly to the top right so camera ray casting wont keep hitting the player
        transform.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z); //rotate lookAt object
    }

    //casts a ray from the player to the camera
    //if the ray hits anything but the camera, it means something is blocking the cameras view
    //with the raycast returning the position of the obstacle hit, we can place the camera just in front of that obstacle
    //<o is camera, (p) is player
    //if this scenario occurs with wall blocking (p)-----|------<o
    // move camera to be in front of wall        (p)---<o|--------
    private void CameraZooming()
    {
        RaycastHit hitBack;
        Ray frontRay = new Ray(transform.position, FocusOn.position - transform.position);
        Ray backRay = new Ray(FocusOn.position, this.transform.position - FocusOn.position);
        //Debug.DrawRay(FocusOn.position, this.transform.position - FocusOn.position, Color.black);
        //Debug.DrawRay(transform.position, -1 * (FocusOn.position - transform.position), Color.blue);
        if (Physics.Raycast(backRay, out hitBack))
        {
            if (hitBack.collider.gameObject.tag != "Water")
            {
                if (hitBack.collider.tag != "MainCamera")
                {
                    //creates a new distance from ray casting. clamps the distance between 0.01 and maxDist. Linear interpolations to create smooth motion
                    _CurrentDistance = Mathf.Lerp(_CurrentDistance, Mathf.Clamp((Vector3.Distance(hitBack.point, FocusOn.transform.position) - 2), 0.01f, Distance), 0.9f);
                    //Debug.Log("Name: " + hitBack.collider.name + " Point: " + hitBack.point + " Distance: " + hitBack.distance + "Current: " + _CurrentDistance);
                }
            }

        }
        else
        {
            _CurrentDistance = Distance;
        }
    }


}
