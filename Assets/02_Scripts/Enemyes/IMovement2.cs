using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement2
{
    void MoveTowards(Transform target, float speed, Transform self);
}
