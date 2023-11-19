using UnityEngine;
public class CharacterMovement : MonoBehaviour
{
    public FixedJoystick joystick;
    public float runSpeed = 4;
    public float walkSpeed = 2;
    private Rigidbody2D rb; // Ссылка на компонент Rigidbody2D
    public Animator anim; // Ссылка на компонент Rigidbody2D
    public bool isRun; // Ссылка на компонент Rigidbody2D
    public bool phoneContoller = false; // Ссылка на компонент Rigidbody2D
    
    private Vector2 movementDirection = Vector2.right; // Направление движения, изначально вправо
    private float moveSpeed = 5f; // Скорость движения персонажа
    public float verticalSpeed = 3; // Скорость движения вверх и вниз


    public void Start()
    {
       joystick.gameObject.SetActive(false);

        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D
    }
    public void ChangeControll()
    {
        phoneContoller = !phoneContoller;
    }
    void Update()
    {
        if (!gameObject.GetComponent<Player>().inBattle)
        {
            float horizontalInput = 0;
            if (!phoneContoller)
            {
                joystick.gameObject.SetActive(false);
                // if (!isLocalPlayer) return;

                horizontalInput = Input.GetAxis("Horizontal");
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
            }
            if (phoneContoller)
            {
                if (gameObject.GetComponent<Player>().canMove) joystick.gameObject.SetActive(true);
                if (!gameObject.GetComponent<Player>().canMove) joystick.gameObject.SetActive(false);

                horizontalInput = joystick.Horizontal;
                if (Mathf.Abs(horizontalInput) > 0.75f && Mathf.Abs(horizontalInput) != 0)
                {
                    moveSpeed = runSpeed;
                    isRun = true;
                }
                else
                {
                    moveSpeed = walkSpeed;
                    isRun = false;
                }
            }
            if (gameObject.GetComponent<Player>().canMove)
            {
                horizontalInput = horizontalInput;
                joystick.AxisOptions = AxisOptions.Horizontal;
            }
            else
            {
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", false);
                horizontalInput = 0;
            }


            if (Mathf.Abs(horizontalInput) > 0.1f)
            {
                anim.SetBool("isWalk", true);
                anim.SetBool("isRun", isRun);
            }
            else
            {
                anim.SetBool("isWalk", false);
                anim.SetBool("isRun", isRun);
            }

            Vector2 movement = new Vector2(horizontalInput * moveSpeed, 0);

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
