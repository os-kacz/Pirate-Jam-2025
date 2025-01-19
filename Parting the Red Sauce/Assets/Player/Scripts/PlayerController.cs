using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameObject playerObject;
    private GameObject rightStickLookat;
    private GameObject shootyBarrel;
    
    private CharacterController characterController;
    // i dont think this component works for 2D stuff
    
    private Vector2 movement;
    private Vector2 lookat;
    
    [SerializeField] private float playerSpeed = 3.4f;
    [SerializeField] private float rightStickLookatDistance = 1.5f;

    private string currentInputDevice;

    private void Start()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            characterController = playerObject.GetComponent<CharacterController>();
        }

        if (rightStickLookat == null)
        {
            rightStickLookat = GameObject.FindGameObjectWithTag("RightJoystick");
        }

        if (shootyBarrel == null)
        {
            shootyBarrel = GameObject.Find("ShootyBarrel");
        }
        
        movement = playerObject.transform.position;
        lookat = rightStickLookat.transform.position;
    }

    public void DeviceChangeEvent(PlayerInput playerInput)
    {
        //Debug.Log("Device Change Event: " + playerInput.devices[0].name);

        currentInputDevice = playerInput.devices[0].name;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        
        movement = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());

        if (context.ReadValue<Vector2>().x > 0.2f || context.ReadValue<Vector2>().x < -0.2f || 
            context.ReadValue<Vector2>().y > 0.2f || context.ReadValue<Vector2>().y < -0.2f)
        {
            if (currentInputDevice == "Keyboard")
            {
                lookat = new Vector2(
                    Mathf.Clamp(context.ReadValue<Vector2>().x, -1.0f, 1.0f) * rightStickLookatDistance,
                    Mathf.Clamp(context.ReadValue<Vector2>().y, -1.0f, 1.0f) * rightStickLookatDistance);
                // this section will handle mouse input. need to convert mouse position into a world vector
                // then the arrow will look at the converted vector
            }
            else
            {
                lookat = new Vector2(
                    context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y) * rightStickLookatDistance;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Enter: " + collision.gameObject.name);
    }

    private void Update()
    {
        //playerObject.transform.position += new Vector3(movement.x, movement.y, 0) * (playerSpeed * Time.deltaTime);

        playerObject.GetComponent<Rigidbody2D>().MovePosition(new Vector2(movement.x, movement.y) * (playerSpeed * Time.deltaTime));
        // need to use Rigidbody2D.MovePosition() to move the player or else collisions dont happen
        // still playing around with it
        
        rightStickLookat.transform.localPosition = new Vector3(lookat.x, lookat.y, 0);
        
        shootyBarrel.transform.LookAt(rightStickLookat.transform.position);
    }
}
