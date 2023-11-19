using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedSystem : MonoBehaviour
{
   
    public List<GameObject> players; // Список игроков
    public List<GameObject> enemies; // Список врагов
    private int currentTurnIndex = 0; // Индекс текущего активного персонажа
     public bool playerTurn = true; // Флаг, определяющий, чей ход (true - игрок, false - враг)
     public GameObject currentCharacter; // Текущий активный персонаж

    public void Start()
    {
        // Начинаем с первого игрока
        currentTurnIndex = 0;
        playerTurn = true;
        currentCharacter = players[currentTurnIndex];
    }

 
    public void CmdEndCurrentTurn()
    {
        if (playerTurn)
        {
            playerTurn = false;
        }
        else
        {
            playerTurn = true;
            currentTurnIndex++;
            if (currentTurnIndex >= players.Count)
            {
                currentTurnIndex = 0;
            }
            currentCharacter = players[currentTurnIndex];
        }
    }


    public void CmdStartNextTurn()
    {
        // Проверяем, чей сейчас ход: игрока или врага
        if (playerTurn)
        {
            // Если сейчас ход игроков, завершаем его
            CmdEndCurrentTurn();
        }
        else
        {
            // Если сейчас ход врагов, переключаемся на следующего игрока
            CmdEndCurrentTurn();
            CmdStartNextTurn();
        }
    }
    

    public void EndCurrentTurn()
    {
        //if (!isLocalPlayer) return;
        if (playerTurn)
        {
            playerTurn = false;
            if(enemies.Count > 0)enemies[0].GetComponent<EnemyScript>().armored = 0;
        }
        else
        {
            playerTurn = false;
            if (players[0] != null) players[0].GetComponent<Player>().armored = 0;
        }
    }

    public void StartNextTurn()
    {
        //if (!isLocalPlayer) return;
        // Увеличиваем индекс текущего персонажа
        currentTurnIndex++;

        if (playerTurn)
        {
            if (currentTurnIndex < players.Count)
            {
                currentCharacter = players[currentTurnIndex];
            }
            else
            {
                playerTurn = false; // Завершили ходы игроков, начинаем ходы врагов
                currentTurnIndex = 0;
                currentCharacter = enemies[currentTurnIndex];
            }
        }
        else
        {
            if (currentTurnIndex < enemies.Count)
            {
                currentCharacter = enemies[currentTurnIndex];
            }
            else
            {
                playerTurn = true; // Завершили ходы врагов, начинаем ходы игроков
                currentTurnIndex = 0;
                currentCharacter = players[currentTurnIndex];
            }
        }

        if (playerTurn)
        {
            Debug.Log("Ход игрока " + currentTurnIndex);
            players[0].GetComponent<Player>().actionPoints = players[0].GetComponent<Player>().maxActionPoints;
            players[0].GetComponent<Player>().ap.UpdateActionPoints(players[0].GetComponent<Player>().actionPoints);
        }
        else
        {
            Debug.Log("Ход врага " + currentTurnIndex);
        }
    }
}
