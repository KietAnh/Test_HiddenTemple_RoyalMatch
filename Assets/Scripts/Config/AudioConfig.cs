using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioConfig", menuName = "Config/AudioConfig", order = 1)]
public class AudioConfig : BaseConfig
{
    public List<AudioRecord> recordList;

    public Dictionary<int, AudioRecord> recordMap;
    public Dictionary<string, AudioRecord> recordMapByName;

    public override void CreateRecordMap()
    {
        recordMap = new Dictionary<int, AudioRecord>();
        recordMapByName = new Dictionary<string, AudioRecord>();
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
public class AudioRecord : BaseRecord
{
    public AudioClip clip;
}