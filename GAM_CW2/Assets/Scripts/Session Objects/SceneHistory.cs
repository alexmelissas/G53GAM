using System.Collections.Generic;
using UnityEngine;

//! Singleton - Store list of scene viewing hierarchy
public class SceneHistory : MonoBehaviour
{
    //! The Singleton object
    public static SceneHistory scene_history;

    //! Keep a list of scenes opened in this session
    public List<string> scenes;

    //! Handle the Singleton object
    void Awake()
    {
        if (scene_history == null)
        {
            DontDestroyOnLoad(gameObject);
            scene_history = this;
        }
        else if (scene_history != this)
        {
            Destroy(gameObject);
        }
    }
}
