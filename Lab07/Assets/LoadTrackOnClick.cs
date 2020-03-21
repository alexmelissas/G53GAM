using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTrackOnClick : MonoBehaviour
{
    public void loadTrack(int trackIndex)
    {
        SceneManager.LoadScene(trackIndex);
    }
}