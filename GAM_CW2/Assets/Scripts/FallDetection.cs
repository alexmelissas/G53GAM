using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("FALL");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
