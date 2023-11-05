using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.SceneManagement;

// Token: 0x02000020 RID: 32
public class Player : NetworkBehaviour
{
	
	public int rating;
	public NetworkConnectionToClient connection;
	public CameraFollow camFollow;
	public bool cameraSpawn = false;
	public GameObject cameraPrefab;
	public GameObject[] canvasUIs;

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
	[Header("Статы")]
	public int strength = 10;
	public int agility = 10;
	public int intelligence = 10;
	public int level = 1;
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
	public int actionPoints;
	public bool inBattle;
	public bool finish;
	private bool attack;
	private GameObject camera;
	private Queue<ActionWithPriority> actionQueue = new Queue<ActionWithPriority>();

	public void Awake()
	{
		if (cameraSpawn) camera = Instantiate(cameraPrefab, transform.position.normalized, Quaternion.identity);
		else camera = GameObject.FindGameObjectWithTag("MainCamera");
		camera.SetActive(true);
		camFollow = camera.GetComponent<CameraFollow>();

		foreach (var canvasUI in canvasUIs)
		{
			canvasUI.gameObject.SetActive(true);
		}
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
					var sortedActions = actionQueue.OrderBy(a => a.Priority).ToList();
					actionQueue.Clear();
					foreach (ActionWithPriority actionWithPriority in sortedActions)
					{
						yield return StartCoroutine(actionWithPriority.Actions);
					}
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
			if (inBattle && turn.playerTurn && actionPoints >= 2)
			{
				actionPoints -= 2;
				ap.UpdateActionPoints(actionPoints);
				actionQueue.Enqueue(new ActionWithPriority(10, Attack(enemy)));
				yield return new WaitForSeconds(1f);
			}
			yield break;
		}
	}

	private IEnumerator ExecuteDefence()
	{
		//if (!isLocalPlayer) yield break;

		{
			if (inBattle && turn.playerTurn && actionPoints >= 1)
			{
				actionPoints--;
				ap.UpdateActionPoints(actionPoints);
				actionQueue.Enqueue(new ActionWithPriority(1, Defence()));
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
			yield return new WaitForSeconds(1f);
			yield break;
		}
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
	public void EndBattle()
	{
		//if (!isLocalPlayer) return;
		{
			if (camFollow != null) camFollow.StopFollowing();
			attack = false;
			actionQueue.Clear();
			turn.playerTurn = true;
			canMove = true;
			inBattle = false;
			apUI.SetActive(false);
			GameObject[] array = buttonList;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(false);

			}
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
	}

	private void Update()
	{
		//if (!isLocalPlayer) return;
		{
			if (SceneManager.GetActiveScene().buildIndex == 1) isHub = true;
			else isHub = false;
			if (SceneManager.GetActiveScene().buildIndex == 0) canMove = false;
			else canMove = true;

			maxHealth = (float)(baseHealth * strength);
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
	}
	private void MoveFight()
	{
		if (inBattle && attack && nearestEnemy != null)
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

