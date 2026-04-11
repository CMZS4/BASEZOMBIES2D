using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    TextMeshPro tmp;
    float lifetime = 1f;
    float elapsed = 0f;
    Vector3 moveDir = new Vector3(0f, 1.5f, 0f);
    Color startColor;

    void Awake()
    {
        tmp = GetComponent<TextMeshPro>();
        startColor = tmp.color;
    }

    public void Init(int damage, Color color)
    {
        tmp.text = damage.ToString();
        tmp.color = color;
        startColor = color;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        transform.position += moveDir * Time.deltaTime;

        // Yavaşla
        moveDir = Vector3.Lerp(moveDir, Vector3.zero, Time.deltaTime * 3f);

        // Solar
        float alpha = Mathf.Lerp(1f, 0f, elapsed / lifetime);
        tmp.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        if (elapsed >= lifetime)
            Destroy(gameObject);
    }
}