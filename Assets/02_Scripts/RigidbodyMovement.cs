    using UnityEngine;

    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMovement : MonoBehaviour, IMovement
    {
        private Rigidbody rb;

        public float moveSpeed = 5f;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
        }

        public void Move(Vector3 velocity)
        {
            rb.MovePosition(rb.position + velocity * moveSpeed * Time.fixedDeltaTime);
        }
    }