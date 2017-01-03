using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {
    public Enemy EnemyPrefab;
    public Transform TargetDestination;
    const int MAX_ENEMIES = 1;

    private List<Enemy> enemies = new List<Enemy>();
    // Use this for initialization
    void Start () {
        var targetPosition = TargetDestination.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
        StartCoroutine(SpawnEnemy());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //for now just spawn one every 5 seconds
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(5);
        if (enemies.Count < MAX_ENEMIES)
        {
            var enemy = (Enemy)Instantiate(EnemyPrefab, transform.position, transform.rotation);
            enemy.Spawner = this;
            enemy.Died += Died;
            enemies.Add(enemy);
            enemy.TargetDestination = TargetDestination;
        }
        StartCoroutine(SpawnEnemy());
    }

    void Died(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}
