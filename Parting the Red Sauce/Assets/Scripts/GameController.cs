using UnityEngine;

public class GameController : MonoBehaviour
{

    public Transform pfHealthbar;
    private HealthSystem healthSystem;


    private void Start()
    {
        // sets the health and defence of the player
        healthSystem = new HealthSystem(100, 0);

        // create the health bar, sets the postition and sets up the health bar to the player
        Transform healthBarTransform = Instantiate(pfHealthbar, new Vector3(0, 1), Quaternion.identity);
        HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.HealthBarSetup(healthSystem);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            healthSystem.TakeDamage(10);
            Debug.Log("Health = " + healthSystem.GetHealth());
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            healthSystem.Heal(10);
            Debug.Log("Health = " + healthSystem.GetHealth());
        }
    }
}
