using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//! Scene forward/back management
public class ChangeScene : MonoBehaviour {

    //! Go forward a scene and add current scene to SceneHistory
    public void Forward(String next_scene){
        SceneHistory.scene_history.scenes.Add(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(next_scene);
        return;
    }

    //! Go back to the top scene of SceneHistory
    public void Back() {
        if(SceneHistory.scene_history.scenes!=null)
        {
            int top = SceneHistory.scene_history.scenes.Count - 1;
            if (top >= 0)
            {
                SceneManager.LoadScene(SceneHistory.scene_history.scenes[top]);
                SceneHistory.scene_history.scenes.RemoveAt(top);
            }
            else
                Application.Quit();
        }
        else
            Application.Quit();
        return;
    }
}
