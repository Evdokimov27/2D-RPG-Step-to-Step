using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using YG;

// Token: 0x02000020 RID: 32
public class Player : MonoBehaviour
{

	public int rating;
	public CameraFollow camFollow;
	public bool cameraSpawn = false;
	public GameObject cameraPrefab;
	public GameObject[] canvasUIs;
	public GameObject[] upgradeButton;

	public SpriteRenderer eyeDeath;
	public SpriteRenderer mounthDeath;
	public Sprite eyeDeathPrefab;
	public Sprite mounthDeathPrefab;
	public Animator anim;
	public EquipmentManager equipmentManager;
	public bool canMove = true;
	public bool isHub = false;
	public ActionPointsUI ap;
	public GameObject apUI;
	public EnemyScript nearestEnemy;
	public float attackRange = 2f;
	public float returnSpeed = 2f;
	public Vector3 originalPosition;
	public GameObject[] buttonList;
	public Text armorText;
	public Text levelText;
	public Text healthText;
	public Image healthSlider;
	public Image expSlider;
	public Text expText;
	[Header("Статы")]
	public int strength = 10;
	public int agility = 10;
	public int intelligence = 10;
	public int level = 1;
	public int exp = 0;
	public int expNeedToNextLevel = 0;
	public int baseHealth = 5;
	public int armor;
	public int damage;
	public float moveSpeed = 4f;
	public float maxHealth;
	[Header("Параметры")]
	public int armored;
	public float currentHealth;
	public TurnBasedSystem turn;
	public int maxActionPoints = 10;
	public int freePointStats = 0;
	public int actionPoints;
	public int attackAction;
	public int defenceAction;
	public int useAction;
	public bool inBattle;
	public bool finish;
	private bool attack;
	private GameObject camera;
	private Queue<ActionWithPriority> actionQueue = new Queue<ActionWithPriority>();
	private List<ActionWithPriority> sortedActions = new List<ActionWithPriority>();

	public void Awake()
	{
		if (YandexGame.savesData.strength > 0 && YandexGame.savesData.agility > 0 && YandexGame.savesData.intelligence > 0)
		{
			level = YandexGame.savesData.level;
			exp = YandexGame.savesData.exp;
			freePointStats = YandexGame.savesData.freePointStats;
			strength = YandexGame.savesData.strength;
			agility = YandexGame.savesData.agility;
			intelligence = YandexGame.savesData.intelligence;
		}

		expNeedToNextLevel = level * 100;
		foreach (GameObject obj in canvasUIs)
		{
			obj.SetActive(false);
		}
		if (cameraSpawn) camera = Instantiate(cameraPrefab, transform.position.normalized, Quaternion.identity);
		else camera = GameObject.FindGameObjectWithTag("MainCamera");
		camera.SetActive(true);
		camFollow = camera.GetComponent<CameraFollow>();
		maxHealth = (float)(baseHealth * strength);
		currentHealth = maxHealth;
		turn = GetComponent<TurnBasedSystem>();
		StartCoroutine(ProcessActionQueue());
		turn.playerTurn = true;
		actionPoints = maxActionPoints;
	}

	public class ActionWithPriority
	{
		public int Priority { get; private set; }

		public IEnumerator Actions { get; private set; }
		public ActionWithPriority(int priority, IEnumerator actions)
		{
			Priority = priority;
			Actions = actions;
		}
	}

