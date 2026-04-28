using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    private float slowFactor;
    private float duration;
    private Enemy enemy;
    private float originalSpeed;
    private SpriteRenderer sr;
    private Color originalColor;

    public void Initialize(float factor, float time)
    {
        slowFactor = factor;
        duration = time;
        enemy = GetComponent<Enemy>();
        sr = GetComponent<SpriteRenderer>();

        if (enemy != null)
        {
            originalSpeed = enemy.GetOriginalSpeed();
            if (sr != null) originalColor = sr.color;
            StartCoroutine(ApplySlow());
        }
    }

    IEnumerator ApplySlow()
    {
        // Зменшуємо швидкість (наприклад якщо factor 0.5, то швидкість стане вдвічі меншою)
        enemy.SetSpeed(originalSpeed * slowFactor);

        // Робимо ворога синім
        if (sr != null) sr.color = new Color(0.5f, 0.7f, 1f);

        yield return new WaitForSeconds(duration);

        // Повертаємо все назад
        if (enemy != null) enemy.SetSpeed(originalSpeed);
        if (sr != null) sr.color = originalColor;

        Destroy(this);
    }
}