using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public float cellSize = 1;
    public int numGems = 4;

    public Transform stageTrans;
    public Transform gridCenter;
    public Transform blockLayer;
    public Transform gemLayer;
    public GameObject cellPrefab;
    public Transform topLayer;

    private GridData _gridData;
    private int _gridWidth;
    private int _gridHeight;

    private TableBehaviour _tableBehaviour;
    private int _numGemFound = 0;

    private void Start()
    {
        Initialize();

        GED.ED.addListener(EventID.OnDestroyBlock, OnDestroyBlock_CheckShowGem);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GED.ED.removeListener(EventID.OnDestroyBlock, OnDestroyBlock_CheckShowGem);
    }
    public void Initialize()
    {
        _gridData = ConfigLoader.GetRecord<StageRecord>(1).gridData;
        _gridWidth = _gridData.width;
        _gridHeight = _gridData.height;

        InitBlockLayer();
        InitGemLayer();
        InitMissingGemTable();
    }

    public void InitBlockLayer()
    {
        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                var cellObject = Instantiate(cellPrefab, blockLayer);
                cellObject.name = "block-" + i + "-" + j;
                cellObject.transform.position = IndexToPosition(i, j);
                cellObject.GetComponent<BlockBehaviour>().index2D = new Vector2Int(i, j);
            }
        }
    }

    public void InitGemLayer()  
    {
        foreach (var gemData in _gridData.gemList)
        {
            GameObject gemPrefab = ConfigLoader.GetRecord<GemRecord>(gemData.gemId).gemPrefab;
            var gemObject = Instantiate(gemPrefab, gemLayer);

            Transform pivotPoint = gemObject.transform.Find("pivot");
            Vector2 pivotLocalPos = new Vector2(pivotPoint.localPosition.x, pivotPoint.localPosition.y);
            Vector2 cellPos = IndexToPosition(gemData.originCoor.x, gemData.originCoor.y);
            gemObject.transform.position = cellPos - pivotLocalPos;

            var gemBehaviour = gemObject.GetComponent<GemBehaviour>();
            gemBehaviour.gemData = gemData;
            gemBehaviour.index = _gridData.gemList.IndexOf(gemData); 
            gemBehaviour.InitHiddenGemIndexList();
        }
    }

    public Vector2 IndexToPosition(int i, int j)
    {
        var xPos = gridCenter.position.x + cellSize * (j - _gridWidth / 2.0f + 0.5f);
        var yPos = gridCenter.position.y - cellSize * (i - _gridHeight / 2.0f + 0.5f);
        return new Vector2(xPos, yPos);
    }

    public void InitMissingGemTable()
    {
        var tablePrefab = ConfigLoader.GetRecord<StageRecord>(1).tablePrefab;
        var tableObject = Instantiate(tablePrefab, stageTrans);
        _tableBehaviour = tableObject.GetComponent<TableBehaviour>();
    }

    public void MoveGemToTable(GemBehaviour gem)
    {
        Transform target = _tableBehaviour.missingGemList[gem.index];
        gem.transform.SetParent(topLayer, true);
        gem.transform.DOScale(target.localScale, 1f);
        gem.transform.DOMove(target.position, 1f).OnComplete(() =>
        {
            gem.transform.SetParent(_tableBehaviour.transform, true);
            _numGemFound += 1;
            if (_numGemFound == _tableBehaviour.missingGemList.Count)
            {
                GED.ED.dispatchEvent(EventID.OnCompleteStage);
            }
        });
        
    }

    #region event

    private void OnDestroyBlock_CheckShowGem(GameEvent gameEvent)
    {
        var param = gameEvent.Data as OneParam<Vector2Int>;


    }

    #endregion
}


