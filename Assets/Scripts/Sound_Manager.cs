using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager instance;
    [Header("Audio Sources")]

    public AudioSource levelSoundHandler;
    public AudioSource PlayerSoundHandler_Loop;
    public AudioSource PlayerSoundHandler_Once;
    public AudioSource EnvironmentSoundHandler;

    [Header("MainThemes")]
    public AudioClip LevelSound;

    [Header("Sound Effects - Player")]
    public AudioClip Jump;
    public AudioClip Run;
    public AudioClip Run_Slow;
    public AudioClip DoubleJump;
    public AudioClip Climb;
    public AudioClip RangeAttack_getReady;
    public AudioClip RangeAttack_Release;
    public AudioClip RangeAttack_CallBack;
    public AudioClip MeleeAttack;
    public AudioClip Die;
    public AudioClip pickUpItem;
    public AudioClip pickUpUpgrade;

    [Header("Environment Effect")]
    public AudioClip MovingPlatform;
    public AudioClip FailingTools;
    public AudioClip Levier_Enable;
    public AudioClip Levier_Disable;
    public AudioClip BoxDestroy;
    public AudioClip DoorUnlock;

    [Header("Misc Effect")]
    public AudioClip Pause;
    public AudioClip UnPause;
    public AudioClip Deathscreen;
    public AudioClip CompleteLevel;

    private void Awake() {
        instance=this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void playerSoundOnce(string sound)
    {
        AudioSource TargetAudiosource=PlayerSoundHandler_Once;
        Debug.Log("Play sound once");

        playClip(TargetAudiosource,sound);

    }

    public void environmentSoundOnce(string sound)
    {
        AudioSource TargetAudiosource=EnvironmentSoundHandler;

        playClip(TargetAudiosource,sound);

    }

    public void playerSoundLoop(string sound)
    {
        AudioSource TargetAudiosource=PlayerSoundHandler_Loop;

        playClip(TargetAudiosource,sound);

    }

    public void playerSoundLoopStop()
    {
        AudioSource TargetAudiosource=PlayerSoundHandler_Loop;
        TargetAudiosource.Stop();
    }

    private void playClip(AudioSource TargetSource,string sound_name)
    {
        var sound =GetAudioClip(sound_name);

        //Play it Once , do not play if looping animation
        TargetSource.clip=sound;
        TargetSource.Play();
        
        
    }

    
    private void playerStopAllSounds()
    {
        PlayerSoundHandler_Loop.Stop();
        PlayerSoundHandler_Loop.Stop();
        
        
    }

    public AudioClip GetAudioClip( string variableName)
    {
        MonoBehaviour script = this;
        // Get the type of the script
        Type type = script.GetType();

        // Attempt to get the field by name
        FieldInfo fieldInfo = type.GetField(variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            // Return the value of the field as an AudioClip
            return fieldInfo.GetValue(script) as AudioClip;
        }

        // Attempt to get the property by name
        PropertyInfo propertyInfo = type.GetProperty(variableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (propertyInfo != null)
        {
            // Return the value of the property as an AudioClip
            return propertyInfo.GetValue(script) as AudioClip;
        }

        // If neither field nor property was found, return null
        Debug.LogWarning($"AudioClip '{variableName}' not found on script '{type.Name}'.");
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
