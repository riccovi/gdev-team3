using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool Activate;
    public Transform PosA;
    public Transform PosB;
    public Vector2 targetPos;
    public int Speed;

    [Tooltip("Platform will start from position B")]
    public Transform Platform;
    public Transform CollisionDetection;

    public float yOffsetColDetect=1;

    
    // Start is called before the first frame update
    void Start()
    {
        targetPos=PosB.position;
        Platform.position=PosB.position;

        var detectVector = new Vector2(PosB.position.x,PosB.position.y+yOffsetColDetect);
        CollisionDetection.position=PosB.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Activate)
        {
            if(Vector2.Distance(Platform.transform.position,PosA.position)<0.1f) targetPos = PosB.position;
            if(Vector2.Distance(Platform.transform.position,PosB.position)<0.1f) targetPos = PosA.position;
        
            Platform.transform.position = Vector2.MoveTowards(Platform.transform.position,targetPos,Speed*Time.deltaTime);
             var detectVector = new Vector2(Platform.transform.position.x,Platform.transform.position.y+yOffsetColDetect);
            CollisionDetection.position= Vector2.MoveTowards(Platform.transform.position,targetPos,Speed*Time.deltaTime);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(PosA.position,PosB.position);
        
    }

}
