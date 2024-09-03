using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ChestBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _showPoint;
    [SerializeField] private List<RewardInfo> _rewardList;
    [SerializeField] private GameObject _rewardUIPrefab;
    [SerializeField] private Sprite _spriteChestOpen;
    [SerializeField] private List<Transform> _rewardStartPointList;
    private bool _enableClick;
    private void Awake()
    {
        GED.ED.addListener(EventID.OnHideGemTable, OnHideGemTable_Show);
    }

    private void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnHideGemTable, OnHideGemTable_Show);
    }

    private void OnMouseDown()
    {
        if (_enableClick)
        {
            Open();
        }
    }

    private void Open()
    {
        _enableClick = false;
        GetComponent<SpriteRenderer>().sprite = _spriteChestOpen;
        foreach (var rewardInfo in _rewardList)
        {
            int index = _rewardList.IndexOf(rewardInfo);
            DOVirtual.DelayedCall(0.25f * index, () =>
            {
                var rewardUIObject = Instantiate(_rewardUIPrefab);
                rewardUIObject.transform.position = _rewardStartPointList[index].position;
                var rewardUI = rewardUIObject.GetComponent<RewardUI>();
                rewardUI.Init(rewardInfo);

                if (rewardInfo.type == RewardType.Pickaxe)
                {
                    MainModel.AddPickaxe(rewardInfo.quantity);
                    GED.ED.dispatchEvent(EventID.OnUpdatePickaxe);
                }

                AudioManager.Singleton.PlayEffect("reward");
            });
        }

        DOVirtual.DelayedCall(1.0f, () => UIManager.Singleton.ShowReplayButton());
    }

    private void Show()
    {
        transform.DOMove(_showPoint.position, 1f).OnComplete(() =>
        {
            _enableClick = true;

            AudioManager.Singleton.PlayEffect("chest-fall");
        });
    }

    private void OnHideGemTable_Show(GameEvent gameEvent)
    {
        Show();
    }

}

[Serializable]
public struct RewardInfo
{
    public RewardType type;
    public int quantity;
}