using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookaheadController : MonoBehaviour
{
  Vector2Int direction = Vector2Int.right;

  [SerializeField]
  Transform lookaheadPoint;

  [SerializeField]
  Vector2 lookaheadAmount;

  IVelocityProvider xVelocityProvider;
  IVelocityProvider yVelocityProvider;

  void Start()
  {
    xVelocityProvider = GetComponent<MovementController>();
    yVelocityProvider = GetComponent<MovementController>();
  }

  void LateUpdate()
  {
    float velocityX = xVelocityProvider.GetVelocityX();
    float velocityY = yVelocityProvider.GetVelocityY();

    if (velocityX != 0) direction.x = Mathf.CeilToInt(velocityX);
    if (velocityY != 0) direction.y = Mathf.CeilToInt(velocityY);

    lookaheadPoint.transform.localPosition = new Vector3(
      direction.x * lookaheadAmount.x,
      direction.y * lookaheadAmount.y,
      0
    );
  }
}
