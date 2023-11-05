using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class EnemyScript : NetworkBehaviour
{
    [Header("Действия")]
    public List<ListAction> actionList = new List<ListAction>();
    [Header("Инициализация")]
    public GameObject player;
    public int moveSpeed;
    public float attackRange;
    public float returnSpeed;
    public Animator animator;
    [Header("Параметры")]
    public int currentHealth;
    public int armor;
    public int armored; 
    public int damage;

    public Vector3 originalPosition;
    private List<Actions> actionsToPerform = new List<Actions>();
    public bool isPerformingActions = false;
    private int random = 0;

    public void Awake()
    {
        if (this.gameObject.GetComponent<Animator>() != null) animator = this.gameObject.GetComponent<Animator>();
    }
        public void Start()
    {
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        ShuffleActionList();
        random = Random.Range(0, actionList.Count);
        actionsToPerform.AddRange(actionList[random].action);
        // Получите компонент аниматора
    }

    private void ShuffleActionList()
    {
        for (int i = 0; i < actionList.Count; i++)
        {
            int randomIndex = Random.Range(i, actionList.Count);
            ListAction temp = actionList[i];
            actionList[i] = actionList[randomIndex];
            actionList[randomIndex] = temp;
        }
    }

    private void Update()
    {
        //if (!isLocalPlayer) return;
        {
            foreach (GameObject enemy in player.GetComponent<Player>().turn.enemies)
            {

                if (!player.GetComponent<Player>().turn.playerTurn && !isPerformingActions)
                {
                    StartCoroutine(PerformActions());
                }
            }
        }
    }



    private IEnumerator PerformActions()
    {
        isPerformingActions = true;
                   foreach (Actions action in actionsToPerform)
            {
                if (action == Actions.Attack)
                {
                    // Устанавливаем параметры аниматора

                    // Подойти к игроку
                    float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                    if (distanceToPlayer > attackRange)
                    {
                        animator.SetInteger("State", 2);

                        yield return StartCoroutine(MoveToPlayer(player.transform.position));
                    }
                    else
                    {
                        yield return new WaitForSeconds(1.0f);
                    }
                    animator.SetTrigger("Attack");

                // Сбрасываем параметры аниматора
                    animator.SetInteger("State", 0);

            }
            PerformAction(action);

                // Добавьте общую задержку (2 секунды) здесь
                yield return new WaitForSeconds(1.0f); // Общая задержка

                // Сбрасываем параметры аниматора после атаки
            }

            actionsToPerform.Clear();
            ShuffleActionList();
            random = Random.Range(0, actionList.Count);
            actionsToPerform.AddRange(actionList[random].action);

            // Включаем анимацию возвращения
            animator.SetInteger("State", 2);

            // Вернуться на исходную позицию
            yield return StartCoroutine(MoveToOriginalPosition());

            // Отключаем анимацию возвращения
            animator.SetInteger("State", 0);
            player.GetComponent<TurnBasedSystem>().EndCurrentTurn();
            player.GetComponent<TurnBasedSystem>().StartNextTurn();
            isPerformingActions = false;
            player.GetComponent<Player>().turn.playerTurn = true;
    }



    private void PerformAction(Actions action)
    {
        switch (action)
        {
            case Actions.Attack:
                Debug.Log("Враг атакует!");
                player.GetComponent<Player>().TakeDamage(damage);
                break;

            case Actions.Block:
                Debug.Log("Враг блокирует!");
                armored += armor;
                break;
        }
    }

    private IEnumerator MoveToPlayer(Vector3 playerPosition)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        while (distanceToPlayer > attackRange)
        {
            // Рассчитываем направление к игроку
            Vector3 direction = (playerPosition - transform.position).normalized;

            // Двигаем врага в этом направлении
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Обновляем расстояние до игрока
            distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
            yield return null;
        }

    }

    private IEnumerator MoveToOriginalPosition()
    {
        float distanceToOriginalPosition = Vector3.Distance(transform.position, originalPosition);

        while (distanceToOriginalPosition > 0.1f)
        {
            // Рассчитываем направление к исходной позиции
            Vector3 direction = (originalPosition - transform.position).normalized;

            // Двигаем врага в этом направлении
            transform.position += direction * returnSpeed * Time.deltaTime;

            // Обновляем расстояние до исходной позиции
            distanceToOriginalPosition = Vector3.Distance(transform.position, originalPosition);

            yield return null;
        }
    }

    public void TakeDamage(int damagePlayer)
    {
        int finaldamagePlayer = damagePlayer;
        if (gameObject != null)
        {
            if (armored >= damage)
            {
                // Броня поглощает весь урон
                armored -= finaldamagePlayer; // Уменьшаем броню
                finaldamagePlayer = 0;
            }
            else
            {
                // Броня не может поглотить весь урон
                finaldamagePlayer -= armored; // Урон уменьшается на количество брони
                armored = 0;
            }
            Debug.Log(armored + " " + finaldamagePlayer);
            currentHealth -= finaldamagePlayer;

            if (currentHealth <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    private IEnumerator Die()
    {
        animator.SetInteger("State", 9);
        this.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}

public enum Actions
{
    Attack,
    Block
}

[System.Serializable]
public class ListAction
{
    public Actions[] action;
}
