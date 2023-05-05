using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapsCheck : MonoBehaviour
{
    [Header("Points")]
    public GameObject start;
    public GameObject end;
    public GameObject[] checkpoints;

    [Header("Settings")]
    public float laps = 1;

    [Header("Information")]
    private float currentCheckpoint;
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
    }

    private void Update()
    {
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
                print("Go");
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
                        print("Missed A Checkpoin");
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
                        print($"Lap {currentLap}");
                    }
                    else
                    {
                        print("Missed A Checkpoint");
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
                    print($"Correct Checkpoint: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000}");
                    currentCheckpoint++;
                }
                // If the checkpoint is incorrect
                else if (thisCheckpoint == checkpoints[i] && i + 1 != currentCheckpoint + 1)
                {
                    print($"Go Back!");
                }
            }
        }
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = 50;
        // Current time
        string formattedCurrentLapTime = $"Current: {Mathf.FloorToInt(currentLapTime / 60)}:{currentLapTime % 60:00.000} - (Lap {currentLap})";
        GUI.Label(new Rect(100, 20, 250, 900), formattedCurrentLapTime);

        // Best time
        string formattedBestLapTime = $"Best: {Mathf.FloorToInt(bestLapTime / 60)}:{bestLapTime % 60:00.000} - (Lap {bestLap})";
        GUI.Label(new Rect(1600, 20, 250, 900), (started) ? formattedBestLapTime : "0:00.000");
    }
}