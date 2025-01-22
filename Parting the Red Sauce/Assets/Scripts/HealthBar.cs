using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    public void HealthBarSetup(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        // makes this health bar listen to the health system it is connected to, checking if the event OnHealthChanged has been fired
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
    }

    private void HealthSystem_OnHealthChanged(object sender, System.EventArgs e)
    {
        //when the event is fired off, it finds the bar within the healthbar prefeb and adjusts the scale to apear as if health has been lost
        transform.Find("Bar").localScale = new Vector3(healthSystem.GetHealthPercentage(), 1);
    }

    private void Update()
    {

    }

}
