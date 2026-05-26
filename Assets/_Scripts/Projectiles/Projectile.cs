using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public GameObject myPrefab;

    public void ReturnToPool()
    {
        if (myPrefab != null && PoolManager.Instance != null)
        {
            PoolManager.Instance.Release(myPrefab, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}