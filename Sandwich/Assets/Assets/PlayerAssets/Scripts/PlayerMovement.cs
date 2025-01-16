using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public void Move(InputAction.CallbackContext context)
    { 
        transform.Translate(Vector3.forward * Time.deltaTime);
        Debug.Log("Forward");

        transform.Translate(Vector3.back * Time.deltaTime);
        Debug.Log("Backward");

        transform.Translate(Vector3.left * Time.deltaTime);
        Debug.Log("Left");

        transform.Translate(Vector3.right * Time.deltaTime);
        Debug.Log("Right");
    }
}
