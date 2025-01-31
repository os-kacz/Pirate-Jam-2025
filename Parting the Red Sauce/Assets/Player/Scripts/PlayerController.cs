using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //events 


    //healthbar information
    public Transform pfHealthbar;
    private Vector3 healthbarLocation;
    private Transform healthbarTransform;

    //Attack information
    [SerializeField]
    private GameObject attackBox;
    private float attackTimer;



    private GameObject playerObject;
    private GameObject rightStickLookat;
    private GameObject shootyBarrelArrow;

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


            //healthbar setup



            /*healthSystem.HealthSystem(100, 0);
             healthbarTransform = Instantiate(pfHealthbar, healthbarLocation, Quaternion.identity);
        HealthBar healthBar = healthbarTransform.GetComponent<HealthBar>();
        HealthSystem healthSystem = this.GetComponent<HealthSystem>();
        healthBar.HealthBarSetup(healthSystem);
            */


            // need to connect the enemyController class or think of a better way
            //enemyController.OnEnemyAttack += EnemyController_OnEnemyAttack;
        }

        if (rightStickLookat == null)
        {
            rightStickLookat = GameObject.FindGameObjectWithTag("RightJoystick");
        }

        if (shootyBarrelArrow == null)
        {
            shootyBarrelArrow = GameObject.Find("ShootyBarrel");
        }
        if (attackBox == null)
        {
            attackBox = GameObject.Find("AttackArea");
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

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attackBox.SetActive(true);
            Debug.Log("Atack area true");
        }
        

        if(context.canceled)
        {
            attackBox.SetActive(false);
            Debug.Log("Atack area false");
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collision Enter: " + collision.gameObject.name);
    }

    public void OnPossess(InputAction.CallbackContext context)
    {
        
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
        shootyBarrelArrow.transform.LookAt(rightStickLookat.transform.position);

        // keeping the healthbar above the players head
        //healthbarLocation = this.transform.position + new Vector3(0, 1, 0);
        //UpdateHealthbarLocation();

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
        IHealth enemyHealth = collision.GetComponent<IHealth>();

        if(enemyHealth != null)
        {
            if(collision.CompareTag("Enemy"))
            {
                DamageEvent.TriggerDamage(enemyHealth, 25f);

            }
        }
        
    }

}
