using UnityEditor;
using UnityEngine;

namespace TFM.Debug.Scripts.Interaction.Actions
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

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Remove Action"))
                {
                    _actionsProp.DeleteArrayElementAtIndex(i);
                    continue;
                }

                EditorGUILayout.EndHorizontal();

                if (actionType.EndsWith("DialogueAction"))
                {
                    var actorProp = actionProp.FindPropertyRelative("actor");
                    var dialogueLineProp = actionProp.FindPropertyRelative("dialogueLine");
                    EditorGUILayout.PropertyField(actorProp, new GUIContent("Actor"));
                    EditorGUILayout.PropertyField(dialogueLineProp, new GUIContent("Dialogue Line"));
                }
                else if (actionType.EndsWith("SceneChangeAction"))
                {
                    var sceneProp = actionProp.FindPropertyRelative("sceneName");
                    EditorGUILayout.PropertyField(sceneProp, new GUIContent("Scene Name"));
                }
            }

            if (GUILayout.Button("Add Dialogue Action"))
            {
                AddAction<DialogueAction>();
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