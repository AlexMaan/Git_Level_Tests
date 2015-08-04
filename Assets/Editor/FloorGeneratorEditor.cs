using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(FloorGenerator))]
public class FloorGeneratorEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        FloorGenerator generator = (FloorGenerator)target;

        generator.FieldSizeX = EditorGUILayout.IntField("Field X Size", generator.FieldSizeX);
        generator.FieldSizeY = EditorGUILayout.IntField("Field Y Size", generator.FieldSizeY);
        generator.CellSizeX = EditorGUILayout.FloatField("Cell X Size", generator.CellSizeX);
        generator.CellSizeY = EditorGUILayout.FloatField("Cell Y Size", generator.CellSizeY);
        generator.FloorCell = EditorGUILayout.ObjectField("Floor Cell", generator.FloorCell, typeof(GameObject), false) as GameObject;
        generator.PathNode = EditorGUILayout.ObjectField("Path Node", generator.PathNode, typeof(GameObject), false) as GameObject;
        generator.CellsRoot = EditorGUILayout.ObjectField("Cells Root", generator.CellsRoot, typeof(Transform), true) as Transform;
        generator.NodesRoot = EditorGUILayout.ObjectField("Nodes Root", generator.NodesRoot, typeof(Transform), true) as Transform;

        if (GUILayout.Button("Generate New Field"))
        {
            generator.GenerateField();
        }

        if (GUILayout.Button("Generate Path Nodes"))
        {
            generator.GeneratePathNodes();
        }
    }
}
