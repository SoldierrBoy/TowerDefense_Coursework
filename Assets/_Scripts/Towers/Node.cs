using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public GameObject towerOnThisNode; 

    void OnMouseDown()
    {
        // 1. Захист від проклікування крізь UI (меню, кнопки)
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // 2. Захист: якщо вежа вже є - блокуємо клік
        if (towerOnThisNode != null)
        {
            Debug.Log("Місце зайняте! Більше веж сюди не поставити.");
            return; 
        }

        // 3. Якщо все ок - передаємо цей Нод в BuildManager і відкриваємо магазин
        BuildManager.Instance.SelectNode(this);
    }
}