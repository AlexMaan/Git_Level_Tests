using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomEditor(typeof(FloorCell))]
[CanEditMultipleObjects]
public class Cell : Editor 
{
    SerializedProperty cellType;
    //SerializedProperty cellState;
    SerializedProperty doorState;
    SerializedProperty floorMaterial;
    SerializedProperty wallMaterial;

    SerializedProperty interactiveStatesMaterials;

    SerializedProperty doorClosedMaterial;
    SerializedProperty doorOpenedMaterial;

    SerializedProperty interactiveCellStates;
    SerializedProperty startInteractiveCellStateID;
    SerializedProperty pressedInteractiveCellStateID;
    SerializedProperty blockedInteractiveCellStateID;

    SerializedProperty stateTransitions;

    private int interactiveCellStatesSize;
    private int stateTransitionsSize;
    //private int interactiveStatesMaterialsSize;


    void OnEnable()
    {
        // Setup the SerializedProperties.
        cellType = serializedObject.FindProperty("Type");
        doorState = serializedObject.FindProperty("DoorCellState");
        
        floorMaterial = serializedObject.FindProperty("FloorMaterial");
        wallMaterial = serializedObject.FindProperty("WallMaterial");

        doorClosedMaterial = serializedObject.FindProperty("DoorClosedMaterial");
        doorOpenedMaterial = serializedObject.FindProperty("DoorOpenedMaterial");

        interactiveStatesMaterials = serializedObject.FindProperty("InteractiveStatesMaterials");
        interactiveCellStates = serializedObject.FindProperty("InteractiveCellStates");

        startInteractiveCellStateID = serializedObject.FindProperty("StartInteractiveCellStateID");
        pressedInteractiveCellStateID = serializedObject.FindProperty("PressedInteractiveCellStateID");
        blockedInteractiveCellStateID = serializedObject.FindProperty("BlockedInteractiveCellStateID");

        stateTransitions = serializedObject.FindProperty("StateTransitions");
    }

