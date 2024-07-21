using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Mathematics;
using UnityEngine;
using VSCodeEditor;

public class Wrench : MonoBehaviour
{
    public float rotateSpead;
    public bool isRotating;

    public float moveSpeed;
    public float CallBackSpeed;
    public Vector3 targetPosition;

    public Camera mainCamera;

    private bool isClicked;

    private bool isDamaged=false;
    public bool CanCallBack=false;

    private bool returnWrench;

    public Transform wrenchHolder;

    public Quaternion origRotation;
    public Vector3 origPos;
    //Use trigger to check damage
    

    // Start is called before the first frame update
    void Start()
    {
        origRotation=transform.localRotation;
        origPos=transform.localPosition;
        mainCamera=Camera.main;
        wrenchHolder=transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        selfRotation();

        if (Input.GetMouseButtonDown(0) && !CanCallBack)
        {
            isClicked = true;
            targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            targetPosition.z = 0; // Ensure z is 0 since it's a 2D plane

            transform.parent=null;
            StartCoroutine(ThrowWrenchCoroutine(targetPosition));
        }

        if (Input.GetMouseButtonDown(0) && CanCallBack)
        {
            isDamaged = true;
            returnWrench = true;
        }

        if (returnWrench)
        {
            StartCoroutine(ReturnWrenchCoroutine());
            returnWrench = false;
        }
    }

    public void selfRotation()
    {
        if(isRotating)
        {
            transform.Rotate(0,0,rotateSpead*Time.deltaTime);
        }
        else
        {
            transform.Rotate(0,0,0);
        }
        
    }



    private IEnumerator ThrowWrenchCoroutine(Vector3 targetPosition)
    {
        isRotating = true;

        while (Vector2.Distance(transform.position, targetPosition) > 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.parent = null;
        isRotating = false;
        isDamaged = false;
        CanCallBack = true;
        isClicked = false;
    }

    private IEnumerator ReturnWrenchCoroutine()
    {
        isRotating = true;

        while (Vector2.Distance(transform.position, wrenchHolder.position) > 0.5f)
        {
            transform.position = Vector2.MoveTowards(transform.position, wrenchHolder.position, CallBackSpeed * Time.deltaTime);
            yield return null;
        }

        transform.parent = wrenchHolder;
        transform.localPosition = Vector3.zero;
        transform.localRotation = origRotation;

        Debug.Log("Rotation reset to: " + origRotation);

        isRotating = false;
        CanCallBack = false;
        isDamaged = false;
        isClicked = false;
    }

    private void ReturnWrench()
    {        
        isRotating=true;
        transform.position = Vector2.MoveTowards(transform.position,wrenchHolder.position,CallBackSpeed*Time.deltaTime);
    }
    public void ThrowWrench()
    {           
        CanCallBack=false;
        isDamaged=true;
        isRotating=true;

        transform.position = Vector2.MoveTowards(transform.position,targetPosition,moveSpeed*Time.deltaTime);
    }

}
