using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    void Update()
    {

        if (Input.GetKeyDown("escape"))
            SceneManager.LoadScene("Main Menu");

    }

}

