using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DirectionCheckController))]
[RequireComponent(typeof(Rigidbody2D))]
public class JumpController : MonoBehaviour, IVelocityProvider
{
  enum JumpMethod { Force, Impulse, Velocity };

  [SerializeField]
  LayerMask groundLayer;

  [SerializeField]
  float jumpForce = 800;

  [SerializeField]
  JumpMethod jumpMethod = JumpMethod.Impulse;

  [SerializeField]
  int maxAirJumps = 0;
  int airJumps = 0;

  bool isGrounded;
  bool jump;

  Rigidbody2D rigidBody;
  DirectionCheckController directionCheck;

  Dictionary<JumpMethod, Action> jumpActions;

  private void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    directionCheck = GetComponent<DirectionCheckController>();

    Collider2D collider = GetComponent<Collider2D>();

    directionCheck.RecalculateCheckers(collider);

    jumpActions = new Dictionary<JumpMethod, Action>();
    jumpActions.Add(JumpMethod.Force, ForceJump);
    jumpActions.Add(JumpMethod.Impulse, ImpulseJump);
    jumpActions.Add(JumpMethod.Velocity, VelocityJump);

  }

  private void Update()
  {
    isGrounded = directionCheck.IsContacting(DirectionCheckController.Direction.bottom);
    if (isGrounded)
    {
      airJumps = 0;
    }
  }

  private void FixedUpdate()
  {
    if (jump)
    {
      jump = false;
      NullifyVelocityY();
      jumpActions[jumpMethod]();
    }
  }

  void ImpulseJump()
  {
    rigidBody.AddForce(Vector2.up * jumpForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
  }

  void ForceJump()
  {
    rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
  }

  void VelocityJump()
  {
    rigidBody.velocity += Vector2.up * jumpForce * Time.fixedDeltaTime;
  }

  void NullifyVelocityY()
  {
    Vector2 v = rigidBody.velocity;
    v.y = 0;
    rigidBody.velocity = v;
  }

  void OnJump()
  {
    if (isGrounded)
    {
      jump = true;
    }
    else if (!isGrounded && airJumps < maxAirJumps)
    {
      jump = true;
      airJumps++;
    }
  }

  public float GetVelocityX()
  {
    return rigidBody.velocity.x;
  }

  public float GetVelocityY()
  {
    return rigidBody.velocity.y;
  }
}