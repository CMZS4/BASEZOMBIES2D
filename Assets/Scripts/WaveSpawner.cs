using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{
    public GameObject basicZombiePrefab;
    public GameObject runnerZombiePrefab;
    public GameObject tankZombiePrefab;
    public GameObject smokerZombiePrefab;
    public GameObject bossZombiePrefab;
    public GameObject boss2ZombiePrefab;
    public GameObject boss3ZombiePrefab;
    public Transform[] windows;

    public int currentWave = 0;
    public int zombiesAlive = 0;
    public int bossCount = 0;
    public int totalKills = 0;
    public float waveTimeRemaining = 0f;
    public bool waveActive = false;

    bool waitingForClaim = false;
    bool bossAlive = false;
    float minWaveTime = 60f;
    float maxWaveTime = 90f;
    float waveTimeIncrement = 2f;
    int baseZombieCount = 30;

    ClaimUI claimUI;
    public float[] windowBarricadeTimers;

    void Start()
    {
        claimUI = FindObjectOfType<ClaimUI>();
        windowBarricadeTimers = new float[windows.Length];
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        for (int i = 0; i < windowBarricadeTimers.Length; i++)
        {
            if (windowBarricadeTimers[i] > 0)
                windowBarricadeTimers[i] -= Time.deltaTime;
        }

        if (waveActive)
        {
            waveTimeRemaining -= Time.deltaTime;
            if (waveTimeRemaining <= 0)
            {
                waveTimeRemaining = 0;
                waveActive = false;
            }
        }
    }

    IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (waitingForClaim) { yield return null; continue; }

            // Max 40 wave
            if (currentWave >= 40)
            {
                Debug.Log("=== DEMO BITTI ===");
                // TODO: Demo bitis ekrani goster
                yield break;
            }

            currentWave++;
            Debug.Log("=== WAVE " + currentWave + " BASLADI ===");

            bool isBossWave = currentWave == 10 || currentWave == 20 || 
                              currentWave == 30 || currentWave == 40;

            float waveTime = isBossWave ? 120f :
                Mathf.Min(minWaveTime + (currentWave - 1) * waveTimeIncrement, maxWaveTime);

            int zombieCount = baseZombieCount + (currentWave - 1) * 2;
            float spawnInterval = waveTime / zombieCount;

            waveTimeRemaining = waveTime;
            waveActive = true;

            // Boss spawn
            if (isBossWave)
            {
                bossAlive = true;
                bossCount++;
                SpawnBoss();
            }

            yield return StartCoroutine(SpawnWave(zombieCount, spawnInterval));
            yield return new WaitUntil(() => !waveActive);

            Debug.Log("=== WAVE " + currentWave + " BITTI ===");

            // Claim mantigi: Wave 5 ve her boss waveden sonra
            bool shouldClaim = currentWave == 5 || isBossWave;

            if (shouldClaim)
            {
                waitingForClaim = true;
                claimUI.ShowClaimScreen();
            }
            else
            {
                yield return new WaitForSeconds(5f);
            }
        }
    }

    void SpawnBoss()
    {
        if (currentWave == 10 && bossZombiePrefab != null)
        {
            // Boss 1
            Transform window = windows[Random.Range(0, windows.Length)];
            Instantiate(bossZombiePrefab, window.position, Quaternion.identity);
            zombiesAlive++;
            Debug.Log("BOSS 1 SPAWNED!");
        }
        else if (currentWave == 20 && boss2ZombiePrefab != null)
        {
            // Boss 2
            Transform window = windows[Random.Range(0, windows.Length)];
            Instantiate(boss2ZombiePrefab, window.position, Quaternion.identity);
            zombiesAlive++;
            Debug.Log("BOSS 2 SPAWNED!");
        }
        else if (currentWave == 30 && boss3ZombiePrefab != null)
        {
            // Boss 3
            Transform window = windows[Random.Range(0, windows.Length)];
            Instantiate(boss3ZombiePrefab, window.position, Quaternion.identity);
            zombiesAlive++;
            Debug.Log("BOSS 3 SPAWNED!");
        }
        else if (currentWave == 40)
        {
            // FINAL — Boss 1 + 2 + 3 aynı anda!
            Debug.Log("FINAL BOSS WAVE!");
            Transform[] spawnPoints = windows;

            if (bossZombiePrefab != null)
            {
                Instantiate(bossZombiePrefab, spawnPoints[0].position, Quaternion.identity);
                zombiesAlive++;
            }
            if (boss2ZombiePrefab != null)
            {
                Instantiate(boss2ZombiePrefab, spawnPoints[1 % spawnPoints.Length].position, Quaternion.identity);
                zombiesAlive++;
            }
            if (boss3ZombiePrefab != null)
            {
                Instantiate(boss3ZombiePrefab, spawnPoints[2 % spawnPoints.Length].position, Quaternion.identity);
                zombiesAlive++;
            }
        }
    }

    IEnumerator SpawnWave(int zombieCount, float spawnInterval)
    {
        int spawned = 0;

        while (waveActive && spawned < zombieCount)
        {
            List<int> availableWindows = new List<int>();
            for (int w = 0; w < windows.Length; w++)
            {
                if (windowBarricadeTimers[w] <= 0)
                    availableWindows.Add(w);
            }

            if (availableWindows.Count > 0)
            {
                int windowIndex = availableWindows[Random.Range(0, availableWindows.Count)];
                GameObject prefab = GetZombiePrefab();
                Instantiate(prefab, windows[windowIndex].position, Quaternion.identity);
                zombiesAlive++;
                spawned++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    GameObject GetZombiePrefab()
    {
        if (currentWave >= 15 && tankZombiePrefab != null && Random.value < 0.3f)
            return tankZombiePrefab;

        if (currentWave >= 10 && smokerZombiePrefab != null && Random.value < 0.2f)
            return smokerZombiePrefab;

        if (currentWave >= 5 && runnerZombiePrefab != null)
        {
            float runnerChance = Mathf.Min(0.10f + (currentWave - 5) * 0.05f, 0.50f);
            if (Random.value < runnerChance)
                return runnerZombiePrefab;
        }

        return basicZombiePrefab;
    }

    public void OnZombieDied()
    {
        zombiesAlive--;
        totalKills++;
    }

    public void OnBossDied()
    {
        zombiesAlive--;
        totalKills++;
        bossAlive = false;
        Debug.Log("Boss oldu!");
    }

    public void ContinueGame()
    {
        waitingForClaim = false;
    }

    public void PlaceBarricade(int windowIndex)
    {
        if (windowIndex >= 0 && windowIndex < windowBarricadeTimers.Length)
        {
            windowBarricadeTimers[windowIndex] = 30f;
        }
    }
}