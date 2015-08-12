using UnityEngine;
using System.Collections;

public class FloorCell : MonoBehaviour 
{
    public enum CellType
    {
        Floor,
        Wall,
        Interactive,
        Door
    }

    public enum CellState
    {
        Off,
        On,
        Blocked
    }

    public enum DoorState
    {
        Opened,
        Closed
    }

    [Header("Cell Type")]
    public CellType Type;

    [Header("Cell Materials")]
    public Material FloorMaterial;
    public Material WallMaterial;
    public Material InteractiveOffMaterial;
    public Material InteractiveOnMaterial;
    public Material InteractiveBlockMaterial;
    public Material DoorOpenedMaterial;
    public Material DoorClosedMaterial;


    [Header("Interactive Cell Properties")]
    public CellState InteractiveCellState;
    public FloorCell TargetCell;
    public CellState TargetInteractiveState;
    public DoorState TargetDoorState;


    [Header("Door Properties")]
    public DoorState DoorCellState;

    public int ID;

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

    public void UpdateInteractiveTargetState(CellState state)
    {
        InteractiveCellState = state;
        UpdateInteractiveCellType();
    }

    public void UpdateDoorTargetState(DoorState state)
    {
        DoorCellState = state;
        if (state == DoorState.Opened)
        {
            FloorGenerator.Instance.AddPathNode(ID);
            AstarPath.active.Scan();
        }
        if (state == DoorState.Closed)
        {
            FloorGenerator.Instance.RemovePathNode(ID);
            AstarPath.active.Scan();
        }

        UpdateDoorCellType();
    }

    void UpdateInteractiveCellType()
    {
        switch (InteractiveCellState)
        {
            case CellState.Off:
                GetComponent<MeshRenderer>().material = InteractiveOffMaterial;
                break;
            case CellState.On:
                GetComponent<MeshRenderer>().material = InteractiveOnMaterial;
                break;
            case CellState.Blocked:
                GetComponent<MeshRenderer>().material = InteractiveBlockMaterial;
                break;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Type == CellType.Interactive && TargetCell != null)
        {
            if (InteractiveCellState == CellState.Off)
            {
                InteractiveCellState = CellState.On;
                switch (TargetCell.Type)
                {
                    case CellType.Interactive:
                        TargetCell.UpdateInteractiveTargetState(TargetInteractiveState);
                        UpdateInteractiveCellType();
                        break;
                    case CellType.Door:
                        TargetCell.UpdateDoorTargetState(TargetDoorState);
                        UpdateInteractiveCellType();
                        break;
                }
            }
        }
    }
}
