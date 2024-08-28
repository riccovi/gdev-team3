using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UIHandler))]
public class UIHandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector elements
        DrawDefaultInspector();

        // Get a reference to the ScaleObject script
        UIHandler scaleObject = (UIHandler)target;

        // Create a button that calls ActivatePopUp() when clicked
        if (GUILayout.Button("Activate Pop-Up"))
        {
            scaleObject.ActivatePopUp();
        }
    }
}

