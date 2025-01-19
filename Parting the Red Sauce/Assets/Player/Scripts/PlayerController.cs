using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private GameObject player;
    private Vector2 movement;
    [SerializeField] float playerSpeed = 0.2f;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        movement = player.transform.position;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
        movement = new Vector2(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }

    private void Update()
    {
        player.transform.position += new Vector3(movement.x, movement.y, 0) * (playerSpeed * Time.deltaTime);
    }
}
