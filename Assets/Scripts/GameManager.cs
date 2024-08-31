using DG.Tweening;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public int numPickaxe { get; private set; } = 0;

    public float cellSize = 1;

    public Transform stageTrans;
    public Transform boardCenter;
    public Transform blockLayer;
    public Transform floorLayer;
    public Transform gemLayer;
    public GameObject cellPrefab;
    public GameObject floorPrefab;
    public Transform topLayer;
    public GameObject pickaxeUIPrefab;
    public Transform tableHidePoint;
    public Transform uiTopLayer;

    private GridData _gridData;
    private int _gridWidth;
    private int _gridHeight;

    private TableBehaviour _tableBehaviour;
    private int _numGemFound = 0;
    

    //private void Start()
    //{
    //    Initialize();
    //}

    //protected override void OnDestroy()
    //{
    //    base.OnDestroy();
    //}
    public void Initialize()
    {
        _gridData = ConfigLoader.GetRecord<StageRecord>(1).gridData;
        _gridWidth = _gridData.width;
        _gridHeight = _gridData.height;

        InitBlockLayer();
        InitGemLayer();
        InitFloorLayer();
        InitMissingGemTable();
        //InitStage();
        InitPickaxeNumUI();
    }

    //public void InitStage()
    //{
    //    var stagePrefab = ConfigLoader.GetRecord<StageRecord>(1).stagePrefab;
    //    var stageObject = Instantiate(stagePrefab);

    //    stageTrans = stageObject.transform;
    //}

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

    public void InitFloorLayer()
    {
        for (int i = 0; i < _gridHeight; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                var floorObject = Instantiate(floorPrefab, floorLayer);
                floorObject.name = "floor-" + i + "-" + j;
                floorObject.transform.position = IndexToPosition(i, j);
                //floorObject.GetComponent<BlockBehaviour>().index2D = new Vector2Int(i, j);
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

    public void InitMissingGemTable()
    {
        var tablePrefab = ConfigLoader.GetRecord<StageRecord>(1).tablePrefab;
        var tableObject = Instantiate(tablePrefab, stageTrans);
        _tableBehaviour = tableObject.GetComponent<TableBehaviour>();
    }

    public Vector2 IndexToPosition(int i, int j)
    {
        var xPos = boardCenter.position.x + cellSize * (j - _gridWidth / 2.0f + 0.5f);
        var yPos = boardCenter.position.y - cellSize * (i - _gridHeight / 2.0f + 0.5f);
        return new Vector2(xPos, yPos);
    }

    public void InitPickaxeNumUI()
    {
        var pickaxeNumUIObject = Instantiate(pickaxeUIPrefab, stageTrans);
        pickaxeNumUIObject.transform.position = new Vector2(0, boardCenter.position.y + cellSize * (_gridHeight / 2.0f));
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

    

    public void AddPickaxe()
    {
        numPickaxe += 1;
    }

    public void ConsumePickaxe()
    {
        numPickaxe -= 1;
    }

}


