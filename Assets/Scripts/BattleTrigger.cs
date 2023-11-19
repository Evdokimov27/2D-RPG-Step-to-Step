using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    public GameObject nearestEnemy = null;
    public float battleDistance = 2.0f; // Расстояние, при котором начинается бой
    public GameObject player; // Ссылка на игрока

    public void Start()
    {
        player = GameObject.FindWithTag("Player"); // Найдем игрока по тегу
    }

    private void FixedUpdate()
    {
        //if (!isLocalPlayer) return;
        {
            if (!player.GetComponent<Player>().isHub)
            {
                if (player == null)
                {
                    return;
                }

                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                {
                    float nearestDistance = float.MaxValue;

                    foreach (GameObject enemy in enemies)
                    {
                        float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestEnemy = enemy;
                            player.GetComponent<Player>().nearestEnemy = nearestEnemy.GetComponent<EnemyScript>();

                        }
                    }


                    if (nearestEnemy != null && nearestDistance < battleDistance)
                    {
                        if (!player.GetComponent<Player>().inBattle)
                        {
                            nearestEnemy.GetComponent<EnemyScript>().enabled = true;
                            player.GetComponent<Player>().StartBattle();
                            player.GetComponent<Player>().turn.enemies.Add(nearestEnemy);
                        }
                    }

                }
            }
        }
    }
}
