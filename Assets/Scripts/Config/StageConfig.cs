using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageConfig", menuName = "Config/StageConfig", order = 1)]
public class StageConfig : BaseConfig
{
    public List<StageRecord> recordList;

    public Dictionary<int, StageRecord> recordMap;
    public Dictionary<string, StageRecord> recordMapByName;

    public override void CreateRecordMap()
    {
        recordMap = new Dictionary<int, StageRecord>();
        recordMapByName = new Dictionary<string, StageRecord>();
        foreach (var record in recordList)
        {
            if (!recordMap.ContainsKey(record.id))
            {
                recordMap.Add(record.id, record);
            }
            if (!recordMapByName.ContainsKey(record.name))
            {
                recordMapByName.Add(record.name, record);
            }
        }
    }
    public override BaseRecord GetRecordById(int id)
    {
        if (recordMap.ContainsKey(id))
            return recordMap[id];
        else
        {
            DevLog.Log("id is invalid: " + id);
            return null;
        }
    }

    public override BaseRecord GetRecordByName(string name)
    {
        if (recordMapByName.ContainsKey(name))
            return recordMapByName[name];
        else
        {
            DevLog.Log("name is invalid: " + name);
            return null;
        }
    }
}

[Serializable]
public class StageRecord : BaseRecord
{
    public GridData gridData;
    public GameObject tablePrefab;
}

[Serializable]
public class GridData
{
    public int width;
    public int height;
    public List<GemData> gemList;
}

[Serializable]
public class GemData
{
    public int gemId;
    public Vector2Int originCoor;


}