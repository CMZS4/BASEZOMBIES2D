using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int healthPackCount = 0;
    PlayerController player;

    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            UseHealthPack();
    }

    public void AddHealthPack()
    {
        healthPackCount++;
        Debug.Log("Sağlık paketi alındı! Toplam: " + healthPackCount);
    }

    public void UseHealthPack()
    {
        if (healthPackCount <= 0)
        {
            Debug.Log("Sağlık paketi yok!");
            return;
        }

        if (player.currentHealth >= player.maxHealth)
        {
            Debug.Log("Can zaten dolu!");
            return;
        }

        healthPackCount--;
        player.currentHealth = Mathf.Min(player.currentHealth + 3, player.maxHealth);
        Debug.Log("Sağlık paketi kullanıldı! Can: " + player.currentHealth + "/" + player.maxHealth);
    }
}