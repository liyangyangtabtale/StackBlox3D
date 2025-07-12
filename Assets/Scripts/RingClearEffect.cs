using UnityEngine;

public class RingClearEffect : MonoBehaviour
{
    public float duration = 0.7f;
    public float startScale = 0.5f;
    public float endScale = 2.5f;
    private float timer = 0f;
    private SpriteRenderer sr;
    private Color baseColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            baseColor = sr.color;
    }

    void OnEnable()
    {
        timer = 0f;
        if (sr != null)
            sr.color = baseColor;
        transform.localScale = Vector3.one * startScale;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;
        float scale = Mathf.Lerp(startScale, endScale, t);
        transform.localScale = Vector3.one * scale;
        if (sr != null)
        {
            Color c = baseColor;
            c.a = Mathf.Lerp(baseColor.a, 0f, t);
            sr.color = c;
        }
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
} 