using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR) 
namespace TFM.Actions
{
    /// <summary>
    /// Class <c>ActionSequenceEditor</c> is a custom editor for the <c>ActionSequence</c> class.
    /// </summary>
    [CustomEditor(typeof(ActionSequence))]
    public class ActionSequenceEditor : Editor
    {
        /// <value>Property <c>_actionsProp</c> represents the sequence actions property.</value>
        private SerializedProperty _actionsProp;

        /// <summary>
        /// Method <c>OnEnable</c> is called when the object is loaded.
        /// </summary>
        private void OnEnable()
        {
            _actionsProp = serializedObject.FindProperty("sequenceActions");
        }

        /// <summary>
        /// Method <c>OnInspectorGUI</c> draws the inspector GUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
            
            for (var i = 0; i < _actionsProp.arraySize; i++)
            {
                var actionProp = _actionsProp.GetArrayElementAtIndex(i);
                if (actionProp?.managedReferenceValue == null)
                    continue;

                var actionType = actionProp.managedReferenceFullTypename;
                var actionName = actionType.Substring(actionType.LastIndexOf('.') + 1);
                
                switch (actionName)
                {
                    case "ShowDialogueAction":
                        EditorGUILayout.LabelField("Show Dialogue Action", EditorStyles.boldLabel);
                        break;
                    case "HideDialogueAction":
                        EditorGUILayout.LabelField("Hide Dialogue Action", EditorStyles.boldLabel);
                        break;
                    case "SceneChangeAction":
                        EditorGUILayout.LabelField("Scene Change Action", EditorStyles.boldLabel);
                        break;
                }

                if (actionType.EndsWith("ShowDialogueAction"))
                {
                    var actorProp = actionProp.FindPropertyRelative("actor");
                    var dialogueLineProp = actionProp.FindPropertyRelative("dialogueLine");
                    var positionProp = actionProp.FindPropertyRelative("position");
                    EditorGUILayout.PropertyField(actorProp, new GUIContent("Actor"));
                    EditorGUILayout.PropertyField(dialogueLineProp, new GUIContent("Dialogue Line"));
                    positionProp.intValue = EditorGUILayout.Popup(positionProp.displayName, positionProp.intValue, new[] {"Left", "Right"});
                }
                else if (actionType.EndsWith("SceneChangeAction"))
                {
                    var sceneProp = actionProp.FindPropertyRelative("sceneName");
                    var fadeOverlayProp = actionProp.FindPropertyRelative("fadeOverlay");
                    EditorGUILayout.PropertyField(sceneProp, new GUIContent("Scene Name"));
                    fadeOverlayProp.intValue = EditorGUILayout.Popup(fadeOverlayProp.displayName, fadeOverlayProp.intValue, new[] {"No", "Yes"});
                }
                var waitForInputProp = actionProp.FindPropertyRelative("waitForInput");
                waitForInputProp.intValue = EditorGUILayout.Popup(waitForInputProp.displayName, waitForInputProp.intValue, new[] {"No", "Yes"});

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Remove Action"))
                {
                    _actionsProp.DeleteArrayElementAtIndex(i);
                    continue;
                }

                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Show Dialogue Action"))
            {
                AddAction<ShowDialogueAction>();
            }
            
            if (GUILayout.Button("Add Hide Dialogue Action"))
            {
                AddAction<HideDialogueAction>();
            }

            if (GUILayout.Button("Add Scene Change Action"))
            {
                AddAction<SceneChangeAction>();
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Method <c>AddAction</c> adds an action to the sequence.
        /// </summary>
        /// <typeparam name="T">The action type.</typeparam>
        private void AddAction<T>() where T : ActionBase, new()
        {
            _actionsProp.InsertArrayElementAtIndex(_actionsProp.arraySize);
            var newActionProp = _actionsProp.GetArrayElementAtIndex(_actionsProp.arraySize - 1);
            newActionProp.managedReferenceValue = new T();
        }
    }
}
#endif
