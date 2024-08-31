using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntroScreen : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SelfDestroy();
        GameManager.Singleton.Initialize();
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
