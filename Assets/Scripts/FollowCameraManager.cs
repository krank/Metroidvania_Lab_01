using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraManager : MonoBehaviour
{
  [SerializeField]
  GameObject target;

  [SerializeField]
  float timeToTarget = 1f;

  [SerializeField]
  float snapThreshold = 0.1f;

  [SerializeField]
  bool horizontal = true;

  [SerializeField]
  bool vertical = true;

  Vector3 originalPosition;

  float currentChangeSpeedX, currentChangeSpeedY;

  private void Start()
  {
    originalPosition = transform.position;
  }

  private void LateUpdate()
  {
    Vector3 destination = new Vector3(
      horizontal ? target.transform.position.x : originalPosition.x,
      vertical ? target.transform.position.y : originalPosition.y,
      originalPosition.z
    );

    if (Vector2.Distance(transform.position, destination) > snapThreshold)
    {
      Vector3 newPos = new Vector3(
        horizontal ? Mathf.SmoothDamp(transform.position.x, destination.x, ref currentChangeSpeedX, timeToTarget) : originalPosition.x,
        vertical ? Mathf.SmoothDamp(transform.position.y, destination.y, ref currentChangeSpeedY, timeToTarget) : originalPosition.y,
        originalPosition.z
      );

      transform.position = newPos;
    }
    else
    {
      transform.position = destination;
    }
  }
}
