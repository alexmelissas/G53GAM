using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreManagerData", menuName ="ScriptableObjects/ScoreManager",order =1)]
public class ScoreManager : ScriptableObject
{
    public double bestTime1 = 10000.0f;
    public double bestTime2 = 10000.0f;

    public void setTime(int level, double newTime)
    {
        switch(level)
        {
            case 1: if (newTime < bestTime1) bestTime1 = newTime; break;
            case 2: if (newTime < bestTime2) bestTime2 = newTime; break;
        }

    }

    public double getTime(int level)
    {
        switch (level)
        {
            case 1: return bestTime1;
            case 2: return bestTime2;
            default: return 10000.0f;
        }
    }
}
