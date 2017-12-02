using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour {

    protected static float timeScale = 1.0f;

    void Update()
    {
        Move();
    }
    protected abstract void Move();
}
