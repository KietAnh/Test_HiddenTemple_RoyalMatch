using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBehaviour : MonoBehaviour
{
    public List<Transform> missingGemList;
    [SerializeField] private Animation _anim;
    [SerializeField] private string _strAnimIn;
    [SerializeField] private string _strAnimOut;

    private void Awake()
    {
        GED.ED.addListener(EventID.OnStartStage, OnStartStage_Show);
        GED.ED.addListener(EventID.OnCompleteStage, OnCompleteStage_Hide);
    }

    private void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnStartStage, OnStartStage_Show);
        GED.ED.removeListener(EventID.OnCompleteStage, OnCompleteStage_Hide);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Hide()
    {
        _anim.Play(_strAnimOut);
        //transform.DOMove(StageManager.Singleton.tableHidePoint.position, 1f).OnComplete(() =>
        //{
        //    StageManager.Singleton.ShowWinEffect();
        //    GED.ED.dispatchEvent(EventID.OnHideGemTable);
        //});

    }

    public void OnAnimComplete(string animName)
    {
        if (animName == _strAnimIn)
        {
            GED.ED.dispatchEvent(EventID.OnEndAnimShowTable);
        }
        else if (animName == _strAnimOut)
        {
            StageManager.Singleton.ShowWinEffect();
            GED.ED.dispatchEvent(EventID.OnHideGemTable);
        }
        
    }

    private void OnStartStage_Show(GameEvent gameEvent)
    {
        gameObject.SetActive(true);
        _anim.Play(_strAnimIn);
    }

    private void OnCompleteStage_Hide(GameEvent gameEvent)
    {
        Hide();
    }
}
