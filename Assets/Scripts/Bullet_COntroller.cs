using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_COntroller : MonoBehaviour
{
    public float aliveTime = 1f; // Delay in seconds

    private void Start()
    {
        // Invoke the DestroyPrefab method after the specified delay
        Invoke("DestroyPrefab", aliveTime);
    }

    private void DestroyPrefab()
    {
        // Destroy the game object this script is attached to
        Destroy(gameObject);
    }
}
