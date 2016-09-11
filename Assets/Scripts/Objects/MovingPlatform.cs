using UnityEngine;
using System.Collections.Generic;

public class MovingPlatform : RayCastController
{
    public LayerMask passengerMask;
    public Vector3[] localWaypoints;
    public float PlatformSpeed;
    public bool Cyclic;
    public float WaitTime;

    [Range (0,2)]
    public float EaseAmount;

    List<PassengerMovement> passengerMovement;
    Dictionary<Transform, Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();

    Vector3[] globalWaypoints;
    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    public override void Start()
    {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];
        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }

    }

    void Update()
    {
        UpdateRaycastOrigins();

        Vector3 velocity = CalculayePlatformMovement();

        CalculatePassengerMovement(velocity);

        MovePassengers(true);
        transform.Translate(velocity);
        MovePassengers(false);
    }

    Vector3 CalculayePlatformMovement()
    {
        if (Time.time < nextMoveTime)
            return Vector3.zero;

        fromWaypointIndex %= globalWaypoints.Length;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * PlatformSpeed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (!Cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Length - 1)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + WaitTime;
        }

       
        return newPos - transform.position;
    }

    void MovePassengers(bool beforeMovePlatform)
    {
        foreach (PassengerMovement passenger in passengerMovement)
        {
            if (!passengerDictionary.ContainsKey(passenger.tPassenger))
            {
                passengerDictionary.Add(passenger.tPassenger, passenger.tPassenger.GetComponent<Controller2D>());
            }

            if (passenger.moveBeforePlatform == beforeMovePlatform)
            {
                passengerDictionary[passenger.tPassenger].Move(passenger.velocity, passenger.standingOnPlatform);
            }

        }
    }

    void CalculatePassengerMovement(Vector3 velocity)
    {
        HashSet<Transform> MovedPassengers = new HashSet<Transform>();
        passengerMovement = new List<PassengerMovement>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        // Vertically moving platform

        if (velocity.y != 0)
        {
            float rayLenght = Mathf.Abs(velocity.y) + SKINWIDTH;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, passengerMask);

                if (hit)
                {
                    if (!MovedPassengers.Contains(hit.transform))
                    {
                        MovedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0;
                        float pushY = velocity.y - (hit.distance - SKINWIDTH) * directionY;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), directionY == 1, true));
                    }
                }
            }
        }

        // Horizontally moving platform

        if (velocity.x != 0)
        {
            float rayLenght = Mathf.Abs(velocity.x) + SKINWIDTH;

            for (int i = 0; i < horizontalRayCount; i++)
            {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, passengerMask);


                if (hit)
                {
                    if (!MovedPassengers.Contains(hit.transform))
                    {
                        MovedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - SKINWIDTH) * directionX;
                        float pushY = -SKINWIDTH;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), false, true));
                    }
                }

            }

        }

        // Passenger on top of a horizontally or downward moving platform

        if (directionY == -1 || velocity.y == 0 && velocity.x != 0)
        {
            float rayLenght = SKINWIDTH * 2;

            for (int i = 0; i < verticalRayCount; i++)
            {
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLenght, passengerMask);

                if (hit)
                {
                    if (!MovedPassengers.Contains(hit.transform))
                    {
                        MovedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX, pushY), true, false));
                    }
                }
            }
        }

    }

    float Ease(float x)
    {
        float a = EaseAmount + 1;
        return Mathf.Pow(x,a) / (Mathf.Pow(x, a) + Mathf.Pow(1-x , a));
    }

    void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.blue;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying)? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}

public struct PassengerMovement
{
    public Transform tPassenger;
    public Vector3 velocity;
    public bool standingOnPlatform;
    public bool moveBeforePlatform;

    public PassengerMovement(Transform _pTransform, Vector3 _velocity, bool _standingOnPlatform, bool _moveBeforePlatform)
    {
        tPassenger = _pTransform;
        velocity = _velocity;
        standingOnPlatform = _standingOnPlatform;
        moveBeforePlatform = _moveBeforePlatform;
    }
}