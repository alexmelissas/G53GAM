using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreManagerData", menuName ="ScriptableObjects/ScoreManager",order =1)]
public class ScoreManager : ScriptableObject
{
    public float bestTime1 = 100.0f;
    public float bestTime2 = 100.0f;

    public void setTime(int level, float newTime, int trackNumber)
    {
        switch(level)
        {
            case 1: if (newTime < bestTime1) bestTime1 = newTime; break;
            case 2: if (newTime < bestTime2) bestTime2 = newTime; break;
        }
    }

    public float getTime(int level)
    {
        switch (level)
        {
            case 1: return bestTime1;
            case 2: return bestTime2;
            default: return 100.0f;
        }
    }
}
