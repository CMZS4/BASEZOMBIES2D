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
    public int totalKills = 0; // YENİ
    public float waveTimeRemaining = 0f;
    public bool waveActive = false;

    bool waitingForClaim = false;
    bool bossAlive = false;
    float baseWaveTime = 90f;
    float minWaveTime = 60f;

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
            float waveTime = isBossWave ? 120f : Mathf.Max(baseWaveTime - (currentWave - 1) * 2f, minWaveTime);
            waveTimeRemaining = waveTime;
            waveActive = true;

            if (isBossWave)
            {
                bossAlive = true;
                bossCount++;
                Transform window = windows[Random.Range(0, windows.Length)];
                Instantiate(bossZombiePrefab, window.position, Quaternion.identity);
                zombiesAlive++;
                Debug.Log("BOSS SPAWNED! Boss #" + bossCount);
            }

            yield return StartCoroutine(SpawnWave());

            yield return new WaitUntil(() => !waveActive);

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
        float spawnInterval = 2f;
        float elapsed = 0f;

        while (waveActive)
        {
            elapsed += spawnInterval;
            if (elapsed > waveTimeRemaining + 5f) yield break;

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
            }

            yield return new WaitForSeconds(spawnInterval);
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
        totalKills++; // YENİ
    }

    public void OnBossDied()
    {
        zombiesAlive--;
        totalKills++; // YENİ
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