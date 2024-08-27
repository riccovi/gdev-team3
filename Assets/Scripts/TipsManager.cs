using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class TipsManager : MonoBehaviour
{
    public List<TipsScriptableObject> TipList ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TipsScriptableObject getData(string name)
    {
        foreach(TipsScriptableObject tip in TipList)
        {
            if(tip.name==name)
            {
                return tip;
            }
        }

        return null;
    }
}
