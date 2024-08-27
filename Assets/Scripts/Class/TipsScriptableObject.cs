using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Tip", order = 1)]
 [System.Serializable]
public class TipsScriptableObject : ScriptableObject
{
    [TextArea(15,20)]
    public string Description = "";

    public enum TipType
    {
        OnTrigger,
        OnPickup,
        Dialogue,
        interactable
    }

    public TipType tipType;

}
