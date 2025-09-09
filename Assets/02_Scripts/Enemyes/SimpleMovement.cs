using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour, IMovement2
{
    public void MoveTowards(Transform target, float speed, Transform self)
    {
        if (target == null || self == null) return;
        Vector3 dir = (target.position - self.position).normalized;
        self.position += dir * speed * Time.deltaTime;
    }
}
