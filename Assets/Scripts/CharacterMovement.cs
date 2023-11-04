using UnityEngine;
using Mirror;
public class CharacterMovement : NetworkBehaviour
{
    public float runSpeed = 4;
    public float walkSpeed = 2;
    private Rigidbody2D rb; // Ссылка на компонент Rigidbody2D
    public Animator anim; // Ссылка на компонент Rigidbody2D
    public bool isRun; // Ссылка на компонент Rigidbody2D
    
    private Vector2 movementDirection = Vector2.right; // Направление движения, изначально вправо
    private float moveSpeed = 5f; // Скорость движения персонажа
    public float verticalSpeed = 3; // Скорость движения вверх и вниз


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
    }

    void Update()
    {
        // if (!isLocalPlayer) return;

        float horizontalInput = 0;
        float verticalInput = 0;

        if (gameObject.GetComponent<Player>().canMove)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if(gameObject.GetComponent<Player>().isHub)verticalInput = Input.GetAxis("Vertical");
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            horizontalInput = 0;
            verticalInput = 0;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
            isRun = true;
        }
        else
        {
            moveSpeed = walkSpeed;
            isRun = false;
        }

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            anim.SetBool("isWalk", true);
            anim.SetBool("isRun", isRun);
        }
        else
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", isRun);
        }

        Vector2 movement = new Vector2(horizontalInput * moveSpeed, verticalInput * verticalSpeed);

        rb.velocity = movement;

        if (horizontalInput < 0 && movementDirection.x > 0)
        {
            FlipCharacter();
        }
        else if (horizontalInput > 0 && movementDirection.x < 0)
        {
            FlipCharacter();
        }
    }

    // Поворачиваем персонажа
    void FlipCharacter()
    {
        movementDirection.x *= -1; // Меняем направление движения
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;

    }
}
