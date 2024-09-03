using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    [SerializeField] private GameObject _introScreen;
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;//Screen.currentResolution.refreshRate;

        MainModel.LoadData();

        UIManager.Singleton.Init();
        AudioManager.Singleton.Init();
        EffectManager.Singleton.Init();

        LaunchGame();
    }
    private void LaunchGame()     // entry point of the application
    {
        DevLog.Log("Launch game!");
        _introScreen.SetActive(true);
        var stageTemplate = Resources.Load<GameObject>("stage-template");
        Instantiate(stageTemplate);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            MainModel.SaveData();
        }
    }

    private void OnDestroy()
    {
        MainModel.SaveData();
    }
}
