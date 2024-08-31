using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardUI : MonoBehaviour
{
    [SerializeField] private Image _imgReward;
    [SerializeField] private TextMeshProUGUI _textCount;

    private RewardInfo _rewardInfo;

    public void Init(RewardInfo rewardInfo)
    {
        _rewardInfo = rewardInfo;

        _imgReward.sprite = ConfigLoader.GetRecord<RewardRecord>((int)rewardInfo.type).rewardSprite;
        //_imgReward.SetNativeSize();
        _textCount.text = "+" + _rewardInfo.quantity.ToString();
    }

    public void OnAnimationComplete_Hide()
    {
        Destroy(gameObject);
    }
}
