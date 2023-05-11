using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("Points")]
    public GameObject start;
    public GameObject[] checkpoints;
    private GameObject checkpointObj;
    Rigidbody rb;

    [Header("Settings")]
    public int laps = 100;

    [Header("Information")]
    private int currentCheckpoint;
    private int currentLap;
    private bool started;
    private bool finished;

    private float currentLapTime;
    private float bestLapTime;
    private float bestLap;

    private void Start()
    {
        currentCheckpoint = 0;
        currentLap = 1;

        started = false;
        finished = false;

        currentLapTime = 0;
        bestLapTime = 0;
        bestLap = 0;
        checkpointObj = start;
        rb = GetComponent<Rigidbody>();
    }

    private void setKine()
    {
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            rb.isKinematic = true;
            print("return key was pressed");
            Vector3 pos = checkpointObj.transform.position + new Vector3(0,0,0);
            Vector3 position =  Vector3.MoveTowards(pos, rb.position, 1f * Time.fixedDeltaTime);
            rb.MovePosition(position);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = checkpointObj.transform.rotation;
            Invoke("setKine", 0.1f);
        }

        if (started && !finished)
        {
            currentLapTime += Time.deltaTime;
            
            if (bestLap == 0)
            {
                bestLap = 1;
            }
        }

        if (started)
        {
            if (bestLap == currentLap)
            {
                bestLapTime = currentLapTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If collided with checkpoint
        if (other.CompareTag("Checkpoint"))
        {
            print("Collided with checkpoint");

            GameObject thisCheckpoint = other.gameObject;

            // Start the race
            if (thisCheckpoint == start && !started)
            {
                checkpointObj = thisCheckpoint;
                print("Started");
                started = true;
                currentLapTime = 0;
            }  
            // If less than total laps
            // else if (currentLap < laps)
            // {
            // If at last checkpoint
            if (currentCheckpoint == checkpoints.Length)
            {
                print("AT LAST CHECKPOINT");
                if (currentLapTime < bestLapTime)
                {
                    bestLap = currentLap;
                    bestLapTime = currentLapTime; // Because the update function has already run this frame, we need to add this line or it won't work
                }

                // currentLap++;
                currentCheckpoint = 0;
                currentLapTime = 0;
                checkpointObj = start;
                print($"Started lap {currentLap}");
            }
            else
            {
                print($"Did not go through all checkpoints");
            }
            // }
            // }

            // Loop through the checkpoints to compare and check which one the player touched
            for (int i = 0; i < checkpoints.Length; i++)
            {
                // If the checkpoint is correct
                if (thisCheckpoint == checkpoints[i] && i + 1 == currentCheckpoint + 1)
                {
                    checkpointObj = thisCheckpoint;
                    print($"Correct Checkpoint: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    currentCheckpoint++;
                }
                // If the checkpoint is incorrect
                else if (thisCheckpoint == checkpoints[i] && i + 1 != currentCheckpoint + 1)
                {
                    print($"Incorrect checkpoint");
                }
            }
            print($"current: {currentCheckpoint} / total: {checkpoints.Length}");

        }
    }

    private void OnGUI()
    {
    //     // Current time
        string formattedCurrentLapTime = $"Current: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000} - (Lap {currentLap})";
        GUI.Label(new Rect(50, 50, 250, 100), formattedCurrentLapTime);

    //     // Best time
        string formattedBestLapTime = $"Best: {Mathf.FloorToInt(bestLapTime / 60)}:{bestLapTime % 60:00.000} - (Lap {bestLap})";
        GUI.Label(new Rect(50, 150, 250, 100), (started) ? formattedBestLapTime : "0:00.000");
    }
}