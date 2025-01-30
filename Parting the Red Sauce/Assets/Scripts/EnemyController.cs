using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    //events

    //healthbar information
    public Transform pfHealthbar;
    private Vector3 healthbarLocation;

    //movemeent and detection information
    [SerializeField] private float walkSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float detectionRange;
    [SerializeField] private int fieldOfView;
    [SerializeField] private float attackRange;
    private Vector3 targetDirection;
    private float dotProduct;
    private float directionChangeCooldown;
    private Rigidbody2D _rigidbody;
    public GameObject player;

    //enemy attack
    public GameObject attackArea;
    private float attackTimer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        targetDirection = this.transform.up;
    }

    private void Start()
    {
        
        if(attackArea == null)
        {
            attackArea = GameObject.Find("EnemyAttackArea");
        }
        // sets health and defence, need to add a way to input specifc health and defence to each enemy
       /* healthSystem = new HealthSystem(100, 0);
        HealthBar healthBar = healthbarTransform.GetComponent<HealthBar>();
        healthBar.HealthBarSetup(healthSystem);
        healthbarTransform = Instantiate(pfHealthbar, healthbarLocation, Quaternion.identity);*/

    }


    private void FixedUpdate()
    {
        healthbarLocation = this.transform.position + new Vector3(0, -0.75f, 0);

        UpdateEnemyMovement();
        UpdateRotation();

        attackTimer += Time.deltaTime;
        Debug.Log(attackTimer);

    }
    

    private void UpdateEnemyMovement()
    {

        if (DetectPlayer())
        {
            _rigidbody.linearVelocity = targetDirection * chaseSpeed;
            if(Vector3.Distance(this.transform.position, player.transform.position) < attackRange)
            {
                _rigidbody.linearVelocity = targetDirection * 0f;
            }

        }
        else
        {
            RandomDirectionChange();
            _rigidbody.linearVelocity = targetDirection * walkSpeed;
        }
    }

    private void RandomDirectionChange()
    {

        directionChangeCooldown -= Time.deltaTime;

        if (directionChangeCooldown <= 0)
        {
            //float angleChange = UnityEngine.Random.Range(-90f, 90f);
            //Quaternion rotation = Quaternion.AngleAxis(angleChange, this.transform.up);
            float randomX = UnityEngine.Random.Range(-1f, 1f);
            float randomY = UnityEngine.Random.Range(-1f, 1f);
            Vector3 randomDirection = new Vector3(randomX, randomY, 0).normalized;

            targetDirection = randomDirection;

            directionChangeCooldown = UnityEngine.Random.Range(1f, 5f);
        }
    }

    private void UpdateRotation()
    {
        // sets the target rotation and rotates the rigidbody of the enemy to the players direction, still confused on how quartenions work
        //Quaternion targetRoattion = Quaternion.LookRotation(this.transform.up, targetDirection);
        //Quaternion rotation = Quaternion.RotateTowards(this.transform.rotation, targetRoattion, rotationSpeed * Time.deltaTime);

        /*if (Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.y))
        {

            if (targetDirection.x >= 0)
            {
                _rigidbody.SetRotation(Quaternion.Euler(0, 0, -90));
            }
            else
            {
                _rigidbody.SetRotation(Quaternion.Euler(0, 0, 90));
            }
        }
        else
        {
             if (targetDirection.y >= 0)
             {
                 _rigidbody.SetRotation(Quaternion.Euler(0, 0, 90));
             }
             else
             {
                 _rigidbody.SetRotation(Quaternion.Euler(0, 0, -90));
             }
        }*/

        if( dotProduct < 0)
        {
            _rigidbody.SetRotation(Quaternion.Euler(0, 0, this.transform.rotation.eulerAngles.z - 180));
            pfHealthbar.SetPositionAndRotation(healthbarLocation, Quaternion.identity);
            
        }
        

        //_rigidbody.SetRotation(rotation);

    }


    private bool DetectPlayer()
    {
        // sets and noramlises the direction from this enemy to the player
        Vector3 directionToPlayer = (player.transform.position - this.transform.position).normalized;

        //find the angle and the distance from this enemy to the player
        //float angleToPlayer = Vector3.Angle(this.transform.up, directionToPlayer);
        float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);

        //checks if the player is within this enemies FOV and if it is wihtin a decectable range of this enemy
        if (distanceToPlayer <= detectionRange)
        {
            targetDirection = directionToPlayer;
            dotProduct = Vector3.Dot(this.transform.right, directionToPlayer);
            return true;
        }

        //returns that the player is not within range
        return false;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IHealth playerHealth = collision.GetComponent<IHealth>();

        if (playerHealth != null)
        {
            if (attackTimer >= 3f)
            {
                DamageEvent.TriggerDamage(playerHealth, 10f);
                attackTimer = 0f;
                
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IHealth playerHealth = collision.GetComponent<IHealth>();

        if (playerHealth != null)
        {
        }
    }

    private void UpdateHealthbarLocation()
    {
        //healthbarTransform.SetPositionAndRotation(healthbarLocation, Quaternion.identity);
    }

}
