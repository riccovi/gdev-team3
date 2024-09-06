using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class cameraShake : MonoBehaviour
{
    public static cameraShake instance;
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public enum ShakeType
    {
        Easy,
        Medium,
        Hard
    }

    [Header("Easy")]
    public float shakeIntensity_Easy=0.8f;
    public float shakeTime_Easy=0.15f;
    // Start is called before the first frame update

    [Header("Medium")]

    public float shakeIntensity_Medium=1.5f;
    public float shakeTime_Medium=0.22f;

    [Header("Hard")]

    public float shakeIntensity_Hard=2f;
    public float shakeTime_Hard=0.30f;

    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private IEnumerator shakeCo;

    private void Awake() {
        cinemachineVirtualCamera=GetComponent<CinemachineVirtualCamera>();
        instance=this;
    }
    void Start()
    {
        StopShake();
    }

    public void ShakeCamera(string EasyMediumHard)
    {
        var shakeIntensity=0.0f;
        if(EasyMediumHard == "Easy")
        {
            shakeIntensity=shakeIntensity_Easy;
        }
        else if(EasyMediumHard == "Medium")
        {
            shakeIntensity=shakeIntensity_Medium;
        }
        else
        {
            shakeIntensity=shakeIntensity_Hard;
        }
        _cbmcp=cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain=shakeIntensity;

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartShakeCoroutine(string EasyMediumHard)
    {
        if(shakeCo==null)
        {
            shakeCo=ShakeCoroutine(EasyMediumHard);
            StartCoroutine(shakeCo);
        }
        else
        {
            StopCoroutine(shakeCo);
            shakeCo=ShakeCoroutine(EasyMediumHard);
            StartCoroutine(shakeCo);
        }
    }

    private IEnumerator ShakeCoroutine(string EasyMediumHard)
    {
        ShakeCamera(EasyMediumHard);

        var shakeTime=0.0f;
        if(EasyMediumHard == "Easy")
        {
            shakeTime=shakeTime_Easy;
        }
        else if(EasyMediumHard == "Medium")
        {
            shakeTime=shakeTime_Medium;
        }
        else
        {
            shakeTime=shakeTime_Hard;
        }

        yield return new WaitForSeconds(shakeTime);

        StopShake();
        
        shakeCo=null;
    }

    public void StopShake()
    {
         _cbmcp=cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain=0f;

    }
}