    public override void OnInspectorGUI()
    {
        FloorCell cell = (FloorCell)target;

        serializedObject.Update();

        EditorGUILayout.LabelField("ID: " + cell.ID);

        EditorGUILayout.PropertyField(cellType);

        foreach (FloorCell t in targets)
        {
            t.UpdateCellType();
        }
        
        //INTERACTIVE CELL SETTINGS
        EditorGUILayout.Space();
        if (cell.Type == FloorCell.CellType.Interactive)
        {
            EditorGUILayout.LabelField("Interactive Cell States", EditorStyles.boldLabel);
            //Interactive States Here --------------------------------------------------------------------------------------------------
            interactiveCellStatesSize = interactiveCellStates.arraySize;
            interactiveCellStatesSize = EditorGUILayout.IntField("States Count:", interactiveCellStatesSize);

            if (interactiveCellStatesSize != interactiveCellStates.arraySize)
            {
                while (interactiveCellStatesSize > interactiveCellStates.arraySize)
                {
                    interactiveCellStates.InsertArrayElementAtIndex(interactiveCellStates.arraySize);
                }
                while (interactiveCellStatesSize < interactiveCellStates.arraySize)
                {
                    interactiveCellStates.DeleteArrayElementAtIndex(interactiveCellStates.arraySize - 1);
                }
            }

            for (int i = 0; i < interactiveCellStates.arraySize; i++)
            {
                SerializedProperty itemRef = interactiveCellStates.GetArrayElementAtIndex(i);
                SerializedProperty itemID = itemRef.FindPropertyRelative("ID");
                SerializedProperty itemName = itemRef.FindPropertyRelative("Name");

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("State ID " + i, GUILayout.Width(75));
                //EditorGUILayout.PropertyField(itemID, GUIContent.none, true, GUILayout.MinWidth(50), GUILayout.MaxWidth(50));
                GUILayout.Label("Name", GUILayout.Width(50));
                EditorGUILayout.PropertyField(itemName, GUIContent.none, true, GUILayout.MinWidth(100));
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add New State"))
            {
                FloorCell.CellState state = new FloorCell.CellState();
                state.ID = interactiveCellStatesSize;
                cell.InteractiveCellStates.Add(state);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Interactive Cell Materials", EditorStyles.boldLabel);
            //interactiveStatesMaterialsSize = interactiveStatesMaterials.arraySize;

            if (interactiveCellStatesSize != interactiveStatesMaterials.arraySize)
            {
                while (interactiveCellStatesSize > interactiveStatesMaterials.arraySize)
                {
                    interactiveStatesMaterials.InsertArrayElementAtIndex(interactiveStatesMaterials.arraySize);
                }
                while (interactiveCellStatesSize < interactiveStatesMaterials.arraySize)
                {
                    interactiveStatesMaterials.DeleteArrayElementAtIndex(interactiveStatesMaterials.arraySize - 1);
                }
            }

            for (int i = 0; i < interactiveStatesMaterials.arraySize; i++)
            {
                SerializedProperty itemRef = interactiveStatesMaterials.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(itemRef, new GUIContent("Material for ID " + i));
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Predefined States", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(startInteractiveCellStateID, new GUIContent("Start State ID"));
            EditorGUILayout.PropertyField(pressedInteractiveCellStateID, new GUIContent("Pressed State ID"));
            EditorGUILayout.PropertyField(blockedInteractiveCellStateID, new GUIContent("Blocked State ID"));



            //Transit Conditions here ----------------------------------------------------------------------------------
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Cell Transitions", EditorStyles.boldLabel);
            stateTransitionsSize = stateTransitions.arraySize;
            stateTransitionsSize = EditorGUILayout.IntField("Transitions Count:", stateTransitionsSize);

            if (stateTransitionsSize != stateTransitions.arraySize)
            {
                while (stateTransitionsSize > stateTransitions.arraySize)
                {
                    stateTransitions.InsertArrayElementAtIndex(stateTransitions.arraySize);
                }
                while (stateTransitionsSize < stateTransitions.arraySize)
                {
                    stateTransitions.DeleteArrayElementAtIndex(stateTransitions.arraySize - 1);
                }
            }

            EditorGUILayout.Space();

            EditorGUI.indentLevel++;
            
            for (int i = 0; i < stateTransitions.arraySize; i++)
            {
                EditorGUILayout.LabelField("Transition " + i, EditorStyles.boldLabel);
                SerializedProperty itemRef = stateTransitions.GetArrayElementAtIndex(i);
                SerializedProperty transitionToID = itemRef.FindPropertyRelative("TransitionToID");
                SerializedProperty transitionTime = itemRef.FindPropertyRelative("TransitionTime");

                SerializedProperty conditions = itemRef.FindPropertyRelative("Conditions");
                EditorGUILayout.PropertyField(transitionToID);
                EditorGUILayout.PropertyField(transitionTime);



                EditorGUILayout.Space();
                
                EditorGUILayout.LabelField("Transit Conditions", EditorStyles.boldLabel);

                EditorGUI.indentLevel++;
                int conditionsSize = conditions.arraySize;
                conditionsSize = EditorGUILayout.IntField("Conditions Count:", conditionsSize);

                if (conditionsSize != conditions.arraySize)
                {
                    while (conditionsSize > conditions.arraySize)
                    {
                        conditions.InsertArrayElementAtIndex(conditions.arraySize);
                    }
                    while (stateTransitionsSize < conditions.arraySize)
                    {
                        conditions.DeleteArrayElementAtIndex(conditions.arraySize - 1);
                    }
                }

                EditorGUILayout.Space();

                for (int j = 0; j < conditions.arraySize; j++)
                {
                    SerializedProperty subItemRef = conditions.GetArrayElementAtIndex(j);
                    SerializedProperty conditionCell = subItemRef.FindPropertyRelative("Cell");
                    SerializedProperty conditionCellStateID = subItemRef.FindPropertyRelative("CellStateID");

                    EditorGUILayout.PropertyField(conditionCell);
                    EditorGUILayout.PropertyField(conditionCellStateID);

                    
                }
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Condition", GUILayout.MaxWidth(Screen.width * 0.85f)))
                {
                    FloorCell.ConditionCellStruct conditionCellStruct = new FloorCell.ConditionCellStruct();

                    cell.StateTransitions[i].Conditions.Add(conditionCellStruct);
                }
                GUILayout.EndHorizontal();
                
            }

            EditorGUILayout.Space();
            

            if (GUILayout.Button("Add New Transition"))
            {
                FloorCell.StateTransition transition = new FloorCell.StateTransition();
                transition.Conditions = new List<FloorCell.ConditionCellStruct>();

                cell.StateTransitions.Add(transition);
            }
        }

        //END OF INTERACTIVE SETTINGS

        //DOOR SETTINGS

        if (cell.Type == FloorCell.CellType.Door)
        {
            EditorGUILayout.PropertyField(doorState);
           
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Door Materials", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(doorClosedMaterial);
            EditorGUILayout.PropertyField(doorOpenedMaterial);

            //Transit Conditions here ----------------------------------------------------------------------------------
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Door Transitions", EditorStyles.boldLabel);
            stateTransitionsSize = stateTransitions.arraySize;
            stateTransitionsSize = EditorGUILayout.IntField("Transitions Count:", stateTransitionsSize);

            if (stateTransitionsSize != stateTransitions.arraySize)
            {
                while (stateTransitionsSize > stateTransitions.arraySize)
                {
                    stateTransitions.InsertArrayElementAtIndex(stateTransitions.arraySize);
                }
                while (stateTransitionsSize < stateTransitions.arraySize)
                {
                    stateTransitions.DeleteArrayElementAtIndex(stateTransitions.arraySize - 1);
                }
            }

            EditorGUILayout.Space();

            EditorGUI.indentLevel++;

            for (int i = 0; i < stateTransitions.arraySize; i++)
            {
                EditorGUILayout.LabelField("Transition " + i, EditorStyles.boldLabel);
                SerializedProperty itemRef = stateTransitions.GetArrayElementAtIndex(i);
                SerializedProperty transitionToDoorState = itemRef.FindPropertyRelative("TransitionToDoorState");
                SerializedProperty transitionTime = itemRef.FindPropertyRelative("TransitionTime");

                SerializedProperty conditions = itemRef.FindPropertyRelative("Conditions");
                EditorGUILayout.PropertyField(transitionToDoorState);
                EditorGUILayout.PropertyField(transitionTime);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Transit Conditions", EditorStyles.boldLabel);

                EditorGUI.indentLevel++;
                int conditionsSize = conditions.arraySize;
                conditionsSize = EditorGUILayout.IntField("Conditions Count:", conditionsSize);

                if (conditionsSize != conditions.arraySize)
                {
                    while (conditionsSize > conditions.arraySize)
                    {
                        conditions.InsertArrayElementAtIndex(conditions.arraySize);
                    }
                    while (stateTransitionsSize < conditions.arraySize)
                    {
                        conditions.DeleteArrayElementAtIndex(conditions.arraySize - 1);
                    }
                }

                EditorGUILayout.Space();

                for (int j = 0; j < conditions.arraySize; j++)
                {
                    SerializedProperty subItemRef = conditions.GetArrayElementAtIndex(j);
                    SerializedProperty conditionCell = subItemRef.FindPropertyRelative("Cell");
                    SerializedProperty conditionCellStateID = subItemRef.FindPropertyRelative("CellStateID");

                    EditorGUILayout.PropertyField(conditionCell);
                    EditorGUILayout.PropertyField(conditionCellStateID);


                }
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Condition", GUILayout.MaxWidth(Screen.width * 0.85f)))
                {
                    FloorCell.ConditionCellStruct conditionCellStruct = new FloorCell.ConditionCellStruct();

                    cell.StateTransitions[i].Conditions.Add(conditionCellStruct);
                }
                GUILayout.EndHorizontal();

            }

            EditorGUILayout.Space();


            if (GUILayout.Button("Add New Transition"))
            {
                FloorCell.StateTransition transition = new FloorCell.StateTransition();
                transition.Conditions = new List<FloorCell.ConditionCellStruct>();

                cell.StateTransitions.Add(transition);
            }
        }

        // END OF DOOR SETTINGS

        if (cell.Type == FloorCell.CellType.Floor)
        {
            EditorGUILayout.PropertyField(floorMaterial);
        }

        if (cell.Type == FloorCell.CellType.Wall)
        {
            EditorGUILayout.PropertyField(wallMaterial);
        }



        serializedObject.ApplyModifiedProperties();





        /*FloorCell cell = (FloorCell)target;

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

        serializedObject.ApplyModifiedProperties();*/
    }    
}
