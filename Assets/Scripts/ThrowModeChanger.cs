using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowModeChanger : MonoBehaviour
{
    public playerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player=transform.parent.GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void swapRunAnimation()
    {
        player.ReplaceRunAnimationIdle();
    }
    
    public void UnloadThorwMode()
    {
        player.ReplaceRunAnimationIdle();
        player.ThrowMode=false;
    }

    public void stopAttack()
    {
        player.attackAnim=false;
    }
}