	private IEnumerator ProcessActionQueue()
	{
		//if (!isLocalPlayer) yield break;
		{
			if (finish && actionQueue.Count > 0)
			{

				if (nearestEnemy != null)
				{
					attack = true;
					sortedActions = actionQueue.OrderBy(a => a.Priority).ToList();
					HasAttackAction(sortedActions);
					actionQueue.Clear();
					foreach (ActionWithPriority actionWithPriority in sortedActions)
					{
						yield return StartCoroutine(actionWithPriority.Actions);
						if (nearestEnemy.currentHealth < 1) break;
					}
					sortedActions.Clear();
					yield return new WaitForSeconds(0.5f);
					attack = false;
					yield return new WaitForSeconds(2f);
					turn.EndCurrentTurn();
				}
				else
				{
					yield return null;
				}
			}
			else
			{
				finish = false;
				yield return null;

			}
		}
	}
	private IEnumerator ExecuteAttack(EnemyScript enemy)
	{
		//if (!isLocalPlayer) yield break;

		{
			if (inBattle && turn.playerTurn && actionPoints >= attackAction)
			{
				actionPoints -= attackAction;
				ap.UpdateActionPoints(actionPoints);
				actionQueue.Enqueue(new ActionWithPriority(5, Attack(enemy)));
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}
	}
	public void UpStats(string stat)
	{
		if (stat == "сила") strength++;
		if (stat == "ловкость") agility++;
		if (stat == "интелект") intelligence++;
		freePointStats--;
		equipmentManager.GetTotalStats();
		equipmentManager.CountStats();
	}
	private IEnumerator ExecuteDefence()
	{
		//if (!isLocalPlayer) yield break;

		{
			if (inBattle && turn.playerTurn && actionPoints >= defenceAction)
			{
				actionPoints -= defenceAction;
				ap.UpdateActionPoints(actionPoints);
				actionQueue.Enqueue(new ActionWithPriority(2, Defence()));
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}
	}
	private IEnumerator ExecuteUse()
	{
		//if (!isLocalPlayer) yield break;

		{
			if (inBattle && turn.playerTurn && actionPoints >= useAction)
			{
				actionPoints -= useAction;
				ap.UpdateActionPoints(actionPoints);
				actionQueue.Enqueue(new ActionWithPriority(1, Use()));
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}
	}
	private IEnumerator Attack(EnemyScript enemy)
	{
		//if (!isLocalPlayer) yield break;

		{
			if ((double)Vector3.Distance(this.gameObject.transform.position, nearestEnemy.transform.position) - 0.1 > (double)attackRange)
			{
				yield return new WaitForSeconds(2f);
			}
			foreach (Items items in equipmentManager.items)
			{
				if (items.itemType == ItemTypes.Weapon)
				{
					if (items.weaponType == WeaponTypes.Дробящий || items.weaponType == WeaponTypes.Разрубающий)
					{
						anim.SetTrigger("HardAttack");
					}
					if (items.weaponType == WeaponTypes.Режущий || items.weaponType == WeaponTypes.Колющий)
					{
						anim.SetTrigger("SwordAttack");
					}
				}
			}
			yield return new WaitForSeconds(1f);
			if (enemy != null)
			{
				enemy.TakeDamage(equipmentManager.currentDamage);
			}
			yield break;
		}
	}
	private IEnumerator Defence()
	{
		//if (!isLocalPlayer) yield break;
		{
			armored += equipmentManager.currentArmor;
			yield break;
		}
	}
	private IEnumerator Use()
	{
		if (equipmentManager.equipmentSlots[ItemTypes.Health].childCount > 0)
		{
			anim.SetTrigger("Use");
			yield return new WaitForSeconds(1f);
			if (currentHealth < maxHealth)
			{
				currentHealth += equipmentManager.currentHealth;
				GameObject.FindGameObjectWithTag("upgradeItem").GetComponent<ItemUpgrade>().UseDurability(equipmentManager.equipmentSlots[ItemTypes.Health].GetChild(0).GetComponent<Items>(), 1);
				if (currentHealth > maxHealth) currentHealth = maxHealth;
			}
		}
		yield break;
	}
	public void TakeDamage(int damageEnemy)
	{
		//if (!isLocalPlayer) return;
		{
			if (this.gameObject.gameObject != null)
			{
				int num;
				if (armored >= damageEnemy)
				{
					armored -= damageEnemy;
					num = 0;
				}
				else
				{
					num = damageEnemy - armored;
					armored = 0;
				}
				Debug.Log(armored.ToString() + " " + num.ToString());
				currentHealth -= (float)num;
				if (currentHealth <= 0f)
				{
					Die();
				}
			}
		}
	}

	private void Die()
	{
		//if (!isLocalPlayer) return;
		{
			anim.SetTrigger("Death");
			eyeDeath.sprite = eyeDeathPrefab;
			mounthDeath.sprite = mounthDeathPrefab;
			this.GetComponent<CharacterMovement>().enabled = false;
			this.enabled = false;
		}
	}

	public void StartBattle()
	{
		//if (!isLocalPlayer) return;
		{
			anim.SetBool("isRun", false);

			gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
			gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

			gameObject.GetComponent<CharacterMovement>().joystick.Horizontal = 0;
			gameObject.GetComponent<CharacterMovement>().joystick.transform.GetChild(0).localPosition = new Vector3(0, 0);
			gameObject.GetComponent<CharacterMovement>().joystick.gameObject.SetActive(false);
			foreach (GameObject obj in canvasUIs)
			{
				obj.SetActive(true);
			}
			if (camFollow != null) camFollow.StartFollowing();
			actionPoints = maxActionPoints;
			ap.UpdateActionPoints(actionPoints);
			originalPosition = this.gameObject.transform.position;
			canMove = false;
			inBattle = true;
			apUI.SetActive(true);
			GameObject[] array = buttonList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(true);
			}
		}
	}

	public void CheatConsole(int command)
	{
		if (command == 0) exp += 100;
		if (command == 1) GetComponent<Inventory>().money += 100;
		if (command == 2) currentHealth = maxHealth;
	}
	public void EndBattle()
	{
		//if (!isLocalPlayer) return;
		{
			foreach (GameObject obj in canvasUIs)
			{
				obj.SetActive(false);
			}
			if (camFollow != null) camFollow.StopFollowing();
			attack = false;
			actionQueue.Clear();
			canMove = true;
			inBattle = false;
			apUI.SetActive(false);
			GameObject[] array = buttonList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);
			}
			exp += nearestEnemy.expForWin;
		}
	}
	private void MoveToTarget()
	{
		//if (!isLocalPlayer) return;
		{
			Vector3 normalized = (nearestEnemy.gameObject.transform.position - this.gameObject.transform.position).normalized;
			gameObject.transform.Translate(normalized * moveSpeed * Time.deltaTime);
		}
	}
	private void MoveToOriginalPosition()
	{
		//if (!isLocalPlayer) return;
		{
			Vector3 vector = originalPosition - this.gameObject.transform.position;
			if (vector.magnitude > 0.1f)
			{
				Vector3 normalized = vector.normalized;
				this.gameObject.transform.Translate(normalized * moveSpeed * Time.deltaTime);
			}
		}
	}
	public void EndTurn()
	{
		//if (!isLocalPlayer) return;
		{
			if (inBattle)
			{
				if (actionQueue.Count == 0) turn.EndCurrentTurn();

				actionPoints = 0;
				ap.UpdateActionPoints(actionPoints);

			}
		}
	}

