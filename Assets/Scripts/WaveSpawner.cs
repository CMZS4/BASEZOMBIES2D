using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject basicZombiePrefab;
    public GameObject runnerZombiePrefab;
    public GameObject tankZombiePrefab;
    public Transform[] windows;

    public int currentWave = 0;
    public int zombiesAlive = 0;

    bool waitingForClaim = false;
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
            if (waitingForClaim)
            {
                yield return null;
                continue;
            }

            currentWave++;
            Debug.Log("=== WAVE " + currentWave + " BAŞLADI ===");

            yield return StartCoroutine(SpawnWave());

            yield return new WaitUntil(() => zombiesAlive <= 0);

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

        if (currentWave > 5 && runnerZombiePrefab != null && Random.value < 0.5f)
            return runnerZombiePrefab;

        return basicZombiePrefab;
    }

    public void OnZombieDied()
    {
        zombiesAlive--;
    }

    public void ContinueGame()
    {
        waitingForClaim = false;
    }
}