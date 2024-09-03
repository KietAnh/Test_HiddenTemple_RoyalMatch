using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainModel
{
    public static int numPickaxe { get; private set; }
    public static int timeRemain { get; private set; } // in seconds
    public static long lastTimeOpenGame { get; private set; } // in ticks


    public static void LoadData()
    {
        numPickaxe = PlayerPrefs.GetInt("num-pickaxe", 0);
        timeRemain = PlayerPrefs.GetInt("time-remain", 432000);  
        lastTimeOpenGame = long.Parse(PlayerPrefs.GetString("last-time-open-game", "0")); 
    }

    public static void SaveData()
    {
        DevLog.Log("Data saved");
        PlayerPrefs.SetInt("num-pickaxe", numPickaxe);
        PlayerPrefs.SetInt("time-remain", timeRemain);
        PlayerPrefs.SetString("last-time-open-game", DateTime.Now.Ticks.ToString());
    }

    public static void AddPickaxe(int value = 1)
    {
        numPickaxe += value;
    }

    public static void ConsumePickaxe(int value = 1)
    {
        numPickaxe -= value;
    }

    public static void SetTimeRemain(int value)
    {
        timeRemain = value;
    }
}
