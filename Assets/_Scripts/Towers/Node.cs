using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public GameObject towerOnThisNode; 
    
    // НОВА ЗМІННА: Дозволяє підсунути вежу при спавні, не чіпаючи префаб!
    public Vector3 positionOffset; 

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (towerOnThisNode != null)
        {
            Debug.Log("Місце зайняте!");
            return; 
        }

        BuildManager.Instance.SelectNode(this);
    }
}