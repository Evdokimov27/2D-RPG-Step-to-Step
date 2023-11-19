using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedSystem : MonoBehaviour
{
   
    public List<GameObject> players; // ������ �������
    public List<GameObject> enemies; // ������ ������
    private int currentTurnIndex = 0; // ������ �������� ��������� ���������
     public bool playerTurn = true; // ����, ������������, ��� ��� (true - �����, false - ����)
     public GameObject currentCharacter; // ������� �������� ��������

    public void Start()
    {
        // �������� � ������� ������
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
        // ���������, ��� ������ ���: ������ ��� �����
        if (playerTurn)
        {
            // ���� ������ ��� �������, ��������� ���
            CmdEndCurrentTurn();
        }
        else
        {
            // ���� ������ ��� ������, ������������� �� ���������� ������
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
        // ����������� ������ �������� ���������
        currentTurnIndex++;

        if (playerTurn)
        {
            if (currentTurnIndex < players.Count)
            {
                currentCharacter = players[currentTurnIndex];
            }
            else
            {
                playerTurn = false; // ��������� ���� �������, �������� ���� ������
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
                playerTurn = true; // ��������� ���� ������, �������� ���� �������
                currentTurnIndex = 0;
                currentCharacter = players[currentTurnIndex];
            }
        }

        if (playerTurn)
        {
            Debug.Log("��� ������ " + currentTurnIndex);
            players[0].GetComponent<Player>().actionPoints = players[0].GetComponent<Player>().maxActionPoints;
            players[0].GetComponent<Player>().ap.UpdateActionPoints(players[0].GetComponent<Player>().actionPoints);
        }
        else
        {
            Debug.Log("��� ����� " + currentTurnIndex);
        }
    }
}
