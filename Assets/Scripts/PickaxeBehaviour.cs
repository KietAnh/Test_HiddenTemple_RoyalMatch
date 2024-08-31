using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeBehaviour : MonoBehaviour
{
    public BlockBehaviour blockOnClick { get; set; }

    private void Awake()
    {
        GED.ED.addListener(EventID.OnClickBlock, OnClickBlock_PlayAnimation);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GED.ED.removeListener(EventID.OnClickBlock, OnClickBlock_PlayAnimation);
    }

    private void OnClickBlock_PlayAnimation(GameEvent gameEvent)
    {
        var param = gameEvent.Data as OneParam<BlockBehaviour>;
        blockOnClick = param.value1;
        transform.position = blockOnClick.transform.position;
        gameObject.SetActive(true);
        GetComponent<Animation>().Play();
    }

    public void OnAnimComplete_DestroyBlock()
    {
        if (blockOnClick == null)
            return;

        GameManager.Singleton.ConsumePickaxe();
        GED.ED.dispatchEvent(EventID.OnUpdatePickaxe);

        var param = new OneParam<Vector2Int>(blockOnClick.index2D);
        GED.ED.dispatchEvent(EventID.OnDestroyBlock, param);

        blockOnClick.SelfDestroy();
        gameObject.SetActive(false);
    }

}
