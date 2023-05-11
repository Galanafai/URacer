using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    [Header("Points")]
    public GameObject start;
    public GameObject end;
    public GameObject[] checkpoints;
    private Vector3 curCheckpoint;
    // public GameObject car;
    Rigidbody rb;

    [Header("Settings")]
    public float laps = 1;

    [Header("Information")]
    private int currentCheckpoint;
    private float currentLap;
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
        curCheckpoint = start.transform.position;
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
            // Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
            print("return key was pressed");
            Vector3 pos = curCheckpoint + new Vector3(0,5,0);
            Vector3 position =  Vector3.MoveTowards(pos, rb.position, 1f * Time.fixedDeltaTime);
            // rb.position = pos;
            // rb.AddForce(0f, 0f, 0f);
            rb.MovePosition(position);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.rotation = Quaternion.Euler(new Vector3(0,0,0));
            // rb.isKinematic = false;
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
        if (other.CompareTag("Checkpoint"))
        {
            GameObject thisCheckpoint = other.gameObject;

            // Started the race
            if (thisCheckpoint == start && !started)
            {
                curCheckpoint = thisCheckpoint.transform.position;
                print("Started");
                started = true;
            }
            // Ended the lap or race
            else if (thisCheckpoint == end && started)
            {
                // If all the laps are finished, end the race
                if (currentLap == laps)
                {
                    if (currentCheckpoint == checkpoints.Length)
                    {
                        if (currentLapTime < bestLapTime)
                        {
                            bestLap = currentLap;
                        }

                        finished = true;
                        print("Finished");
                    }
                    else
                    {
                        print("Did not go through all checkpoints");
                    }
                }
                // If all laps are not finished, start a new lap
                else if (currentLap < laps)
                {
                    if (currentCheckpoint == checkpoints.Length)
                    {
                        if (currentLapTime < bestLapTime)
                        {
                            bestLap = currentLap;
                            bestLapTime = currentLapTime; // Because the update function has already run this frame, we need to add this line or it won't work
                        }

                        currentLap++;
                        currentCheckpoint = 0;
                        currentLapTime = 0;
                        print($"Started lap {currentLap}");
                    }
                    else
                    {
                        print("Did not go through all checkpoints");
                    }
                }
            }

            // Loop through the checkpoints to compare and check which one the player touched
            for (int i = 0; i < checkpoints.Length; i++)
            {
                if (finished)
                    return;

                // If the checkpoint is correct
                if (thisCheckpoint == checkpoints[i] && i + 1 == currentCheckpoint + 1)
                {
                    curCheckpoint = thisCheckpoint.transform.position;
                    // thisCheckpoint.SetActive(false);
                    print($"Correct Checkpoint: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    currentCheckpoint++;
                }
                // If the checkpoint is incorrect
                else if (thisCheckpoint == checkpoints[i] && i + 1 != currentCheckpoint + 1)
                {
                    print($"Incorrect checkpoint");
                }
            }
        }
    }

    // private void OnGUI()
    // {
    //     // Current time
    //     string formattedCurrentLapTime = $"Current: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000} - (Lap {currentLap})";
    //     GUI.Label(new Rect(50, 10, 250, 100), formattedCurrentLapTime);

    //     // Best time
    //     string formattedBestLapTime = $"Best: {Mathf.FloorToInt(bestLapTime / 60)}:{bestLapTime % 60:00.000} - (Lap {bestLap})";
    //     GUI.Label(new Rect(250, 10, 250, 100), (started) ? formattedBestLapTime : "0:00.000");
    // }
}