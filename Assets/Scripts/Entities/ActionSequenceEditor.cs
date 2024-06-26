using UnityEditor;
using UnityEngine;
using TFM.Actions;

#if (UNITY_EDITOR) 
namespace TFM.Entities
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
                    case "DialogueShowAction":
                        EditorGUILayout.LabelField("Dialogue - Show", EditorStyles.boldLabel);
                        break;
                    case "DialogueHideAction":
                        EditorGUILayout.LabelField("Dialogue - Hide", EditorStyles.boldLabel);
                        break;
                    case "EventTriggerAction":
                        EditorGUILayout.LabelField("Event - Trigger", EditorStyles.boldLabel);
                        break;
                    case "LevelLoadAction":
                        EditorGUILayout.LabelField("Level - Load", EditorStyles.boldLabel);
                        break;
                    case "LevelChangeAgeGroupAction":
                        EditorGUILayout.LabelField("Level - Change Age Group", EditorStyles.boldLabel);
                        break;
                    case "MessageShowAction":
                        EditorGUILayout.LabelField("Message - Show", EditorStyles.boldLabel);
                        break;
                }
                
                if (actionType.EndsWith("DialogueShowAction"))
                {
                    var actorProp = actionProp.FindPropertyRelative("actor");
                    var dialogueLineProp = actionProp.FindPropertyRelative("dialogueLine");
                    var positionProp = actionProp.FindPropertyRelative("position");
                    EditorGUILayout.PropertyField(actorProp, new GUIContent("Actor"));
                    EditorGUILayout.PropertyField(dialogueLineProp, new GUIContent("Dialogue Line"));
                    positionProp.intValue = EditorGUILayout.Popup(positionProp.displayName, positionProp.intValue, new[] {"Left", "Right"});
                }
                else if (actionType.EndsWith("DialogueHideAction"))
                {
                    // No properties to display
                }
                else if (actionType.EndsWith("EventTriggerAction"))
                {
                    var eventObjectProp = actionProp.FindPropertyRelative("eventObject");
                    EditorGUILayout.PropertyField(eventObjectProp, new GUIContent("Event"));
                }
                else if (actionType.EndsWith("ItemPickAction"))
                {
                    var itemProp = actionProp.FindPropertyRelative("item");
                    EditorGUILayout.PropertyField(itemProp, new GUIContent("Item"));
                }
                else if (actionType.EndsWith("ItemDiscardAction"))
                {
                    var itemProp = actionProp.FindPropertyRelative("item");
                    EditorGUILayout.PropertyField(itemProp, new GUIContent("Item"));
                }
                else if (actionType.EndsWith("LevelLoadAction"))
                {
                    var levelProp = actionProp.FindPropertyRelative("level");
                    var fadeOverlayProp = actionProp.FindPropertyRelative("fadeOverlay");
                    var destroyPersistentManagers = actionProp.FindPropertyRelative("destroyPersistentManagers");
                    EditorGUILayout.PropertyField(levelProp, new GUIContent("Level"));
                    fadeOverlayProp.intValue = EditorGUILayout.Popup(fadeOverlayProp.displayName, fadeOverlayProp.intValue, new[] {"No", "Yes"});
                    destroyPersistentManagers.intValue = EditorGUILayout.Popup(destroyPersistentManagers.displayName, destroyPersistentManagers.intValue, new[] {"No", "Yes"});
                }
                else if (actionType.EndsWith("LevelChangeAgeGroupAction"))
                {
                    var ageGroupProp = actionProp.FindPropertyRelative("ageGroup");
                    EditorGUILayout.PropertyField(ageGroupProp, new GUIContent("Age Group"));
                }
                else if (actionType.EndsWith("MessageShowAction"))
                {
                    var messageProp = actionProp.FindPropertyRelative("message");
                    var durationProp = actionProp.FindPropertyRelative("duration");
                    var afterMessageProp = actionProp.FindPropertyRelative("afterMessage");
                    EditorGUILayout.PropertyField(messageProp, new GUIContent("Message"));
                    EditorGUILayout.PropertyField(durationProp, new GUIContent("Duration"));
                    EditorGUILayout.PropertyField(afterMessageProp, new GUIContent("After Message"));
                }
                else if (actionType.EndsWith("ScreenShakeAction"))
                {
                    // No properties to display
                }
                var waitForInputProp = actionProp.FindPropertyRelative("waitForInput");
                waitForInputProp.intValue = EditorGUILayout.Popup(waitForInputProp.displayName, waitForInputProp.intValue, new[] {"No", "Yes"});
                var waitAfterActionProp = actionProp.FindPropertyRelative("waitAfterAction");
                EditorGUILayout.PropertyField(waitAfterActionProp, new GUIContent("Wait Between Actions"));

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Remove Action"))
                {
                    _actionsProp.DeleteArrayElementAtIndex(i);
                    continue;
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Dialogue - Show"))
            {
                AddAction<DialogueShowAction>();
            }
            
            if (GUILayout.Button("Dialogue - Hide"))
            {
                AddAction<DialogueHideAction>();
            }
            
            if (GUILayout.Button("Event - Trigger"))
            {
                AddAction<EventTriggerAction>();
            }

            if (GUILayout.Button("Item - Pick"))
            {
                AddAction<ItemPickAction>();
            }
            
            if (GUILayout.Button("Item - Discard"))
            {
                AddAction<ItemDiscardAction>();
            }

            if (GUILayout.Button("Level - Load"))
            {
                AddAction<LevelLoadAction>();
            }
            
            if (GUILayout.Button("Level - Change Age Group"))
            {
                AddAction<LevelChangeAgeGroupAction>();
            }
            
            if (GUILayout.Button("Message - Show"))
            {
                AddAction<MessageShowAction>();
            }
            
            if (GUILayout.Button("Screen - Shake"))
            {
                AddAction<ScreenShakeAction>();
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
