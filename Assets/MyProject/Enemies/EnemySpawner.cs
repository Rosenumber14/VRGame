using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public Enemy EnemyPrefab;
    public Transform TargetDestination;

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
        var enemy = (Enemy)Instantiate(EnemyPrefab, transform.position, transform.rotation);
        enemy.TargetDestination = TargetDestination;
        StartCoroutine(SpawnEnemy());
    }
}
