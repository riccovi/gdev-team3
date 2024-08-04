using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableObject : MonoBehaviour
{
    public string ID;

    private void Start() {
        ID=gameObject.name;
    }
}