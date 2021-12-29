using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCheckController : MonoBehaviour
{
  [SerializeField]
  float checkerSkin = 0.3f;

  [SerializeField]
  LayerMask layers;

  [SerializeField]
  List<Direction> directions = new List<Direction>()
    { Direction.bottom, Direction.top, Direction.left, Direction.right };

  public enum Direction { bottom, top, left, right };

  Dictionary<Direction, Vector2> pointOffset = new Dictionary<Direction, Vector2>();
  Dictionary<Direction, Vector2> size = new Dictionary<Direction, Vector2>();
  Dictionary<Direction, bool> contacts = new Dictionary<Direction, bool>();

  private void FixedUpdate()
  {
    CheckContacts();
  }

  public bool IsContacting(Direction direction)
  {
    if (!contacts.ContainsKey(direction)) return false;
    return contacts[direction];
  }

  public void RecalculateCheckers(Collider2D collider)
  {
    RecalculateCheckerCenterpoints(collider);
    RecalculateCheckerSizes(collider);
  }

  private void RecalculateCheckerCenterpoints(Collider2D collider)
  {
    var (bottom, top, left, right) = (new Vector2(), new Vector2(), new Vector2(), new Vector2());

    bottom.x = top.x = collider.bounds.center.x - transform.position.x;
    bottom.y = transform.position.y - collider.bounds.max.y;
    top.y = transform.position.y - collider.bounds.min.y;

    left.x = transform.position.x - collider.bounds.min.x;
    left.y = right.y = transform.position.y - collider.bounds.center.y;
    right.x = transform.position.x - collider.bounds.max.x;

    if (directions.Contains(Direction.bottom)) pointOffset.Add(Direction.bottom, bottom);
    if (directions.Contains(Direction.top)) pointOffset.Add(Direction.top, top);
    if (directions.Contains(Direction.left)) pointOffset.Add(Direction.left, left);
    if (directions.Contains(Direction.right)) pointOffset.Add(Direction.right, right);
  }

  private void RecalculateCheckerSizes(Collider2D collider)
  {
    var (bottomS, topS, leftS, rightS) = (new Vector2(), new Vector2(), new Vector2(), new Vector2());

    bottomS.x = topS.x = collider.bounds.size.x - checkerSkin;
    bottomS.y = topS.y = leftS.x = rightS.x = checkerSkin;
    leftS.y = rightS.y = collider.bounds.size.y - checkerSkin;

    if (directions.Contains(Direction.bottom)) size[Direction.bottom] = bottomS;
    if (directions.Contains(Direction.top)) size[Direction.top] = topS;
    if (directions.Contains(Direction.left)) size[Direction.left] = leftS;
    if (directions.Contains(Direction.right)) size[Direction.right] = rightS;
  }

  private void CheckContacts()
  {
    foreach (Direction direction in pointOffset.Keys)
    {
      if (size.ContainsKey(direction) && directions.Contains(direction))
      {
        contacts[direction] = Physics2D.OverlapBox(transform.position + (Vector3)pointOffset[direction], size[direction], 0, layers);
      }
    }
  }

  private void OnDrawGizmosSelected()
  {
    foreach (Direction direction in pointOffset.Keys)
    {
      if (size.ContainsKey(direction))
      {
        Gizmos.DrawWireCube(transform.position + (Vector3)pointOffset[direction], size[direction]);
      }
    }
  }

}
