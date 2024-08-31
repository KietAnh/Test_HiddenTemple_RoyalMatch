
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
        if (GameManager.Singleton.numPickaxe > 0)
        {
            GED.ED.dispatchEvent(EventID.OnClickBlock, new OneParam<BlockBehaviour>(this));
        }
        else
        {
            DevLog.Log("TODO: Notify not enough pickaxe");
        }
        
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
        // instantiate vfx here...
    }

    private void OnCompleteStage_SelfDestroy(GameEvent gameEvent)
    {
        SelfDestroy();
    }
}
