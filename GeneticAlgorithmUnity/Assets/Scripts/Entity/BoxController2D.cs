using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class BoxController2D : Controller2D
{
    private BoxCollider2D boxCollider;
    private RaycastOrigins raycastOrigins;

    [SerializeField]
    private float skinWidth = .015f;

    [SerializeField]
    private Vector2 rayCount;
    private Vector2 raySpacing;

    private struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    private void UpdateRaycastOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2f);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2f);

        rayCount.x = Mathf.Clamp(rayCount.x, 2, int.MaxValue);
        rayCount.y = Mathf.Clamp(rayCount.y, 2, int.MaxValue);

        raySpacing.x = bounds.size.x / (rayCount.x - 1);
        raySpacing.y = bounds.size.y / (rayCount.y - 1);
    }

    private void VerticalCollisions(ref Vector2 velocity)
    {
        float direction = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < rayCount.y; i++)
        {
            Vector2 rayOrigin = (direction == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (raySpacing.y * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * direction, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * direction * rayLength, Color.red);

            if (hit && hit.collider.gameObject != gameObject)
            {
                velocity.y = (hit.distance - skinWidth) * direction;
                rayLength = hit.distance;
            }
        }
    }

    private void HorizontalCollisions(ref Vector2 velocity)
    {
        float direction = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < rayCount.x; i++)
        {
            Vector2 rayOrigin = (direction == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (raySpacing.x * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * direction * rayLength, Color.red);

            if (hit && hit.collider.gameObject != gameObject)
            {
                velocity.x = (hit.distance - skinWidth) * direction;
                rayLength = hit.distance;
            }
        }
    }

    public override void Move(Vector2 velocity)
    {
        UpdateRaycastOrigins();

        VerticalCollisions(ref velocity);
        HorizontalCollisions(ref velocity);

        transform.Translate(velocity);
    }
}
