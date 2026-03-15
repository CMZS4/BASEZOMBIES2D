using UnityEngine;

public class Fragment : MonoBehaviour
{
    bool canPickup = false;

    void Start()
    {
        Invoke("EnablePickup", 0.3f); // 0.3 saniye sonra toplanabilir
    }

    void EnablePickup()
    {
        canPickup = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canPickup) return;
        
        if (other.CompareTag("Player"))
        {
            if (FragmentManager.instance != null)
                FragmentManager.instance.AddFragment();
            Destroy(gameObject);
        }
    }
}