	public void _Attack()
	{
		//if (!isLocalPlayer) return;
		{
			StartCoroutine(ExecuteAttack(nearestEnemy));
		}
	}

	public void _Defence()
	{
		//if (!isLocalPlayer) return;
		{
			StartCoroutine(ExecuteDefence());
		}
	}


	public void _Use()
	{
		{
			StartCoroutine(ExecuteUse());
		}
	}

	private void Update()
	{
		maxHealth = baseHealth * strength;
		if (!inBattle) turn.playerTurn = true;

		if (freePointStats > 0)
		{
			foreach (GameObject button in upgradeButton)
			{
				button.SetActive(true);
			}
		}
		else
		{
			foreach (GameObject button in upgradeButton)
			{
				button.SetActive(false);
			}
		}
		float expAmount = (float)exp / expNeedToNextLevel;
		expSlider.fillAmount = expAmount;
		expText.text = exp + "/" + expNeedToNextLevel;

		if (exp >= expNeedToNextLevel)
		{
			level += 1;
			expNeedToNextLevel = (int)(level*100*1.5f);
			freePointStats += 5;
		}
		if (SceneManager.GetActiveScene().buildIndex == 0) isHub = true;
		else isHub = false;
		if (SceneManager.GetActiveScene().buildIndex == 1) canMove = false;
		else if (!inBattle) canMove = true;

		if (armored > 0)
		{
			armorText.text = armored.ToString();
		}
		else
		{
			armorText.text = "0";
		}
		float fillAmount = currentHealth / maxHealth;
		healthSlider.fillAmount = fillAmount;
		healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();
		levelText.text = level.ToString();
		MoveFight();
		if (actionPoints == 0 && turn.playerTurn)
		{
			finish = true;
			StartCoroutine(ProcessActionQueue());
		}
	}
	private bool HasAttackAction(List<ActionWithPriority> queue)
	{
		foreach (var action in queue)
		{
			// Проверяем, является ли действие атакой
			// Это условие зависит от вашей реализации
			if (action.Priority == 5) // или какой-либо другой способ идентификации
			{
				return true;
			}
		}
		return false;
	}
	private void MoveFight()
	{
		if (inBattle && attack && HasAttackAction(sortedActions) && nearestEnemy != null)
		{
			if (Vector3.Distance(this.gameObject.transform.position, nearestEnemy.transform.position) >= attackRange)
			{
				anim.SetBool("isWalk", true);
				anim.SetBool("isRun", true);
				MoveToTarget();
			}
			else
			{
				anim.SetBool("isWalk", false);
				anim.SetBool("isRun", false);
			}
		}
		if (inBattle && !attack && nearestEnemy != null)
		{
			if ((double)Vector3.Distance(originalPosition, this.gameObject.transform.position) >= 0.1)
			{
				anim.SetBool("isWalk", true);
				MoveToOriginalPosition();
				return;
			}
			anim.SetBool("isWalk", false);
		}
	}
}

