using UnityEngine;

public class GameController : MonoBehaviour
{

    public Transform pfHealthbar;
    private HealthSystem healthSystem;


    private void Start()
    {
        healthSystem = new HealthSystem(100, 0);

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
