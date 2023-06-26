using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorMovement : MonoBehaviour
{

    private Vector3 initialPosition;
    private bool isDoorOpen;

    public float maxOpen = 10f;

    public NavMeshSurface navMeshSurface;

    public Enemy_Alarm openDoors;

    public bool isDoorLocked = false;


    private void Start()
    {
        UpdateNavMesh();
        initialPosition = transform.position;
        isDoorOpen = false;

        Enemy_Alarm.doorclose += LockDoors;
        Enemy_Alarm.dooropen += UnLockDoors;
    }

    private void OnDisable()
    {
        Enemy_Alarm.doorclose -= LockDoors;
        Enemy_Alarm.dooropen -= UnLockDoors;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (isDoorOpen && !isDoorLocked)
            {

                OpenDoor();

            }
            else
            {

                CloseDoor();

            }

            isDoorOpen = !isDoorOpen;
        }
    }

    private void UpdateNavMesh()
    {
        if (navMeshSurface != null)
            navMeshSurface.BuildNavMesh();
    }


    public void OpenDoor()
    {

                transform.Translate(Vector3.up * maxOpen);
                UpdateNavMesh();

    }


    public void CloseDoor()
    {

                transform.position = initialPosition;
                UpdateNavMesh();

    }

    public void LockDoors()
    {

        CloseDoor();
        isDoorLocked = true;
        

    }

    public void UnLockDoors()
    {
        OpenDoor();
        isDoorLocked = false;

    }
}