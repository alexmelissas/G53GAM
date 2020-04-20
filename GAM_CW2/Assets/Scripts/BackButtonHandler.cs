using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Singleton - Maintain the screen history list.
public class BackButtonHandler : MonoBehaviour
{
    //! The Singleton object
    public static BackButtonHandler back_button_handler;

    //! Handle the Singleton object
    void Awake()
    {
        if (back_button_handler == null)
        {
            DontDestroyOnLoad(gameObject);
            back_button_handler = this;
        }
        else if (back_button_handler != this)
        {
            Destroy(gameObject);
        }
    }

    //! Handle native Android back button pressed
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                gameObject.AddComponent<ChangeScene>().Back();
                return;
            }
        }

    }
}
