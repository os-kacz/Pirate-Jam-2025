using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameObject playerObject;
    private GameObject rightStickLookat;
    private GameObject shootyBarrel;

    private Rigidbody2D playerMovement;
    
    private Vector2 inputMovement;
    private Vector2 inputLookat;
    
    [SerializeField] private float playerSpeed = 3.4f;
    [SerializeField] private float rightStickLookatDistance = 1.5f;

    private string currentInputDevice;

    private void Start()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            playerMovement = playerObject.GetComponent<Rigidbody2D>();
        }

        if (rightStickLookat == null)
        {
            rightStickLookat = GameObject.FindGameObjectWithTag("RightJoystick");
        }

        if (shootyBarrel == null)
        {
            shootyBarrel = GameObject.Find("ShootyBarrel");
        }
        
        inputMovement = playerObject.transform.position;
        inputLookat = rightStickLookat.transform.position;
    }

    public void DeviceChangeEvent(PlayerInput playerInput)
    {
        //Debug.Log("Device Change Event: " + playerInput.devices[0].name);

        currentInputDevice = playerInput.devices[0].name;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        
        inputMovement = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());

        if (context.ReadValue<Vector2>().x > 0.2f || context.ReadValue<Vector2>().x < -0.2f || 
            context.ReadValue<Vector2>().y > 0.2f || context.ReadValue<Vector2>().y < -0.2f)
        {
            if (currentInputDevice == "Keyboard")
            {
                Vector3 mouseToScreen = Camera.main.ScreenToWorldPoint(
                    new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0));
                inputLookat = new Vector2(mouseToScreen.x, mouseToScreen.y);
            }
            else
            {
                inputLookat = new Vector2(
                    context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y) * rightStickLookatDistance;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision Enter: " + collision.gameObject.name);
    }

    private void FixedUpdate()
    {
        // so the movement is physics-based, we shall see how it goes lol!
        Vector2 newPlayerPosition = new Vector2(inputMovement.x, inputMovement.y) * (playerSpeed * Time.fixedDeltaTime);
        
        playerMovement.MovePosition(newPlayerPosition + playerMovement.position);
    }

    private void Update()
    {
        if (currentInputDevice == "Keyboard")
        {
            rightStickLookat.transform.position = new Vector3(inputLookat.x, inputLookat.y, 0);
        }
        else
        {
            rightStickLookat.transform.localPosition = new Vector3(inputLookat.x, inputLookat.y, 0);
        }
        shootyBarrel.transform.LookAt(rightStickLookat.transform.position);
    }
}
