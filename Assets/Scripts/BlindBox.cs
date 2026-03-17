using UnityEngine;

public class BlindBox : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Envantere ekle
            int currentCount = PlayerPrefs.GetInt("BlindBoxCount", 0);
            PlayerPrefs.SetInt("BlindBoxCount", currentCount + 1);
            PlayerPrefs.Save();

            Debug.Log("Blind Box toplandı! Toplam: " + (currentCount + 1));
            Destroy(gameObject);
        }
    }
}