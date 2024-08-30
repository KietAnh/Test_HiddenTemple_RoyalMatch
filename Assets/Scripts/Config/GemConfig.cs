using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemConfig", menuName = "Config/GemConfig", order = 1)]
public class GemConfig : BaseConfig
{
    public List<GemRecord> recordList;

    public Dictionary<int, GemRecord> recordMap;
    public Dictionary<string, GemRecord> recordMapByName;

    public override void CreateRecordMap()
    {
        recordMap = new Dictionary<int, GemRecord>();
        recordMapByName = new Dictionary<string, GemRecord>();
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
public class GemRecord : BaseRecord
{
    public int width;
    public int height;
    public GameObject gemPrefab;
}