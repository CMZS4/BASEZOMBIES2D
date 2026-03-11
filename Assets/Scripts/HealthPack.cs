using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int healAmount = 3;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            InventorySystem inventory = other.GetComponent<InventorySystem>();
            
            if (inventory != null)
            {
                inventory.AddHealthPack();
                Destroy(gameObject);
            }
        }
    }
}