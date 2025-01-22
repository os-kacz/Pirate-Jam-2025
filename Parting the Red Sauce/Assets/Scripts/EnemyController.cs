using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    //events
    public event EventHandler OnEnemyAttack;

    //healthbar information
    public Transform pfHealthbar;
    private HealthSystem healthSystem;
    private Vector3 healthbarLocation;
    private Transform healthbarTransform;

    //movemeent and detection information
    [SerializeField] private int speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float detectionRange;
    [SerializeField] private int fieldOfView;
    private float attackRange = 0.75f;
    private Vector3 targetDirection;
    private float directionChangeCooldown;
    private Rigidbody2D _rigidbody;
    public GameObject player;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        targetDirection = this.transform.up;
    }

    private void Start()
    {
        // sets health and defence, need to add a way to input specifc health and defence to each enemy
        healthSystem = new HealthSystem(100, 0);
        healthbarTransform = Instantiate(pfHealthbar, healthbarLocation, Quaternion.identity);
        HealthBar healthBar = healthbarTransform.GetComponent<HealthBar>();
        healthBar.HealthBarSetup(healthSystem);

    }


    private void Update()
    {
        healthbarLocation = this.transform.position + new Vector3(0, 1, 0);

        UpdateDirection();
        UpdateRotation(); 
        SetVelocity();
        UpdateHealthbarLocation();

    }
    


    private void UpdateDirection()
    {
        RandomDirectionChange();

        if (DetectPlayer())
        {
            //sets the direction of this enemy towards the player
            targetDirection = player.transform.position - this.transform.position;
        }
    }

    private void RandomDirectionChange()
    {
        directionChangeCooldown -= Time.deltaTime;

        if (directionChangeCooldown <= 0)
        {
            float angleChange = UnityEngine.Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, this.transform.forward);

            targetDirection = rotation * targetDirection;

            directionChangeCooldown = UnityEngine.Random.Range(1f, 5f);
        }
    }

    private void UpdateRotation()
    {
        // sets the target rotation and rotates the rigidbody of the enemy to the players direction, still confused on how quartenions work
        Quaternion targetRoattion = Quaternion.LookRotation(this.transform.up, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(this.transform.rotation, targetRoattion, rotationSpeed * Time.deltaTime);
        

        _rigidbody.SetRotation(rotation);

    }

    private void SetVelocity()
    {
        // makes this enemy move towards the player as long as the player is within range
        _rigidbody.linearVelocity = transform.up * speed;

    }


    private bool DetectPlayer()
    {
        // sets and noramlises the direction from this enemy to the player
        Vector3 directionToPlayer = player.transform.position - this.transform.position;
        directionToPlayer.Normalize();

        //find the angle and the distance from this enemy to the player
        float angleToPlayer = Vector3.Angle(this.transform.up, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        //checks if the player is within this enemies FOV and if it is wihtin a decectable range of this enemy
        if (angleToPlayer <= fieldOfView / 2f)
        {
            if (distanceToPlayer <= detectionRange)
            {
                if(distanceToPlayer <= attackRange)
                {
                    Attack();
                }
                //returns that the player is wihtin range
                return true;
            }
        }

        //returns that the player is not within range
        return false;

    }

    private void Attack()
    {
        // play animation for attacking or something
        //information about damage numbers
        // maybe use specail attack class?
        if (OnEnemyAttack != null) OnEnemyAttack(this, EventArgs.Empty);
    }

    private void UpdateHealthbarLocation()
    {
        healthbarTransform.SetPositionAndRotation(healthbarLocation, Quaternion.identity);
    }

}
