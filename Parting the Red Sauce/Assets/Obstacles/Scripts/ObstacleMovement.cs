using System;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    
    [SerializeField] private float moveByAngle;
    
    private float angle;
    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        angle += moveByAngle;
        rb2d.MoveRotation(angle * Time.fixedDeltaTime);
    }
}
