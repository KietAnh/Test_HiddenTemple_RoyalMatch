
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    public GameObject endGameUI;
    public TextMeshProUGUI textTimeRemain;

    public int _timeRemain;
    private float _timeElapsed;

    public void Init()
    {
        InitTimeRemain();
    }

    private void InitTimeRemain()
    {
        if (MainModel.lastTimeOpenGame == 0)
        {
            _timeRemain = MainModel.timeRemain;
        }
        else
        {
            DateTime now = DateTime.Now;
            TimeSpan offlineTimeSpan = now - new DateTime(MainModel.lastTimeOpenGame);
            _timeRemain = MainModel.timeRemain - (int)offlineTimeSpan.TotalSeconds;
        }
        
        
        if (_timeRemain >= 0)
        {
            MainModel.SetTimeRemain(_timeRemain);
            
            RefreshTextTimeRemain(_timeRemain);
        }
        else
        {
            RefreshTextTimeRemain(0);
            // end of event
        }

        GED.ED.dispatchEvent(EventID.OnSetTimeRemain);
    }

    private void RefreshTextTimeRemain(int secs)
    {
        TimeSpan t = TimeSpan.FromSeconds(secs);

        if (t.Hours >= 100)
        { 
            textTimeRemain.text = string.Format("{0:D3}:{1:D2}:{2:D2}", (int)t.TotalHours, t.Minutes, t.Seconds);
        }
        else
        {
            textTimeRemain.text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)t.TotalHours, t.Minutes, t.Seconds);
        }
        
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed >= 1f)
        {
            _timeElapsed = 0;
            _timeRemain -= 1;
            if (_timeRemain >= 0)
            {
                MainModel.SetTimeRemain(_timeRemain);
                RefreshTextTimeRemain(_timeRemain);
            }
            if (_timeRemain == 0)
            {
                GED.ED.dispatchEvent(EventID.OnSetTimeRemain);
            }    
        }
    }


    public void ShowReplayButton()
    {
        if (MainModel.timeRemain > 0)
            endGameUI.SetActive(true);
    }

    public void ReplayOnClick()
    {
        StageManager.Singleton.SelfDestroy();
        endGameUI.SetActive(false);
        StartCoroutine(DelaySpawnStage());
    }

    IEnumerator DelaySpawnStage()
    {
        yield return null;
        var stageTemplate = Resources.Load<GameObject>("stage-template");
        Instantiate(stageTemplate);
        StageManager.Singleton.Initialize();
        StageManager.Singleton.Slide();
    }
}
