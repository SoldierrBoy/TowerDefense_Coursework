using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CannonBall : MonoBehaviour
{
    private Enemy target;
    private float damage;
    public float speed = 12f; 

    public void Seek(Enemy _target, float _damage)
    {
        target = _target;
        damage = _damage;
    }

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }

        Vector2 direction = (Vector2)target.transform.position - (Vector2)transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        // Обертання ядра (можна зробити, щоб воно крутилося в польоті)
        transform.Rotate(0, 0, 500 * Time.deltaTime);
    }

    void HitTarget()
    {
        target.TakeDamage(damage);
        // Тут можна буде додати звук  або маленький спалах
        Destroy(gameObject);
    }
}