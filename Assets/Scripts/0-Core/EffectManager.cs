using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectManager : SingletonTemplate<EffectManager>
{
    private GameObject _effectUIGroup;

    private long _idxNow = 0;
    private Dictionary<long, EffectInfo> _effectPlayingMap = new Dictionary<long, EffectInfo>();
    private Dictionary<string, Stack<GameObject>> _effectsPool = new Dictionary<string, Stack<GameObject>>();   
    private const int MAX_FX_CACHE = 10;   // refactor, config cho tuwng effect

    public void Init()
    {
        _effectUIGroup = new GameObject();
        _effectUIGroup.name = "[EffectGroup]";
        GameObject.DontDestroyOnLoad(_effectUIGroup);
    }
    //public void PreloadEffects(string name)
    //{
    //    if (_effectsPool.ContainsKey(name))
    //        return;
    //    var pool = new Stack<GameObject>();
    //    for (int i = 0; i < MAX_FX_CACHE; i++)
    //    {
    //        GameObject effect = LoadEffect(name);       // refactor, performance
    //        effect.SetActive(false);
    //        pool.Push(effect);
    //    }
    //    if (!_effectsPool.ContainsKey(name))
    //    {
    //        _effectsPool.Add(name, pool);
    //    }
    //}
    public GameObject PlayEffectUI(string name, Vector3 pos, Quaternion rot, Transform parent, Action callback = null, bool isCache = true)
    {
        GameObject effect = _PlayEffect(name, EffectType.FxUI, callback, isCache);
        effect.transform.position = pos;
        effect.transform.rotation = rot;
        effect.transform.SetParent(parent, true);
        effect.transform.localScale = Vector3.one;
        effect.SetActive(true);
        //effect.GetComponent<ParticleSystem>().Play();
        return effect;
    }
    public GameObject PlayEffect3D(string name, Vector3 pos, Quaternion rot, Action callback = null, bool isCache = true)
    {
        GameObject effect = _PlayEffect(name, EffectType.Fx3D, callback, isCache);
        effect.transform.position = pos;
        effect.transform.rotation = rot;
        effect.SetActive(true);
        //effect.GetComponent<ParticleSystem>().Play();
        return effect;
    }
    private GameObject _PlayEffect(string name, EffectType type, Action callback = null, bool isCache = true)
    {
        long id = _NewId();
        EffectInfo effect = new EffectInfo();
        var effectObj = GetEffect(name);
        effect.gameObj = effectObj;
        effect.name = name;
        effect.type = type;
        effect.isPlaying = true;
        effect.cmpCb = callback;
        effect.duration = effect.gameObj.GetComponent<ParticleSystem>().main.duration;
        effect.isCache = isCache;

        _effectPlayingMap.Add(id, effect);

        DOVirtual.DelayedCall(effect.duration + 0.1f, () =>
        {
            StopEffect(id);
        });

        return effectObj;
    }

    public GameObject GetEffect(string name)
    {
        GameObject effectObj = null;
        if (_effectsPool.ContainsKey(name) && _effectsPool[name].Count > 0)
        {
            effectObj = _effectsPool[name].Pop();
        }
        else
        {
            //effectObj = LoadEffect(name);
            var effectPrefab = ConfigLoader.GetRecord<EffectRecord>(name).prefab;
            effectObj = GameObject.Instantiate(effectPrefab, _effectUIGroup.transform);
        }
        return effectObj;
    }
    //public GameObject LoadEffect(string name)
    //{
    //    string abName = string.Format("vfx_{0}", name).ToLower();
    //    var fxObj = AssetLoadManager.LoadAsset<GameObject>(abName, name);
    //    if (fxObj != null)
    //    {
    //        GameObject fx = GameObject.Instantiate(fxObj, _effectUIGroup.transform);
    //        fx.name = name;
    //        return fx;
    //    }
    //    return null;
    //}
    public void StopEffect(long id)
    {
        if (!_effectPlayingMap.ContainsKey(id))
            return;

        EffectInfo effect = _effectPlayingMap[id];
        effect.isPlaying = false;
        if (effect.isCache)
        {
            string name = effect.name;
            if (!_effectsPool.ContainsKey(name))
            {
                var pool = new Stack<GameObject>();
                _effectsPool.Add(name, pool);
            }
            if (_effectsPool[name].Count < MAX_FX_CACHE)
            {
                _effectsPool[name].Push(effect.gameObj);
            }
            effect.gameObj.SetActive(false);

            if (effect.type == EffectType.FxUI)
            {
                effect.gameObj.transform.SetParent(_effectUIGroup.transform, false);
                effect.gameObj.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            GameObject.Destroy(effect.gameObj);
        }

        if (effect.cmpCb != null)
        {
            effect.cmpCb();
        }

        _effectPlayingMap.Remove(id);
    }
    private long _NewId()
    {
        while (true)
        {
            _idxNow++;
            if (_idxNow < 0)        // case overflow kieu long
                _idxNow = 1;
            if (!_effectPlayingMap.ContainsKey(_idxNow))
                return _idxNow;
        }
    }
}

public class EffectInfo
{
    public string name;
    public GameObject gameObj;
    public EffectType type;
    public float duration;
    public Action cmpCb;
    public bool isPlaying;
    public bool isCache;
}
public enum EffectType
{
    Fx3D,
    FxUI,
}