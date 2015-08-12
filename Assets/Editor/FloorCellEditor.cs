using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(FloorCell))]
[CanEditMultipleObjects]
public class Cell : Editor 
{
    SerializedProperty cellType;
    SerializedProperty cellState;
    SerializedProperty doorState;
    SerializedProperty floorMaterial;
    SerializedProperty wallMaterial;
    SerializedProperty interactiveOffMaterial;
    SerializedProperty interactiveOnMaterial;
    SerializedProperty interactiveBlockedMaterial;
    SerializedProperty doorClosedMaterial;
    SerializedProperty doorOpenedMaterial;
    SerializedProperty targetCell;
    SerializedProperty targetInteractiveState;
    SerializedProperty targetDoorState;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        cellType = serializedObject.FindProperty("Type");
        cellState = serializedObject.FindProperty("InteractiveCellState");
        doorState = serializedObject.FindProperty("DoorCellState");
        targetCell = serializedObject.FindProperty("TargetCell");
        targetInteractiveState = serializedObject.FindProperty("TargetInteractiveState");
        targetDoorState = serializedObject.FindProperty("TargetDoorState");
        
        floorMaterial = serializedObject.FindProperty("FloorMaterial");
        wallMaterial = serializedObject.FindProperty("WallMaterial");
        interactiveOffMaterial = serializedObject.FindProperty("InteractiveOffMaterial");
        interactiveOnMaterial = serializedObject.FindProperty("InteractiveOnMaterial");
        interactiveBlockedMaterial = serializedObject.FindProperty("InteractiveBlockMaterial");
        doorClosedMaterial = serializedObject.FindProperty("DoorClosedMaterial");
        doorOpenedMaterial = serializedObject.FindProperty("DoorOpenedMaterial");
    }

    public override void OnInspectorGUI()
    {
        FloorCell cell = (FloorCell)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(cellType);

        if (cell.Type == FloorCell.CellType.Interactive)
        {
            EditorGUILayout.PropertyField(cellState);
            EditorGUILayout.PropertyField(targetCell);
            if (cell.TargetCell != null)
            {
                if (cell.TargetCell.Type == FloorCell.CellType.Interactive)
                {
                    EditorGUILayout.PropertyField(targetInteractiveState);
                }
                if (cell.TargetCell.Type == FloorCell.CellType.Door)
                {
                    EditorGUILayout.PropertyField(targetDoorState);
                }
            }
        }

        if (cell.Type == FloorCell.CellType.Door)
        {
            EditorGUILayout.PropertyField(doorState);
        }

        foreach (FloorCell t in targets)
        {
            t.UpdateCellType();
        }

        EditorGUILayout.PropertyField(floorMaterial);
        EditorGUILayout.PropertyField(wallMaterial);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(interactiveOffMaterial);
        EditorGUILayout.PropertyField(interactiveOnMaterial);
        EditorGUILayout.PropertyField(interactiveBlockedMaterial);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(doorClosedMaterial);
        EditorGUILayout.PropertyField(doorOpenedMaterial);

        serializedObject.ApplyModifiedProperties();
    }    
}
