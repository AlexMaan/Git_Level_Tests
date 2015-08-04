using UnityEngine;
using System.Collections;

public class FloorCell : MonoBehaviour 
{
    public enum CellType
    {
        Floor,
        Wall,
        Interactive
    }

    [Header("Cell Type")]
    public CellType Type;

    [Header("Cell Materials")]
    public Material FloorMaterial;
    public Material WallMaterial;

    public bool IsOn;

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
                break;
        }
    }
}
