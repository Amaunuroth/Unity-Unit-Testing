using UnityEngine;
using UnityEditor;
using System.Collections;

[InitializeOnLoad]
public class HierarchyUI : MonoBehaviour
{
    static HierarchyUI()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        Object obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj is GameObject)
        {
            GameObject gameObj = (GameObject)obj;
            Rect offset = new Rect(new Vector2(selectionRect.width, selectionRect.y), new Vector2(14, selectionRect.height));
            bool toggle = GUI.Toggle(offset, gameObj.activeInHierarchy, "");
            if (toggle != gameObj.activeInHierarchy)
                gameObj.SetActive(toggle);
        }
    }
}
