using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectConfig", menuName = "Config/EffectConfig", order = 1)]
public class EffectConfig : BaseConfig
{
    public List<EffectRecord> recordList;

    public Dictionary<int, EffectRecord> recordMap;
    public Dictionary<string, EffectRecord> recordMapByName;

    public override void CreateRecordMap()
    {
        recordMap = new Dictionary<int, EffectRecord>();
        recordMapByName = new Dictionary<string, EffectRecord>();
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
public class EffectRecord : BaseRecord
{
    public GameObject prefab;
}