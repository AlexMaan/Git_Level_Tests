using UnityEngine;
using System.Collections;

public class FloorGenerator : MonoBehaviour
{
    public GameObject FloorCell;
    public GameObject PathNode;
    public Transform CellsRoot;
    public Transform NodesRoot;
    public float CellSizeX = 1f;
    public float CellSizeY = 1f;
    public int FieldSizeX;
    public int FieldSizeY;

    public void GenerateField()
    {
        for (int i = 0; i < CellsRoot.childCount; )
        {
            DestroyImmediate(CellsRoot.GetChild(i).gameObject);
        }

        for (int i = -FieldSizeX/2; i < FieldSizeX/2; i++)
        {
            for (int j = -FieldSizeY/2; j < FieldSizeY/2; j++)
            {
                GameObject cell = Instantiate(FloorCell, transform.position + new Vector3(i * CellSizeX, 0f, j * CellSizeY), Quaternion.Euler(90f, 0f, 0f)) as GameObject;
                cell.transform.parent = CellsRoot.transform;
            }
        }
    }

    public void GeneratePathNodes()
    {
        for (int i = 0; i < NodesRoot.childCount;)
        {
            DestroyImmediate(NodesRoot.GetChild(i).gameObject);
        }
        

        for (int i = 0; i < CellsRoot.childCount; i++)
        {
            if (CellsRoot.GetChild(i).GetComponent<FloorCell>().Type == global::FloorCell.CellType.Floor)
            {
                GameObject node = Instantiate(PathNode, CellsRoot.GetChild(i).position, Quaternion.identity) as GameObject;
                node.transform.parent = NodesRoot;
            }
        }
    }
}
