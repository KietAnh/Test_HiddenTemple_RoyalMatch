
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockBehaviour : MonoBehaviour
{
    public Vector2Int index2D { get; set; }

    private void Awake()
    {
        GED.ED.addListener(EventID.OnCompleteStage, OnCompleteStage_SelfDestroy);
    }

    private void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnCompleteStage, OnCompleteStage_SelfDestroy);
    }

    void OnMouseDown()
    {
        if (StageManager.Singleton.blockInput)
            return;

        if (MainModel.numPickaxe > 0)
        {
            GED.ED.dispatchEvent(EventID.OnClickBlock, new OneParam<BlockBehaviour>(this));
        }
        else
        {
            StageManager.Singleton.ShowTextError("Not enough pickaxe");
            AudioManager.Singleton.PlayEffect("error");
        }
        
    }

    public void SelfDestroy()
    {
        AudioManager.Singleton.PlayEffect("break-block");
        EffectManager.Singleton.PlayEffect3D("block-break", transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnCompleteStage_SelfDestroy(GameEvent gameEvent)
    {
        SelfDestroy();
    }
}
