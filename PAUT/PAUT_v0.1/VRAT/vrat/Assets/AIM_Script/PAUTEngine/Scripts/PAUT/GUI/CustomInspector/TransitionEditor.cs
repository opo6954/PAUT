using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
namespace PAUT
{
    [CustomEditor(typeof(Transition))]
    public class TransitionEditor : Editor
    {
        /*
        public override void OnInspectorGUI()
        {
            Transition myTarget = (Transition)target;
            
            EditorGUILayout.BeginHorizontal();
            //myTarget.name = EditorGUILayout.TextField("Name", "Asset name");
            //myTarget.targetTriggerArgs = (TriggerEventArgs)EditorGUILayout.ObjectField("Asset",myTarget.targetTriggerArgs, typeof((TriggerEventArgs)), true);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Search!"))
            {
                
            }
            //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
        }
        */
    }
}
#endif

