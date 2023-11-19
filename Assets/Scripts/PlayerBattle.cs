//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//
//public class PlayerBattle : MonoBehaviour
//{
//    private TurnBasedSystem turnSystem;
//
//    private void Start()
//    {
//        turnSystem = GameObject.FindObjectOfType<TurnBasedSystem>();
//    }
//
//    private void Update()
//    {
//        //if (!isLocalPlayer) return;
//
//        if (turnSystem.playerTurn && turnSystem.currentCharacter == gameObject)
//        {
//            // Обработка хода игрока
//            if (Input.GetKeyDown(KeyCode.Space))
//            {
//                // Завершаем ход игрока и начинаем битву с другим игроком
//                CmdStartPlayerBattle();
//            }
//        }
//    }
//
//    [Command]
//    private void CmdStartPlayerBattle()
//    {
//        // Начало битвы с другим игроком
//        // Вы можете добавить здесь логику битвы между игроками, например, установив флаг входа в битву и синхронизировав его с клиентами
//        RpcStartPlayerBattle();
//    }
//
//    [ClientRpc]
//    private void RpcStartPlayerBattle()
//    {
//        // Запуск битвы на клиенте
//        // Здесь вы можете активировать боевой режим, отключить управление игрока и т. д.
//    }
//}
//