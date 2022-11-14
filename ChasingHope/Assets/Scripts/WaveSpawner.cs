using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{ 
    public enum SpawnState { spawning, waiting, counting };
    
    [System.Serializable]
    public class Wave
    {
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;

    public Transform[] spawnPoints;

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCountdown = 2f;

    public SpawnState state = SpawnState.counting;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {
        if (state == SpawnState.waiting)
        {
            // Check if enemies are still alive
            if (!EnemyIsAlive())
            {
                // Begin new round
                BeginNewRound();
                return;
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.spawning)
            {
                // start spawning
                StartCoroutine( SpawnWave ( waves[nextWave] ));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        } 
    }

    void BeginNewRound()
    {
        Debug.Log ("Wave Completed!");

        state = SpawnState.counting;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log ("Completed all waves.");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive ()
    {
        searchCountdown -= Time.deltaTime;

        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave!");
        state = SpawnState.spawning;
        
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds( 1f/_wave.rate );
        }

        state = SpawnState.waiting;

        yield break;
    }

    void SpawnEnemy (Transform _enemy)
    {
        // Spawn enemy
        Debug.Log ("Spawning Enemy: " + _enemy.name);

        if (spawnPoints.Length == 0)
        {
            Debug.Log("No spawnpoints referenced.");
        }

        Transform _sp = spawnPoints[ Random.Range (0, spawnPoints.Length) ];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

}
