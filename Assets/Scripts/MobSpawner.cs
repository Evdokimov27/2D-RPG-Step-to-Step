using UnityEngine;
using System.Collections.Generic;

public class MobSpawner : MonoBehaviour
{
    public List<EnemyScript> mobs;
    public Transform[] spawnPoints;
    public Transform bossSpawnPoint;
    private int playerLevel;
    public float emptySpawnChance = 0.5f; // Шанс того, что точка спавна останется пустой (30%)
    private int minMobsOnMap = 3;

    void Start()
    {
        // Пример получения уровня игрока, здесь нужно использовать вашу собственную логику
        playerLevel = GetPlayerLevel();
        SpawnMobs();
        SpawnBoss();
    }

    int GetPlayerLevel()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        return player.GetComponent<Player>().level;
    }


    void SpawnMobs()
    {
        int mobsSpawned = 0;
       
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (Random.value > emptySpawnChance || mobsSpawned < minMobsOnMap)
            {
                EnemyScript mobToSpawn = ChooseMobForLevel(playerLevel);
                if (mobToSpawn != null)
                {
                    Instantiate(mobToSpawn.gameObject, spawnPoint.position, Quaternion.identity);
                    mobsSpawned++;
                }
            }
        }

        // Дополнительный спавн мобов, если не достигнуто минимальное количество
        while (mobsSpawned < minMobsOnMap)
        {
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            EnemyScript mobToSpawn = ChooseMobForLevel(playerLevel);
            Instantiate(mobToSpawn.gameObject, randomSpawnPoint.position, Quaternion.identity);
            mobsSpawned++;
        }

        GameObject.FindWithTag("Chest").GetComponent<ChestController>().maxSlot = mobsSpawned * 2;
        GameObject.FindWithTag("Chest").GetComponent<ChestController>().minSlot = 1;
    }
    

    EnemyScript ChooseMobForLevel(int level)
    {
        List<EnemyScript> suitableMobs = mobs.FindAll(m => m.levelRequired <= level && !m.isBoss);
        if (suitableMobs.Count > 0)
        {
            return suitableMobs[Random.Range(0, suitableMobs.Count)];
        }
        return null;
    }

    void SpawnBoss()
    {
        EnemyScript bossToSpawn = ChooseBossForLevel(playerLevel);
        if (bossToSpawn != null)
        {
            Instantiate(bossToSpawn.gameObject, bossSpawnPoint.position, Quaternion.identity);
        }
    }

    EnemyScript ChooseBossForLevel(int level)
    {
        List<EnemyScript> suitableBosses = mobs.FindAll(b => b.levelRequired <= level && b.isBoss);
        if (suitableBosses.Count > 0)
        {
            return suitableBosses[Random.Range(0, suitableBosses.Count)];
        }
        return null;
    }
}
