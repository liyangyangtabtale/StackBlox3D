using UnityEngine;

public class RingClearEffect : MonoBehaviour
{
    public float duration = 0.7f;
    private float timer = 0f;
    
    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
} 