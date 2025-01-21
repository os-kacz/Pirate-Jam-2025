using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private int speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float detectionRange;
    [SerializeField] private int fieldOfView;

    private Vector2 targetDirection;
    private Rigidbody2D _rigidbody;
    public GameObject player;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateToPlayer();
        SetVelocity();
    }
    


    private void UpdateTargetDirection()
    {
        //checks if the player is within range
        if(DetectPlayer())
        {
            //sets the direction of this enemy towards the player
            targetDirection = player.transform.position - this.transform.position;
        }
        else
        {
            // set the direstion to 0 if the player is outside the detection range

            targetDirection = Vector2.zero;
        }
    }

    private void RotateToPlayer()
    {
        // if the object has a direction, the player is within range so if it is 0 do nohting here
        if(targetDirection == Vector2.zero)
        {
            return;
        }

        // sets the target rotation and rotates the rigidbody of the enemy to the players direction, still confused on how quartenions work
        Quaternion targetRoattion = Quaternion.LookRotation(this.transform.up, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(this.transform.rotation, targetRoattion, rotationSpeed * Time.deltaTime);

        _rigidbody.SetRotation(rotation);

    }

    private void SetVelocity()
    {
        // checks if the player is within range by checking if this enemy has a direction
        if(targetDirection == Vector2.zero)
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
        else
        {
            // makes this enemy move towards the player as long as the player is within range
            _rigidbody.linearVelocity = transform.up * speed;

        }
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
                Debug.Log(distanceToPlayer);
                //returns that the player is wihtin range
                return true;
            }
        }

        //returns that the player is not within range
        return false;

    }

}
