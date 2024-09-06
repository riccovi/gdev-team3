using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    public static PositionManager instance;
    public List<checkPointHandler> checkPoints;
    // Start is called before the first frame 
    
    private void Awake() {
        instance=this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public checkPointHandler GetLastCheckpoint()
    {
        if (checkPoints != null && checkPoints.Count > 0)
        {
            return checkPoints[checkPoints.Count - 1];
        }
        return null;
    }
}
