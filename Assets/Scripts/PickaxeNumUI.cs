using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickaxeNumUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textNumPickaxe;
    private void Awake()
    {
        GED.ED.addListener(EventID.OnUpdatePickaxe, OnUpdatePickaxe_RefreshView);
        GED.ED.addListener(EventID.OnCompleteStage, OnCompleteStage_SelfDestroy);
    }

    private void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnUpdatePickaxe, OnUpdatePickaxe_RefreshView);
        GED.ED.removeListener(EventID.OnCompleteStage, OnCompleteStage_SelfDestroy);
    }

    private void Start()
    {
        RefreshView();
    }

    private void RefreshView()
    {
        _textNumPickaxe.text = GameManager.Singleton.numPickaxe.ToString();
    }

    public void OnClickButtonAddPickaxe()
    {
        GameManager.Singleton.AddPickaxe();

        GED.ED.dispatchEvent(EventID.OnUpdatePickaxe);
    }

    private void OnUpdatePickaxe_RefreshView(GameEvent gameEvent)
    {
        RefreshView();
    }

    private void OnCompleteStage_SelfDestroy(GameEvent gameEvent)
    {
        Destroy(gameObject);
    }
}
