using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    private float damagePerTick;
    private float duration;
    private Enemy enemy;
    private Color originalColor;
    private SpriteRenderer sr;

    public void Initialize(float damage, float time)
    {
        damagePerTick = damage;
        duration = time;
        enemy = GetComponent<Enemy>();
        sr = GetComponent<SpriteRenderer>();

        if (sr != null) originalColor = sr.color;

        StartCoroutine(ApplyPoison());
    }

    IEnumerator ApplyPoison()
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerTick);
                if (sr != null) sr.color = new Color(0.2f, 1f, 0.2f); // Робимо зеленим
            }

            yield return new WaitForSeconds(1f); // Шкода щосекунди
            elapsed += 1f;
        }

        if (sr != null) sr.color = originalColor; // Повертаємо колір
        Destroy(this);
    }
}