using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public int healthPackCount = 0;
    public int barricadeCount = 0;

    PlayerController player;
    WaveSpawner waveSpawner;

    void Start()
    {
        player = GetComponent<PlayerController>();
        waveSpawner = FindObjectOfType<WaveSpawner>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            UseHealthPack();

        if (Input.GetKeyDown(KeyCode.B))
            PlaceBarricade();
    }

    public void AddHealthPack()
    {
        healthPackCount++;
        Debug.Log("Sağlık paketi alındı! Toplam: " + healthPackCount);
    }

    public void AddBarricade()
    {
        barricadeCount++;
        Debug.Log("Barikat alındı! Toplam: " + barricadeCount);
    }

    public void UseHealthPack()
    {
        if (healthPackCount <= 0) { Debug.Log("Sağlık paketi yok!"); return; }
        if (player.currentHealth >= player.maxHealth) { Debug.Log("Can zaten dolu!"); return; }

        healthPackCount--;
        player.currentHealth = Mathf.Min(player.currentHealth + 3, player.maxHealth);
        Debug.Log("Sağlık paketi kullanıldı! Can: " + player.currentHealth);
    }

    void PlaceBarricade()
    {
        if (barricadeCount <= 0) { Debug.Log("Barikat yok!"); return; }

        // En yakın pencereyi bul
        int closestWindow = -1;
        float closestDist = 3f; // max 3 birim mesafe

        for (int i = 0; i < waveSpawner.windows.Length; i++)
        {
            float dist = Vector2.Distance(transform.position, waveSpawner.windows[i].position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestWindow = i;
            }
        }

        if (closestWindow >= 0)
        {
            barricadeCount--;
            waveSpawner.PlaceBarricade(closestWindow);
            Debug.Log("Barikat kuruldu! Pencere " + closestWindow);
        }
        else
        {
            Debug.Log("Yakında pencere yok!");
        }
    }
}