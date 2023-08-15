using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb2d; //физика персонажа
    private SpriteRenderer spriterend; //управление персонажам по оси Х
    private bool IsGrounded = false; //проверка приземлен персонаж или нет, если true - касаемся платформы, false - не касаемся
    public float runSpeed = 5f; //скорость бега персонажа
    public float jumpSpeed = 25f; //скорость прыжка персонажа

    private Animator anim; //подключаем аниматор
    private bool isMoving = false; // для анимации из idle в run = true и из run в idle = false

    private bool lookRight = true;//смотрит ли персонаж вправо
    public GameObject bullet; //создаемм объект для пули
    public Transform bulletPos; //позиция пули
    public static bool canShoot = true;//проверяем период стрельбы
    public static int health = 5; //здоровье персонажа
    public float fireCooldown = 0.5f; //задержка выстрела в 0.5 секунды


    //объявляем 3 GroundCheck, которыми обозначали ноги персонажа, для определения касания платформы
    [SerializeField]
    Transform GroundCheck;
    [SerializeField]
    Transform GroundCheck_L;
    [SerializeField]
    Transform GroundCheck_R;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); //ссылка на объект Rigidbody2D нашего персонажа
        spriterend = GetComponent<SpriteRenderer>(); //ссылка на объект SpriteRenderer нашего персонажа
        anim = GetComponent<Animator>(); //ссылка на объект аниматор для настройки переходов анимации
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)//для перезагрузки уровня
        {
            StartCoroutine(Death());
        }
        float move = Input.GetAxis("Horizontal"); //задаем переменную перемещения по горизонтали
        anim.SetBool("isMoving", isMoving); //подключаем логическую переменную из аниматора персонажа и переменную из скрипта 
        //anim.SetBool("isGrounded", IsGrounded);
        anim.SetInteger("Health", health); //подключаем анимацию death
        //проверка персонажа в пространстве
        if(Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground")) ||
           Physics2D.Linecast(transform.position, GroundCheck_L.position, 1 << LayerMask.NameToLayer("Ground")) ||
           Physics2D.Linecast(transform.position, GroundCheck_R.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            IsGrounded = true; //персонаж на земле
        }
        else
        {
            IsGrounded = false; //персонаж в пространстве
        }

        //само движение персонажа вправо или влево с условием проверки
        if(Input.GetKey(KeyCode.D) || Input.GetKey("right")) //если нажата клавиша D или стрелка вправо выполняем след.условие
        {
            //spriterend.flipX = false; //поворот персонажа вправо
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);//придаем ускорение со скоростью runSpeed, координата Y остается неизменная
            isMoving = true;
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey("left"))
        {
            //spriterend.flipX = true; //поворот персонажа влево
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);// вычитаем скорость при движении влево
            isMoving = true;
        }

        //прыжок персонажа
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKey("up")) && Mathf.Abs(rb2d.velocity.y) < 0.01f) //если нажат пробел или нажата клавиша стрелка вверж и ограничение прыжка через Mathf модуль оси у
        {
            anim.SetTrigger("jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed); // Х оставляем без изменения, У придаем ускорение через переменную jumpSpeed
        }

        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp("right") || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp("left")) //когда отпускаем кнопку D или стрелку вправо или кнопку A или стрелку влево
        {
            isMoving = false;
        }

        if (move > 0 && !lookRight)//проверка смотрит ли персонаж врпаво
        {
            Flip();
        }
        else if (move < 0 && lookRight)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.F) && canShoot == true)
        {
            Instantiate(bullet, bulletPos.position, bulletPos.rotation);
            canShoot = false;
            StartCoroutine(FireCoolDown());
        }

    }

    void Flip() //разварот персонажа при стельбе
    {
        lookRight = !lookRight;
        transform.Rotate(0, 180f, 0);
    }

    IEnumerator FireCoolDown() //задаем время стрельбы, по истечению 1 Секунды стреляем
    {
        yield return new WaitForSeconds(fireCooldown); //ждем 1 секунду
        canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D other)//в этом методе будем отнимать здоровье у персонжа если он прикоснулся к врагу
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            health--;
        }
    }

    private IEnumerator Death()//перезагрузка уровня после смерти персонажа
    {
        yield return new WaitForSeconds(1.5f);
        health = 5;
        SceneManager.LoadScene(0);
    }
}
