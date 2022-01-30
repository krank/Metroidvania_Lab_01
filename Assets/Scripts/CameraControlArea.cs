using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CameraControlArea : MonoBehaviour
{

  public bool followHorizontal = true;
  public bool followVertical = false;

  public Collider2D Collider { get; private set; }

  private Rect cameraBounds;

  private void Start()
  {
    cameraBounds = new Rect();
    Collider = GetComponent<Collider2D>();
  }

  public Vector3 LimitCamera(Camera camera, Vector3 destination)
  {
    float halfCameraWidth = Camera.main.orthographicSize * Camera.main.aspect;
    float halfCameraHeight = Camera.main.orthographicSize;

    cameraBounds.min = new Vector2(
      Collider.bounds.min.x + halfCameraWidth,
      Collider.bounds.min.y + halfCameraHeight
    );

    cameraBounds.max = new Vector2(
      Collider.bounds.max.x - halfCameraWidth,
      Collider.bounds.max.y - halfCameraHeight
    );

    if (cameraBounds.max.x > cameraBounds.min.x)
    {
      destination.x = Mathf.Clamp(destination.x, cameraBounds.min.x, cameraBounds.max.x);
    }
    else
    {
      destination.x = cameraBounds.center.x;
    }

    if (cameraBounds.max.y > cameraBounds.min.y)
    {
      destination.y = Mathf.Clamp(destination.y, cameraBounds.min.y, cameraBounds.max.y);
    }
    else
    {
      destination.y = cameraBounds.center.y;
    }

    return destination;
  }

}
