using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool canSpawn = true;
    public float spawnIntervalInSeconds = 4f;
    private float spawnInterval = 0f;
    public float enemySpeedOnSpawn;
    public float enemyHealthOnSpawn;
    private float savedTime = 0f;
    private int lastUsedEnemyID = 0;

    public GameObject enemyObject;
    public Transform nextPosition;

    private void Update()
    {
        if (spawnIntervalInSeconds + savedTime < Time.time && canSpawn)
        {
            Enemy e = Instantiate(enemyObject, transform.position, transform.rotation).GetComponent<Enemy>();
            e.target = nextPosition;
            e.speed = enemySpeedOnSpawn;
            e.health = enemyHealthOnSpawn;

            lastUsedEnemyID++;
            e.id = lastUsedEnemyID;
            GameManager.AskFor.enemies.Add(e);

            float tmp;
            if (lastUsedEnemyID % 2 == 0)
            {
                // enemy speed increase per spawn
                enemySpeedOnSpawn += 0.01f;
                if (enemyHealthOnSpawn < 120)
                    enemyHealthOnSpawn++;
                if (enemyHealthOnSpawn >= 120)
                    enemyHealthOnSpawn+=2;
                //only increase every second enemy // skal være exponential
            }
            // spawn rate increase
            tmp = spawnIntervalInSeconds / 80f;
            if (spawnIntervalInSeconds >= 0.14f)
                spawnIntervalInSeconds -= tmp; //temporary way of increasing dificulty
            savedTime = Time.time;
        }
    }


}
