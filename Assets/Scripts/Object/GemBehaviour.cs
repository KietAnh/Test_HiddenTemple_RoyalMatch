using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemBehaviour : MonoBehaviour
{
    public GemData gemData { get; set; }
    public int index { get; set; }

    public List<Vector2Int> hiddenGemIndexList { get; private set; } // chứa các tọa độ của phần gem bị che dưới block

    private void Start()
    {
        GED.ED.addListener(EventID.OnDestroyBlock, OnDestroyBlock_CheckCompleteGem);
    }

    private void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnDestroyBlock, OnDestroyBlock_CheckCompleteGem);
    }

    public void InitHiddenGemIndexList()
    {
        hiddenGemIndexList = new List<Vector2Int>();
        var gemRecord = ConfigLoader.GetRecord<GemRecord>(gemData.gemId);
        int gemWidth = gemRecord.width;
        int gemHeight = gemRecord.height;
        for (int i = 0; i < gemHeight; i++)
        {
            for (int j = 0; j < gemWidth; j++)
            {
                hiddenGemIndexList.Add(gemData.originCoor + new Vector2Int(i, j));
            }
        }

        //string str = string.Empty;
        //foreach (var item in hiddenGemIndexList)
        //{
        //    str += "(" + item.x + "," + item.y + "),";
        //}
        //DevLog.Log(str);
    }

    #region event

    private void OnDestroyBlock_CheckCompleteGem(GameEvent gameEvent)
    {
        var param = gameEvent.Data as OneParam<Vector2Int>;
        Vector2Int index2D = param.value1;
        if (hiddenGemIndexList.Contains(index2D))
        {
            hiddenGemIndexList.Remove(index2D);
            if (hiddenGemIndexList.Count == 0)
            {
                StageManager.Singleton.MoveGemToTable(this);

                AudioManager.Singleton.PlayEffect("find-gem");
            }
        }
    }

    #endregion
}
