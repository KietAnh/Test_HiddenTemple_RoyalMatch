using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RewardConfig", menuName = "Config/RewardConfig", order = 1)]
public class RewardConfig : BaseConfig
{
    public List<RewardRecord> recordList;

    public Dictionary<int, RewardRecord> recordMap;
    public Dictionary<string, RewardRecord> recordMapByName;

    public override void CreateRecordMap()
    {
        recordMap = new Dictionary<int, RewardRecord>();
        recordMapByName = new Dictionary<string, RewardRecord>();
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
public class RewardRecord : BaseRecord
{
    public RewardType type;
    public Sprite rewardSprite;
}

public enum RewardType { Coin = 1, Star, Pickaxe }