using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraAreaSensor : MonoBehaviour
{

  [SerializeField]
  FollowCameraManager followCamera;

  private void OnTriggerEnter2D(Collider2D other)
  {
    CameraControlArea controlArea = other.gameObject.GetComponent<CameraControlArea>();
    if (controlArea)
    {
      followCamera.SetControlArea(controlArea);
    }
  }

}
