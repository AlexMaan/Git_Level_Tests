using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class FloorCell : MonoBehaviour
{
    public enum CellType
    {
        Floor,
        Wall,
        Interactive,
        Door
    }

    [System.Serializable]
    public struct CellState
    {
        public int ID;
        public string Name;
    }

    public enum DoorState
    {
        Opened,
        Closed
    }

    [System.Serializable]
    public struct StateTransition
    {
        public List<ConditionCellStruct> Conditions;
        public float TransitionTime;
        public int TransitionToID;
        public DoorState TransitionToDoorState;
    }

    [System.Serializable]
    public struct ConditionCellStruct
    {
        public FloorCell Cell;
        public int CellStateID;
    }

    [Header("Cell Type")]
    public CellType Type;

    [Header("Cell Materials")]
    public Material FloorMaterial;
    public Material WallMaterial;
    public Material[] InteractiveStatesMaterials;
    public Material DoorOpenedMaterial;
    public Material DoorClosedMaterial;


    [Header("Interactive Cell Properties")]
    public List<CellState> InteractiveCellStates;
    public int CurrentInteractiveCellStateID;
    public int StartInteractiveCellStateID;
    public int PressedInteractiveCellStateID;
    public int BlockedInteractiveCellStateID;

    [Header("Door Properties")]
    public DoorState DoorCellState;

    public List<StateTransition> StateTransitions;

    private List<FloorCell> targets = new List<FloorCell>();

    public int ID;

    private int currentTransitionTimerIndex;
    private float timer;

    private Transform TimerBar;

    public bool isPlayerTarget = false;

    void Start()
    {
        RegisterTargets();
        CurrentInteractiveCellStateID = StartInteractiveCellStateID;
        currentTransitionTimerIndex = -1;

        TimerBar = GameObject.FindGameObjectWithTag("Transition Timer Bar").transform;
    }

    public void UpdateCellType()
    {
        switch (Type)
        {
            case CellType.Floor:
                GetComponent<MeshRenderer>().material = FloorMaterial;
                break;
            case CellType.Wall:
                GetComponent<MeshRenderer>().material = WallMaterial;
                break;
            case CellType.Interactive:
                UpdateInteractiveCellType();
                break;
            case CellType.Door:
                UpdateDoorCellType();
                break;
        }
    }

    void UpdateInteractiveCellType()
    {
        if (CurrentInteractiveCellStateID < InteractiveStatesMaterials.Length)
        {
            GetComponent<MeshRenderer>().material = InteractiveStatesMaterials[CurrentInteractiveCellStateID];
        }
    }

    void UpdateDoorCellType()
    {
        switch (DoorCellState)
        {
            case DoorState.Closed:
                GetComponent<MeshRenderer>().material = DoorClosedMaterial;
                break;
            case DoorState.Opened:
                GetComponent<MeshRenderer>().material = DoorOpenedMaterial;
                break;
        }
    }

    private void RegisterTargets()
    {
        foreach (StateTransition stateTransition in StateTransitions)
        {
            foreach (ConditionCellStruct conditionCellStruct in stateTransition.Conditions)
            {
                conditionCellStruct.Cell.RegisterTarget(this);
            }
        }
    }

    public void RegisterTarget(FloorCell cell)
    {
        targets.Add(cell);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && isPlayerTarget == true)
        {
            isPlayerTarget = false;
            GetPlayerNewTarget();
        }

        if (other.tag == "Player" && Type == CellType.Interactive)
        {
            if (CurrentInteractiveCellStateID != BlockedInteractiveCellStateID && CurrentInteractiveCellStateID != PressedInteractiveCellStateID)
            {
                PressCell();
            }
        }
    }

    void GetPlayerNewTarget()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<AIPath>().target = TargetsManager.GetTarget();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && Type == CellType.Interactive)
        {
            ResetTargetsTransitions();
        }
    }

    private void ResetTargetsTransitions()
    {
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].StopTransition())
            {
                CurrentInteractiveCellStateID = StartInteractiveCellStateID;
                UpdateInteractiveCellType();
            }
        }
    }

    private bool StopTransition()
    {
        if (currentTransitionTimerIndex != -1)
        {
            currentTransitionTimerIndex = -1;
            timer = 0f;
            TimerBar.localScale = Vector3.one;
            return true;
        }
        return false;
    }

    private void PressCell()
    {
        CurrentInteractiveCellStateID = PressedInteractiveCellStateID;
        UpdateInteractiveCellType();
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].CheckCondition();
        }
    }

    public void CheckCondition()
    {
        for (int i = 0; i < StateTransitions.Count; i++)
        {
            if (CheckStateTransition(StateTransitions[i]))
            {
                StartTransition(i);
            }
        }
    }

    private void StartTransition(int i)
    {
        currentTransitionTimerIndex = i;
        timer = 0f;
    }

    void Update()
    {
        if (currentTransitionTimerIndex != -1)
        {
            //Debug.Log(ID + " " + currentTransitionTimerIndex);
            timer += Time.deltaTime;

            if (StateTransitions[currentTransitionTimerIndex].TransitionTime != 0f)
            {
                TimerBar.localScale = new Vector3(1f, (1f - timer / StateTransitions[currentTransitionTimerIndex].TransitionTime), 1f);
            }
            
            if (timer >= StateTransitions[currentTransitionTimerIndex].TransitionTime)
            {
                TransitState(StateTransitions[currentTransitionTimerIndex]);
                currentTransitionTimerIndex = -1;
            }
        }
    }

    private void TransitState(StateTransition stateTransition)
    {
        TimerBar.localScale = Vector3.one;
        if (Type == CellType.Interactive)
        {
            CurrentInteractiveCellStateID = stateTransition.TransitionToID;
        }
        else
        {
            DoorCellState = stateTransition.TransitionToDoorState;
        }
        UpdateCellType();
    }

    private bool CheckStateTransition(StateTransition stateTransition)
    {
        for (int i = 0; i < stateTransition.Conditions.Count; i++)
        {
            if (stateTransition.Conditions[i].Cell.CurrentInteractiveCellStateID !=
                stateTransition.Conditions[i].CellStateID)
            {
                return false;
            }
        }

        return true;
    }
}

