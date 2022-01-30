using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// https://blog.andrewnapierkowski.com/jumping-to-conclusions-mega-man-x/

[RequireComponent(typeof(DirectionCheckController))]
[RequireComponent(typeof(Rigidbody2D))]
public class JumpController : MonoBehaviour, IVelocityProvider
{
  enum JumpMethod { Force, Impulse, Velocity };

  [SerializeField]
  LayerMask groundLayer;


  [Header("Jump values")]
  [SerializeField]
  float jumpForce = 800;

  [SerializeField]
  JumpMethod jumpMethod = JumpMethod.Impulse;


  [Header("Variable jump height")]
  [SerializeField]
  bool variableJumpHeight = true;

  [SerializeField]
  float jumpVelocityMultiplier = 0.3f;


  [Header("Air jumps")]
  [SerializeField]
  int maxAirJumps = 0;
  int airJumps = 0;


  bool isGrounded;

  bool jump;
  bool cancelJump;

  Rigidbody2D rigidBody;
  DirectionCheckController directionCheck;

  Dictionary<JumpMethod, Action> jumpActions;

  private void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    directionCheck = GetComponent<DirectionCheckController>();

    Collider2D collider = GetComponent<Collider2D>();

    jumpActions = new Dictionary<JumpMethod, Action>()
    {
      {JumpMethod.Force, ForceJump},
      {JumpMethod.Impulse, ImpulseJump},
      {JumpMethod.Velocity, VelocityJump}
    };

    jump = cancelJump = false;

    directionCheck.RecalculateCheckers(collider);
  }

  private void Update()
  {
    isGrounded = directionCheck.IsContacting(DirectionCheckController.Direction.bottom);
    if (isGrounded)
    {
      airJumps = 0;
    }
    // print(rigidBody.velocity.y);
  }

  private void FixedUpdate()
  {
    if (jump)
    {
      jump = false;
      SetVelocityY(0);
      jumpActions[jumpMethod]();
    }

    if (cancelJump && variableJumpHeight)
    {
      cancelJump = false;
      SetVelocityY(rigidBody.velocity.y * jumpVelocityMultiplier);
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

  void SetVelocityY(float yVelocity)
  {
    Vector2 v = rigidBody.velocity;
    v.y = yVelocity;
    rigidBody.velocity = v;
  }

  void OnJump(InputValue value)
  {
    bool jumpButtonDown = value.Get<float>() != 0;
    // print(jumpButtonDown);

    if (jumpButtonDown)
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
    else
    {
      if (jump || rigidBody.velocity.y > 0)
      {
        cancelJump = true;
      }
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