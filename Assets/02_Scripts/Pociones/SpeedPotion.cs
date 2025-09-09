using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPotion : MonoBehaviour
{
    public float speedMultiplier = 2f;
    public float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            RigidbodyMovement move = player.GetComponent<RigidbodyMovement>();
            if (move != null)
            {
                player.StartCoroutine(ApplySpeed(move));
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator ApplySpeed(RigidbodyMovement move)
    {
        move.moveSpeed *= speedMultiplier;
        yield return new WaitForSeconds(duration);
        move.moveSpeed /= speedMultiplier;
    }
}
