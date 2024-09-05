using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeTrigger : Destractable
{
    public Animator hideRope;
    public Rigidbody2D HangingObject;
    // Start is called before the first frame update
    void Start()
    {
        HangingObject.gravityScale=0;
        HangingObject.bodyType=RigidbodyType2D.Static;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void doDamage(int damage)
    {
        HangingObject.transform.SetParent(null);
        HangingObject.gravityScale=1;
        HangingObject.bodyType=RigidbodyType2D.Dynamic;
        HangingObject.freezeRotation=true;

        hideRope.SetTrigger("HideRope");
    }
}
