using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [Header("��������")]
    public List<ListAction> actionList = new List<ListAction>();
    [Header("�������������")]
    public GameObject player;
    public int levelRequired;
    public GameObject prefabHealthMob;
    public GameObject prefabHealthBoss;
    public GameObject healthCanvas;
    public int moveSpeed;
    public float attackRange;
    public float returnSpeed;
    public Animator animator;
    [Header("���������")]
    public bool isBoss;
    public int maxHealth;
    public int currentHealth;
    public int armor;
    public int armored; 
    public int damage;
    public int expForWin;


    public Vector3 originalPosition;
    private List<Actions> actionsToPerform = new List<Actions>();
    public bool isPerformingActions = false;
    private int random = 0;

    public void Awake()
    {
        currentHealth = maxHealth;
        if (this.gameObject.GetComponent<Animator>() != null) animator = this.gameObject.GetComponent<Animator>();
    }
        public void Start()
    {
        if (!isBoss) healthCanvas = Instantiate(prefabHealthMob, new Vector3(gameObject.transform.position.x, -5), Quaternion.identity);
        else healthCanvas = Instantiate(prefabHealthBoss, new Vector3(gameObject.transform.position.x, -5), Quaternion.identity);
        healthCanvas.transform.SetParent(transform);
        originalPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        ShuffleActionList();
        random = Random.Range(0, actionList.Count);
        actionsToPerform.AddRange(actionList[random].action);
        // �������� ��������� ���������
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
            if (healthCanvas != null)
            {
                healthCanvas.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = (float)currentHealth / maxHealth;
                healthCanvas.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = currentHealth + "/" + maxHealth;
                healthCanvas.transform.GetChild(0).GetChild(4).GetComponent<Text>().text = armored.ToString();
                healthCanvas.transform.GetChild(0).GetChild(6).GetComponent<Text>().text = damage.ToString();
            }
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
                    // ������������� ��������� ���������

                    // ������� � ������
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

                // ���������� ��������� ���������
                    animator.SetInteger("State", 0);

            }
            PerformAction(action);

                // �������� ����� �������� (2 �������) �����
                yield return new WaitForSeconds(1.0f); // ����� ��������

                // ���������� ��������� ��������� ����� �����
            }

            actionsToPerform.Clear();
            ShuffleActionList();
            random = Random.Range(0, actionList.Count);
            actionsToPerform.AddRange(actionList[random].action);

            // �������� �������� �����������
            animator.SetInteger("State", 2);

            // ��������� �� �������� �������
            yield return StartCoroutine(MoveToOriginalPosition());

            // ��������� �������� �����������
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
                Debug.Log("���� �������!");
                player.GetComponent<Player>().TakeDamage(damage);
                break;

            case Actions.Block:
                Debug.Log("���� ���������!");
                armored += armor;
                break;
        }
    }

    private IEnumerator MoveToPlayer(Vector3 playerPosition)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

        while (distanceToPlayer > attackRange)
        {
            // ������������ ����������� � ������
            Vector3 direction = (playerPosition - transform.position).normalized;

            // ������� ����� � ���� �����������
            transform.position += direction * moveSpeed * Time.deltaTime;

            // ��������� ���������� �� ������
            distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
            yield return null;
        }

    }

    private IEnumerator MoveToOriginalPosition()
    {
        float distanceToOriginalPosition = Vector3.Distance(transform.position, originalPosition);

        while (distanceToOriginalPosition > 0.1f)
        {
            // ������������ ����������� � �������� �������
            Vector3 direction = (originalPosition - transform.position).normalized;

            // ������� ����� � ���� �����������
            transform.position += direction * returnSpeed * Time.deltaTime;

            // ��������� ���������� �� �������� �������
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
                // ����� ��������� ���� ����
                armored -= finaldamagePlayer; // ��������� �����
                finaldamagePlayer = 0;
            }
            else
            {
                // ����� �� ����� ��������� ���� ����
                finaldamagePlayer -= armored; // ���� ����������� �� ���������� �����
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
        healthCanvas.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = 0;
        healthCanvas.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "0/"+maxHealth;
        animator.SetInteger("State", 9);
        this.enabled = false;
        yield return new WaitForSeconds(2f);
        player.GetComponent<Player>().turn.enemies.Clear();
        player.GetComponent<Player>().EndBattle();
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
