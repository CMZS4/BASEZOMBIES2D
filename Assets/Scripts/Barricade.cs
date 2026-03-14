using UnityEngine;

public class Barricade : MonoBehaviour
{
    public int windowIndex = -1;
    WaveSpawner waveSpawner;
    float duration = 30f;
    float timer;

    void Start()
    {
        waveSpawner = FindObjectOfType<WaveSpawner>();
        timer = duration;

        if (windowIndex >= 0)
            waveSpawner.PlaceBarricade(windowIndex);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Destroy(gameObject);
    }
}