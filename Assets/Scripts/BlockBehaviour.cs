
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
        var param = new OneParam<Vector2Int>(index2D);
        GED.ED.dispatchEvent(EventID.OnDestroyBlock, param);

        SelfDestroy();
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
        // instantiate vfx here...
    }

    private void OnCompleteStage_SelfDestroy(GameEvent gameEvent)
    {
        SelfDestroy();
    }
}
