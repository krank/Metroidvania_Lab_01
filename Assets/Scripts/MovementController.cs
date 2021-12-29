using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// https://gamedevbeginner.com/input-in-unity-made-easy-complete-guide-to-the-new-system/
// https://pressstart.vip/tutorials/2018/10/19/71/rigidbody-vs-translate.html

public class MovementController : MonoBehaviour, IVelocityProvider
{
  [SerializeField]
  Vector2 speed;

  Rigidbody2D rigidBody;
  Vector2 movement;

  private void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
  }

  private void Update()
  {
    transform.Translate(movement * speed * Time.deltaTime);
  }

  void OnMovement(InputValue value)
  {
    movement = value.Get<Vector2>();
  }

  public float GetVelocityX()
  {
    return movement.x;
  }

  public float GetVelocityY()
  {
    return movement.y;
  }
}
