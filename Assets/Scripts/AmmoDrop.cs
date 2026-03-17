using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    public AmmoType ammoType;
    public int amount;
    float lifetime = 15f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponSystem ws = other.GetComponent<WeaponSystem>();
            if (ws != null)
            {
                ws.AddAmmo(ammoType, amount);
                Destroy(gameObject);
            }
        }
    }
}