using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplosive 
{
    void Explode(float damage, float radius, Transform selft);
}
