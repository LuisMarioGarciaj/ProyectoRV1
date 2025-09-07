using UnityEngine;

public class KeyboardInput : IInputProvider
{
    public Vector3 GetInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        return new Vector3(moveX, 0, moveZ);
    }
}