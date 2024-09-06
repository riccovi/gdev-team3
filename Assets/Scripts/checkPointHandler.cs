using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointHandler : MonoBehaviour
{
    public PositionManager positionManager;
    public Vector3 position;
    // Start is called before the first frame update
    public int currentHealth;

    public TipsScriptableObject tipData;

    public TipsScriptableObject HelpTip;

    public Transform graphics;

    public int respawnTimes;

    [HideInInspector]public bool isIgognito;

    [HideInInspector]public bool isActivatedByPlayer;
    void Start()
    {
        respawnTimes=0;
        positionManager=transform.parent.GetComponent<PositionManager>();
        position=transform.position;

        if(isIgognito)
        {
            graphics.gameObject.SetActive(false);
        }
        else
        {
            graphics.gameObject.SetActive(true);
        }
    }

    public void respawn()
    {
        respawnTimes++;
        if(respawnTimes>1)
        {
            UIHandler.instance.ActivatePopUp(HelpTip.Description);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {

        if(!isActivatedByPlayer)
        {
            if(other.CompareTag("Player"))
            {
                if(!positionManager.checkPoints.Contains(this))
                {
                    positionManager.checkPoints.Add(this);
                }
                
                if(tipData!=null && !isIgognito)
                {
                    UIHandler.instance.ActivatePopUp(tipData.Description);
                    currentHealth=other.GetComponent<PlayStats>().CurrentHealth;
                }

                isActivatedByPlayer=true;
            }
        }        
    }
}
