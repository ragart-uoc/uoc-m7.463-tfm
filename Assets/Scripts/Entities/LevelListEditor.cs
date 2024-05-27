using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR) 
namespace TFM.Entities
{
    /// <summary>
    /// Class <c>LevelListEditor</c> represents the editor for the level list.
    /// </summary>
    [CustomEditor(typeof(LevelList))]
    public class LevelListEditor : Editor
    {
        /// <summary>
        /// Method <c>OnInspectorGUI</c> is called to draw the inspector GUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var levelList = (LevelList)target;
            if (GUILayout.Button("Update Scene Names"))
                UpdateSceneNames(levelList);
        }

        /// <summary>
        /// Method <c>UpdateSceneNames</c> updates the scene names for the levels.
        /// </summary>
        /// <param name="levelList">The level list.</param>
        private void UpdateSceneNames(LevelList levelList)
        {
            // Get all the levels from the levels folder
            var levelAssets = AssetDatabase.FindAssets("t:Level", new[] { levelList.levelsFolder });
            foreach (var levelAsset in levelAssets)
            {
                // Get the level
                var level = AssetDatabase.LoadAssetAtPath<Level>(AssetDatabase.GUIDToAssetPath(levelAsset));
                // If the scene asset is null, skip the level
                if (level.sceneAsset == null)
                    continue;
                // Get the scene name from the scene asset file name
                var scenePath = AssetDatabase.GetAssetPath(level.sceneAsset);
                var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                // Update the scene name in the Level scriptable object
                level.sceneName = sceneName;
                // Add the level to the level list
                if (!levelList.levels.Contains(level))
                    levelList.levels.Add(level);
            }
            EditorUtility.SetDirty(levelList);
            Debug.Log("Scene names updated.");
        }
    }
}
#endif
