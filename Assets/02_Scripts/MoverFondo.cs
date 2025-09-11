using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para usar RawImage

public class MoverFondo : MonoBehaviour
{
    public RawImage _img;   // Imagen de tipo RawImage
    public float _x;        // Velocidad en eje X
    public float _y;        // Velocidad en eje Y

    // Update is called once per frame
    void Update()
    {
        // Movimiento infinito del fondo
        _img.uvRect = new Rect(
            _img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime,
            _img.uvRect.size
        );
    }
}
