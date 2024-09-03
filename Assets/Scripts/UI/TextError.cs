using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextError : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    public void SetContent(string message)
    {
        _text.text = message;
    }

    public void OnAnimComplete()
    {
        Destroy(gameObject);
    }
}
