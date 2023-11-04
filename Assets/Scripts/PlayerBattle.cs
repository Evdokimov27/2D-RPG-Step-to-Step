using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBattle : NetworkBehaviour
{
    private TurnBasedSystem turnSystem;

    private void Start()
    {
        turnSystem = GameObject.FindObjectOfType<TurnBasedSystem>();
    }

    private void Update()
    {
        //if (!isLocalPlayer) return;

        if (turnSystem.playerTurn && turnSystem.currentCharacter == gameObject)
        {
            // ��������� ���� ������
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // ��������� ��� ������ � �������� ����� � ������ �������
                CmdStartPlayerBattle();
            }
        }
    }

    [Command]
    private void CmdStartPlayerBattle()
    {
        // ������ ����� � ������ �������
        // �� ������ �������� ����� ������ ����� ����� ��������, ��������, ��������� ���� ����� � ����� � ��������������� ��� � ���������
        RpcStartPlayerBattle();
    }

    [ClientRpc]
    private void RpcStartPlayerBattle()
    {
        // ������ ����� �� �������
        // ����� �� ������ ������������ ������ �����, ��������� ���������� ������ � �. �.
    }
}
