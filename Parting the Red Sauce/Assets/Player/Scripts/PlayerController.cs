using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    private GameObject rightStickLookat;
    private GameObject shootyBarrel;
    private Vector2 movement;
    private Vector2 lookat;
    [SerializeField] private float playerSpeed = 3.4f;
    [SerializeField] private float rightStickLookatDistance = 1.5f;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (rightStickLookat == null)
        {
            rightStickLookat = GameObject.FindGameObjectWithTag("RightJoystick");
        }

        if (shootyBarrel == null)
        {
            shootyBarrel = GameObject.Find("DEBUGDirectionArrow");
        }
        
        movement = player.transform.position;
        lookat = rightStickLookat.transform.position;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        movement = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        lookat = new Vector2(Mathf.Clamp(context.ReadValue<Vector2>().x, -1.0f, 1.0f), 
            Mathf.Clamp(context.ReadValue<Vector2>().y, -1.0f, 1.0f));
        // needs a check if the gamepad is enabled or not
    }

    private void Update()
    {
        player.transform.position += new Vector3(movement.x, movement.y, 0) * (playerSpeed * Time.deltaTime);
        
        rightStickLookat.transform.localPosition = new Vector3(lookat.x * rightStickLookatDistance, 
            lookat.y * rightStickLookatDistance, 0);
        
        shootyBarrel.transform.LookAt(rightStickLookat.transform.position);
    }
}
