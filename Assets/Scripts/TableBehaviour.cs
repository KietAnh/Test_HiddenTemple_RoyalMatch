using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBehaviour : MonoBehaviour
{
    public List<Transform> missingGemList;

    private void Awake()
    {
        GED.ED.addListener(EventID.OnCompleteStage, OnCompleteStage_Hide);
    }

    private void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnCompleteStage, OnCompleteStage_Hide);
    }

    private void Hide()
    {
        transform.DOMove(GameManager.Singleton.tableHidePoint.position, 1f).OnComplete(() =>
        {
            GED.ED.dispatchEvent(EventID.OnHideGemTable);
        });
    }

    private void OnCompleteStage_Hide(GameEvent gameEvent)
    {
        Hide();
    }
}
