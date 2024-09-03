using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickaxeNumUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumPickaxe;
    [SerializeField] private Animation _anim;
    [SerializeField] private string _strAnimIn;
    [SerializeField] private string _strAnimOut;
    private void Awake()
    {
        GED.ED.addListener(EventID.OnEndAnimShowTable, OnEndAnimShowTable_Show);
        GED.ED.addListener(EventID.OnUpdatePickaxe, OnUpdatePickaxe_RefreshView);
        GED.ED.addListener(EventID.OnCompleteStage, OnCompleteStage_SelfDestroy);
    }

    private void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnEndAnimShowTable, OnEndAnimShowTable_Show);
        GED.ED.removeListener(EventID.OnUpdatePickaxe, OnUpdatePickaxe_RefreshView);
        GED.ED.removeListener(EventID.OnCompleteStage, OnCompleteStage_SelfDestroy);
    }

    private void Start()
    {
        RefreshView();
        gameObject.SetActive(false);
    }

    private void RefreshView()
    {
        _textNumPickaxe.text = MainModel.numPickaxe.ToString();
    }

    public void OnClickButtonAddPickaxe()
    {
        MainModel.AddPickaxe();
        GED.ED.dispatchEvent(EventID.OnUpdatePickaxe);
    }

    public void OnAnimComplete(string animName)
    {
        if (animName == _strAnimIn)
        {
            StageManager.Singleton.blockInput = false;
        }
    }

    private void OnEndAnimShowTable_Show(GameEvent gameEvent)
    {
        gameObject.SetActive(true);
        _anim.Play(_strAnimIn);
    }

    private void OnUpdatePickaxe_RefreshView(GameEvent gameEvent)
    {
        RefreshView();
    }

    private void OnCompleteStage_SelfDestroy(GameEvent gameEvent)
    {
        //Destroy(gameObject);
        _anim.Play(_strAnimOut);
    }
}
