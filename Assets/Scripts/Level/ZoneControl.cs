using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneControl : MonoBehaviour
{

    [SerializeField] private List<GameObject> enemys;
    [SerializeField] private int maxEnemyInZone;
    [SerializeField] private int enemysInstantiate = 0;
    [SerializeField] private float rangeToSpawn;

    [SerializeField] private List<GameObject> enemysSpawned;
    [SerializeField] private LayerMask target;
    [SerializeField] private Transform rayStartPos;
    [SerializeField] private float rayCastRange;


    [SerializeField] private WallBehavior wall;

    //TPZone
    [SerializeField] private TeleportBehavior tpZone;

    public int EnemysInstantiate{ get => enemysInstantiate; set => enemysInstantiate = value; }

    #region Normal Enemys
    public void StartSpawnEnemys()
    {
        StartCoroutine(SpawnEnemys());
    }
    IEnumerator SpawnEnemys()
    {
        yield return new WaitForSeconds(2);

        while(enemysInstantiate < maxEnemyInZone)
        {
            foreach (var enemy in enemys)
            {
                float x = Random.Range(transform.position.x - rangeToSpawn, transform.position.x + rangeToSpawn);
                Instantiate(enemy, new Vector3(x, 0, 2), Quaternion.identity);
                enemysInstantiate++;

                yield return new WaitForSeconds(.5f);
            }
        }

        RaycastHit2D[] hit = Physics2D.RaycastAll(rayStartPos.position, rayStartPos.right, rayCastRange, target);

        if(hit != null)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                if (enemysSpawned.IndexOf(hit[i].collider.gameObject) >= 0)
                    Debug.Log("Enemy ja foi add");
                else
                    enemysSpawned.Add(hit[i].collider.GameObject());
            }
        }
    }
    public void PlayerOutEnemyZone()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayStartPos.position, rayStartPos.right, rayCastRange, target);

        if (hit != null)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                Destroy(hit[i].collider.GameObject());
            }
        }
        enemysSpawned.Clear();
        enemysInstantiate = 0;
    }
    public void EnemyInTheZoneDie(GameObject enemyDead)
    {
        enemysSpawned.Remove(enemyDead);
        enemysInstantiate--;

        StartCoroutine(SpawnEnemys());
    }
    #endregion

    public void BossIsDead()
    {
        wall.BossIsDead = true;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(rayStartPos.position, rayStartPos.right * rayCastRange, Color.yellow);
    }

}