using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR) 
namespace TFM.Entities
{
    /// <summary>
    /// Class <c>ItemListEditor</c> represents the editor for the item list.
    /// </summary>
    [CustomEditor(typeof(ItemList))]
    public class ItemListEditor : Editor
    {
        /// <summary>
        /// Method <c>OnInspectorGUI</c> is called to draw the inspector GUI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var itemList = (ItemList)target;
            if (GUILayout.Button("Update Item List"))
                UpdateItemList(itemList);
        }

        /// <summary>
        /// Method <c>UpdateItemList</c> updates the item list.
        /// </summary>
        /// <param name="itemList">The item list.</param>
        private void UpdateItemList(ItemList itemList)
        {
            // Get all the items from the items folder
            var itemAssets = AssetDatabase.FindAssets("t:Item", new[] { itemList.itemsFolder });
            foreach (var itemAsset in itemAssets)
            {
                // Get the item
                var itemFile = AssetDatabase.LoadAssetAtPath<Item>(AssetDatabase.GUIDToAssetPath(itemAsset));
                // Add the item to the item list
                if (!itemList.items.Contains(itemFile))
                    itemList.items.Add(itemFile);
            }
            EditorUtility.SetDirty(itemList);
            Debug.Log("Item list updated.");
        }
    }
}
#endif