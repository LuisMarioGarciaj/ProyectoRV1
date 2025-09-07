using UnityEngine;

public class Player : MonoBehaviour
{
    private IInputProvider inputProvider;
    private IMovement movement;

    private Vector3 moveInput;

    void Awake()
    {
        // Inyección de dependencias simples
        inputProvider = new KeyboardInput();
        movement = GetComponent<IMovement>();
    }

    void Update()
    {
        moveInput = inputProvider.GetInput();
    }

    void FixedUpdate()
    {
        movement.Move(moveInput);
    }
}