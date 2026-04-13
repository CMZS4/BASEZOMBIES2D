using UnityEngine;

public class ReloadIndicator : MonoBehaviour
{
    [Header("Referanslar")]
    public WeaponSystem weaponSystem;
    public Transform playerTransform;

    [Header("Halka")]
    public LineRenderer ring;
    public int segments = 40;
    public float radius = 0.6f;

    void Start()
    {
        if (weaponSystem == null)
            weaponSystem = FindObjectOfType<WeaponSystem>();
        if (playerTransform == null && weaponSystem != null)
            playerTransform = weaponSystem.transform;

        SetupRing();
        if (ring != null) ring.enabled = false;
    }

    void SetupRing()
    {
        if (ring == null) return;
        ring.positionCount = segments + 1;
        ring.loop = false;
        ring.startWidth = 0.08f;
        ring.endWidth = 0.08f;
        ring.startColor = Color.yellow;
        ring.endColor = Color.yellow;
        ring.useWorldSpace = true;
        ring.sortingOrder = 10;
    }

    void Update()
    {
        if (weaponSystem == null || playerTransform == null) return;

        if (weaponSystem.isReloading)
        {
            ring.enabled = true;
            DrawArc(weaponSystem.reloadProgress, playerTransform.position);
        }
        else
        {
            ring.enabled = false;
        }
    }

    void DrawArc(float progress, Vector3 center)
    {
        int pointCount = Mathf.Max(2, Mathf.RoundToInt(progress * segments));
        ring.positionCount = pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            float angle = Mathf.Lerp(90f, 90f - 360f, (float)i / segments) * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            ring.SetPosition(i, center + new Vector3(x, y, 0f));
        }
    }
}