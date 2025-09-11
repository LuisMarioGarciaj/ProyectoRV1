using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBullet
{
    void Init(float damage, Transform target = null);
}
