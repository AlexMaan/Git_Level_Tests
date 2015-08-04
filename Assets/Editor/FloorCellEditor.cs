using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(FloorCell))]
[CanEditMultipleObjects]
public class Cell : Editor 
{
    SerializedProperty cellType;
    SerializedProperty isOn;
    SerializedProperty floorMaterial;
    SerializedProperty wallMaterial;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        cellType = serializedObject.FindProperty("Type");
        isOn = serializedObject.FindProperty("IsOn");
        floorMaterial = serializedObject.FindProperty("FloorMaterial");
        wallMaterial = serializedObject.FindProperty("WallMaterial");
    }

    public override void OnInspectorGUI()
    {
        FloorCell cell = (FloorCell)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(cellType);

        if (cell.Type == FloorCell.CellType.Interactive)
        {
            EditorGUILayout.PropertyField(isOn);
        }

        foreach (FloorCell t in targets)
        {
            t.UpdateCellType();
        }

        EditorGUILayout.PropertyField(floorMaterial);
        EditorGUILayout.PropertyField(wallMaterial);

        serializedObject.ApplyModifiedProperties();
    }    
}
