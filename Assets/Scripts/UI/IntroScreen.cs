using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntroScreen : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _textTapToContinue;
    [SerializeField] private GameObject _textEventOver;

    void Awake()
    {
        GED.ED.addListener(EventID.OnSetTimeRemain, OnSetTimeRemain_RefreshView);
    }
    void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnSetTimeRemain, OnSetTimeRemain_RefreshView);
    }

    void Start()
    {
        AudioManager.Singleton.PlayMusic("bg-music");
    }

    private void RefreshView()
    {
        if (MainModel.timeRemain > 0)
        {
            _textTapToContinue.SetActive(true);
            _textEventOver.SetActive(false);
        }
        else
        {
            _textEventOver.SetActive(true);
            _textTapToContinue.SetActive(false);
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (MainModel.timeRemain <= 0)
            return;
        SelfDestroy();
        StageManager.Singleton.Initialize();
        StageManager.Singleton.Slide();
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    void OnSetTimeRemain_RefreshView(GameEvent gameEvent)
    {
        RefreshView();
    }
}
