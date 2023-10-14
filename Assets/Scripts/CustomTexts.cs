using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomTexts", menuName = "Scriptables/CustomTexts", order = 2)]
public class CustomTexts : ScriptableObject, IEnumerable
{

    public List<string> texts = new List<string>();

    public IEnumerator GetEnumerator()
    {
        return ((IEnumerable)texts).GetEnumerator();
    }
}
