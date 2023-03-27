using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{

    public float speed = 0.2f;
    public float strength = 5f;

    private float randomOffset;

    // Use this for initialization
    void Start()
    {
        randomOffset = Random.Range(-2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Sin(Time.time * speed + randomOffset) * strength;
        transform.position = pos;
    }
}