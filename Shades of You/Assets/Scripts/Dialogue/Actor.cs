using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Actor", menuName = "Dialogue/Actor")]
public class Actor : ScriptableObject
{
    public string actorName;
    public Image portrait;
}