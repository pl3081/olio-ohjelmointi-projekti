using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitBuying : MonoBehaviour
{
    public void Exit()
    {
        SceneManager.LoadScene("map");
    }
}