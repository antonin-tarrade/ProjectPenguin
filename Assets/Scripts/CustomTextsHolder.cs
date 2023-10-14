using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class CustomTextsHolder : MonoBehaviour
{
    public CustomTexts customTexts;
    IEnumerator<string> texts;
    TMP_Text textZone;

    private void Awake()
    {
        textZone = GetComponent<TextMeshPro>();
    }


    public void Read(CustomTexts textsRef)
    {
        texts = (IEnumerator<string>)textsRef.GetEnumerator();
        ReadNext();
    }

    public void Read()
    {
        Read(customTexts);
    }

    public void ReadNext()
    {
        texts.MoveNext();
        textZone.text = texts.Current;
    }
}
