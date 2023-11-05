using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BattleTrigger : NetworkBehaviour
{
    private GameObject nearestPlayer = null;
    public float battleDistance = 2.0f; // Расстояние, при котором начинается бой
    private GameObject player; // Ссылка на игрока

    public void Start()
    {
        player = GameObject.FindWithTag("Player"); // Найдем игрока по тегу
    }

    private void Update()
    {
        //if (!isLocalPlayer) return;
        {
            if (player == null)
            {
                return;
            }
            // Найдем ближайшего игрока в сцене
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            nearestPlayer = null;
            float nearestPlayerDistance = float.MaxValue;

            foreach (GameObject otherPlayer in players)
            {
                if (otherPlayer == player) continue; // Пропустить себя

                float playerDistance = Vector3.Distance(player.transform.position, otherPlayer.transform.position);
                if (playerDistance < nearestPlayerDistance)
                {
                    nearestPlayerDistance = playerDistance;
                    nearestPlayer = otherPlayer;
                }
            }

            // Проверим, если расстояние до ближайшего игрока меньше battleDistance и ближайший игрок не в бою, начнем бой
            if (nearestPlayer != null && nearestPlayerDistance < battleDistance)
            {
                if (!player.GetComponent<Player>().inBattle)
                {
                    // Начать бой с ближайшим игроком
                    StartBattleWithPlayer(nearestPlayer);
                }
            }
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            {
                GameObject nearestEnemy = null;
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

                // Проверим, если расстояние до ближайшего врага меньше battleDistance, начнем бой
                if (nearestEnemy != null && nearestDistance < battleDistance)
                {
                    if (!player.GetComponent<Player>().inBattle)
                    {
                        nearestEnemy.GetComponent<EnemyScript>().enabled = true;
                        player.GetComponent<Player>().StartBattle();
                        player.GetComponent<Player>().turn.enemies.Add(nearestEnemy);
                    }
                }
                else
                {
                    player.GetComponent<Player>().turn.enemies.Clear();
                    player.GetComponent<Player>().EndBattle();
                }

            }
        }
       

    }
    public void StartBattleWithPlayer(GameObject otherPlayer)
    {
        player.GetComponent<Player>().StartBattle();
        player.GetComponent<Player>().turn.enemies.Add(otherPlayer);
    }
}
