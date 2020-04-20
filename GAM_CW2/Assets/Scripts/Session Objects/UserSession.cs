using UnityEngine;

//! Singleton - Store the User object
public class UserSession : MonoBehaviour {

    //! The Singleton object
    public static UserSession user_session;

    public User user;

    //! Handle the Singleton object
	void Awake () {
        if (user_session == null)
        {
            DontDestroyOnLoad(gameObject);
            user_session = this;
        }
        else if(user_session!=this)
        {
            Destroy(gameObject);
        }
	}
}
