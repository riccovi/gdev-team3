using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
 
    public bool MoveOnceStartToEnd;
    public bool Activate;
    public Transform StartPos;
    public Transform EndPos;
    public Vector2 targetPos;

    private int CurrentSpeed;
    public int MaxSpeed;

    [Tooltip("Platform will start from position B")]
    public Transform Platform;
    public Transform CollisionDetection;

    public float yOffsetColDetect=1;

    public AudioSource audiosource;

    
    // Start is called before the first frame update
    void Start()
    {
        if(Activate)
        {
            CurrentSpeed=MaxSpeed;
            startMovement();
        }
        else
        {
            StopMovement();
        }
        targetPos=StartPos.position;
        Platform.position=StartPos.position;

        CollisionDetection.position=StartPos.position;
    }

    public void startMovement()
    {
        audiosource.Play();
        Activate=true;
        Debug.Log("startMovement");
        CurrentSpeed=MaxSpeed;

        if(audiosource!=null)
        {
            audiosource.Play();
        }

    }

    public void StopMovement()
    {
        audiosource.Stop();
        Debug.Log("stopMovement");
        Activate=false;
        CurrentSpeed=0;
        if(audiosource!=null)
        {
            audiosource.Stop();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Activate)
        {
            
            if(MoveOnceStartToEnd)
            {
                if(Vector2.Distance(Platform.transform.position,EndPos.position)<0.1f)
                {  
                    Debug.Log(Vector2.Distance(Platform.transform.position,EndPos.position));                  
                    Activate=false;
                    Debug.Log("stop");
                    return;
                }
                Debug.Log(Vector2.Distance(Platform.transform.position,EndPos.position)); 
                Platform.transform.position = Vector2.MoveTowards(Platform.transform.position,EndPos.position,CurrentSpeed*Time.deltaTime);
                CollisionDetection.position= Vector2.MoveTowards(Platform.transform.position,EndPos.position,CurrentSpeed*Time.deltaTime);
            }
            else
            {
                if(Vector2.Distance(Platform.transform.position,StartPos.position)<0.1f) 
                {
                    targetPos = EndPos.position;                
                }
                if(Vector2.Distance(Platform.transform.position,EndPos.position)<0.1f)
                {
                    targetPos = StartPos.position;   
                } 
            
                Platform.transform.position = Vector2.MoveTowards(Platform.transform.position,targetPos,CurrentSpeed*Time.deltaTime);
                // var detectVector = new Vector2(Platform.transform.position.x,Platform.transform.position.y+yOffsetColDetect);
                CollisionDetection.position= Vector2.MoveTowards(Platform.transform.position,targetPos,CurrentSpeed*Time.deltaTime);
            }         
 
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(StartPos.position,EndPos.position);
        
    }

}
