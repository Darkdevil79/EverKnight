﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class RayCastController : MonoBehaviour {

    public const float SKINWIDTH = .015f;
    const float distanceBetweenRays = 0.25f;

    public LayerMask collisionMask;

    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D objectCollider;
    public RaycastOrigins raycastOrigins;
    

    public virtual void Awake()
    {
        objectCollider = GetComponent<BoxCollider2D>();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        Bounds bounds = objectCollider.bounds;
        bounds.Expand(SKINWIDTH * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        raycastOrigins.bottomCentre = new Vector2(bounds.min.x + (bounds.size.x / 2), bounds.min.y);
    }

    public void CalculateRaySpacing()
    {
        Bounds bounds = objectCollider.bounds;
        bounds.Expand(SKINWIDTH * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / distanceBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / distanceBetweenRays);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);

    }


}

public struct RaycastOrigins
{
    public Vector2 topLeft;
    public Vector2 topRight;
    public Vector2 bottomLeft;
    public Vector2 bottomRight;
    public Vector2 bottomCentre;
}

