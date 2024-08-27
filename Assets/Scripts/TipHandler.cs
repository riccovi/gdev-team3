using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipHandler : MonoBehaviour
{
    public TipsScriptableObject data;
    public TipsManager tipManager;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        tipManager=transform.parent.GetComponent<TipsManager>();
        data = tipManager.getData(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(isActive)
        {
            if(data.tipType==TipsScriptableObject.TipType.OnTrigger && other.CompareTag("Player"))
            {
                Debug.Log("ActivateTip");
                UIHandler.instance.ActivatePopUp(data.Description);
                isActive=false;
            }

        }
        
        
    }

    
}
