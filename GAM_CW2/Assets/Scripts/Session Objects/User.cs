using System;
using UnityEngine;

[Serializable]
//! JSON-able object storing the user's account information
public class User{

    public string id;
    public string username;
    public string password;
    public string accessToken;
    public string accessTokenSecret;

    //! Basic constructor
    public User(string name, string pass)
    {
        username = name;
        password = pass;
    }

    //! JSON constructor
    public static User CreateUserFromJSON (string json)
    {
        User temp = new User(" ", " ");
        JsonUtility.FromJsonOverwrite(json,temp);
        return temp;
    }

    public string GetUsername()
    {
        return username;
    }

    public string GetPassword()
    {
        return password;
    }

    public string GetID()
    {
        return id;
    }

    public string GetAT()
    {
        return accessToken;
    }

    public string GetATS()
    {
        return accessTokenSecret;
    }
}
