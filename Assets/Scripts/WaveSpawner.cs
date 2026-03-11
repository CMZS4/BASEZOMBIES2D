using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject basicZombiePrefab;
    public GameObject runnerZombiePrefab;
    public GameObject tankZombiePrefab;
    public GameObject smokerZombiePrefab;
    public GameObject bossZombiePrefab;
    public Transform[] windows;

    public int currentWave = 0;
    public int zombiesAlive = 0;
    public int bossCount = 0;

    bool waitingForClaim = false;
    bool bossAlive = false;
    ClaimUI claimUI;

    void Start()
    {
        claimUI = FindObjectOfType<ClaimUI>();
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (waitingForClaim) { yield return null; continue; }

            currentWave++;
            Debug.Log("=== WAVE " + currentWave + " BAŞLADI ===");

            // Her 10 wave'de boss spawn et
            if (currentWave % 10 == 0 && bossCount < 8 && bossZombiePrefab != null)
            {
                bossAlive = true;
                bossCount++;
                Transform window = windows[Random.Range(0, windows.Length)];
                Instantiate(bossZombiePrefab, window.position, Quaternion.identity);
                zombiesAlive++;
                Debug.Log("BOSS SPAWNED! Boss #" + bossCount);
            }

            yield return StartCoroutine(SpawnWave());

            yield return new WaitUntil(() => zombiesAlive <= 0 && !bossAlive);

            Debug.Log("=== WAVE " + currentWave + " BİTTİ ===");

            if (currentWave % 5 == 0)
            {
                waitingForClaim = true;
                claimUI.ShowClaimScreen();
            }
            else
            {
                yield return new WaitForSeconds(3f);
            }
        }
    }

    IEnumerator SpawnWave()
    {
        int zombieCount = 4;

        for (int i = 0; i < zombieCount; i++)
        {
            Transform window = windows[Random.Range(0, windows.Length)];
            GameObject prefab = GetZombiePrefab();
            Instantiate(prefab, window.position, Quaternion.identity);
            zombiesAlive++;
            yield return new WaitForSeconds(1f);
        }
    }

    GameObject GetZombiePrefab()
    {
        if (currentWave >= 15 && tankZombiePrefab != null && Random.value < 0.3f)
            return tankZombiePrefab;

        if (currentWave > 10 && smokerZombiePrefab != null && Random.value < 0.5f)
            return smokerZombiePrefab;

        if (currentWave > 5 && runnerZombiePrefab != null && Random.value < 0.5f)
            return runnerZombiePrefab;

        return basicZombiePrefab;
    }

    public void OnZombieDied()
    {
        zombiesAlive--;
    }

    public void OnBossDied()
    {
        zombiesAlive--;
        bossAlive = false;
        Debug.Log("Boss öldü!");
    }

    public void ContinueGame()
    {
        waitingForClaim = false;
    }
}