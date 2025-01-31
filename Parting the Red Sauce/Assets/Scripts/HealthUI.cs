using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private HealthSystem health;
    public GameObject healthBar;

    private void Start()
    {
        health = this.GetComponent<HealthSystem>();
    }

    private void Update()
    {
      
        healthBar.transform.Find("Bar").localScale = new Vector3(health.GetHealthPercentage(), 1);
    }

}
