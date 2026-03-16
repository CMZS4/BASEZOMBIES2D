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

            currentWave++;
            Debug.Log("=== WAVE " + currentWave + " BAŞLADI ===");

            bool isBossWave = currentWave % 10 == 0 && bossCount < 8 && bossZombiePrefab != null;

            float waveTime = isBossWave ? 120f :
                Mathf.Min(minWaveTime + (currentWave - 1) * waveTimeIncrement, maxWaveTime);

            int zombieCount = baseZombieCount + (currentWave - 1) * 2;
            float spawnInterval = waveTime / zombieCount;

            waveTimeRemaining = waveTime;
            waveActive = true;

            Debug.Log($"Wave {currentWave} | Süre: {waveTime}s | Zombie: {zombieCount} | Interval: {spawnInterval:F1}s");

            if (isBossWave)
            {
                bossAlive = true;
                bossCount++;
                Transform window = windows[Random.Range(0, windows.Length)];
                Instantiate(bossZombiePrefab, window.position, Quaternion.identity);
                zombiesAlive++;
                Debug.Log("BOSS SPAWNED! Boss #" + bossCount);
            }

            yield return StartCoroutine(SpawnWave(zombieCount, spawnInterval));
            yield return new WaitUntil(() => !waveActive);

            Debug.Log("=== WAVE " + currentWave + " BİTTİ ===");

            if (currentWave % 5 == 0)
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
        // Wave 15+: Tank ekle (%30)
        if (currentWave >= 15 && tankZombiePrefab != null && Random.value < 0.3f)
            return tankZombiePrefab;

        // Wave 10+: Smoker ekle (%20)
        if (currentWave >= 10 && smokerZombiePrefab != null && Random.value < 0.2f)
            return smokerZombiePrefab;

        // Wave 5+: Runner ekle (wave başına %10, max %50)
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
        Debug.Log("Boss öldü!");
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
            Debug.Log("Barikat kuruldu! Pencere " + windowIndex + " 30 saniye kapalı.");
        }
    }
}