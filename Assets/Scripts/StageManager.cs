using DG.Tweening;
using UnityEngine;

public class StageManager : SingletonBehaviour<StageManager>
{
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
    //public Transform tableHidePoint;
    public Transform fireworkEffPoint;
    public GameObject congratulationText;
    public GameObject textErrorPrefab;

    private GridData _gridData;
    private int _gridWidth;
    private int _gridHeight;

    private TableBehaviour _tableBehaviour;
    private int _numGemFound;

    public bool blockInput { get; set; }

    public void Initialize()
    { 
        _gridData = ConfigLoader.GetRecord<StageRecord>(1).gridData;
        _gridWidth = _gridData.width;
        _gridHeight = _gridData.height;
        _numGemFound = 0;

        blockInput = true;

        InitBlockLayer();
        InitGemLayer();
        InitFloorLayer();
        InitMissingGemTable();
        InitPickaxeNumUI();
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
        pickaxeNumUIObject.transform.position = new Vector2(boardCenter.position.x, boardCenter.position.y + cellSize * (_gridHeight / 2.0f));
    }

    public void Slide()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            stageTrans.DOMove(Vector3.zero, 1f).OnComplete(() =>
            {
                GED.ED.dispatchEvent(EventID.OnStartStage);
            });
        });
        
    }


    public void MoveGemToTable(GemBehaviour gem)
    {
        Transform target = _tableBehaviour.missingGemList[gem.index];
        gem.transform.SetParent(topLayer, true);

        Sequence seq = DOTween.Sequence();
        seq.Append(gem.transform.DOScale(Vector3.one * 1.4f, 0.1f))
            .Append(gem.transform.DOScale(Vector3.one * 1.1f, 0.1f))
            .Append(gem.transform.DOScale(Vector3.one * 1.3f, 0.1f))
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                Vector3[] wayPoints = new Vector3[3];
                wayPoints[0] = gem.transform.position;
                wayPoints[1] = new Vector3((gem.transform.position.x + target.position.x) / 2, gem.transform.position.y - 1f);
                wayPoints[2] = target.position;
                gem.transform.DOPath(wayPoints, 0.75f, PathType.CatmullRom).OnComplete(() =>
                {
                    gem.transform.SetParent(_tableBehaviour.transform, true);
                    _numGemFound += 1;
                    if (_numGemFound == _tableBehaviour.missingGemList.Count)
                    {
                        // win
                        DOVirtual.DelayedCall(0.5f, () => GED.ED.dispatchEvent(EventID.OnCompleteStage));
                    }

                    AudioManager.Singleton.PlayEffect("fill-gem");
                }).SetEase(Ease.InQuart);
                gem.transform.DOScale(target.localScale, 0.75f).SetEase(Ease.InQuart);
            });
        
    }

    public void ShowWinEffect()
    {
        congratulationText.SetActive(true);
        AudioManager.Singleton.PlayEffect("win");
        EffectManager.Singleton.PlayEffect3D("firework", fireworkEffPoint.position, Quaternion.identity);
    }

    public void ShowTextError(string message)
    {
        var textErrorObject = Instantiate(textErrorPrefab, stageTrans);
        textErrorObject.transform.position = new Vector2(boardCenter.position.x, boardCenter.position.y + cellSize * (_gridHeight / 2.0f) + cellSize);
        textErrorObject.GetComponent<TextError>().SetContent(message);
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}