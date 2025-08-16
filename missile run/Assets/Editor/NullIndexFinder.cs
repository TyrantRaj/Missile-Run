using UnityEngine;
using UnityEditor;
using System;

public class NullIndexFinder : EditorWindow
{
    [MenuItem("Tools/Find Null Index 0 References")]
    public static void FindNullIndex0()
    {
        var allObjects = GameObject.FindObjectsOfType<MonoBehaviour>(true);

        foreach (var obj in allObjects)
        {
            if (obj == null) continue; // skip missing script slots

            SerializedObject so = null;
            try
            {
                so = new SerializedObject(obj);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Skipping {obj.name} ({obj.GetType().Name}) because SerializedObject creation failed: {e.Message}", obj);
                continue;
            }

            SerializedProperty prop = so.GetIterator();

            while (prop.NextVisible(true))
            {
                if (prop.isArray && prop.propertyType != SerializedPropertyType.String)
                {
                    if (prop.arraySize > 0)
                    {
                        var firstElement = prop.GetArrayElementAtIndex(0);

                        if (firstElement.propertyType == SerializedPropertyType.ObjectReference &&
                            firstElement.objectReferenceValue == null)
                        {
                            Debug.LogWarning(
                                $"Null at index 0 found in '{obj.name}' on component '{obj.GetType().Name}' in property '{prop.name}'",
                                obj
                            );
                        }
                    }
                }
            }
        }

        Debug.Log("Search complete!");
    }
}
