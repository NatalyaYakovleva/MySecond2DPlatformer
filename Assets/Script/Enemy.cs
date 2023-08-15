using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public Transform startPos; //начальная позиция врага, откуда должен начинать всоё движение
    private bool LookRight = true; //смотри ли враг вправо
    private bool RightEnd = false;//достиг ли враг конца своего движения по правой стороне
    public float TimeToMoveRight = 1f;// сколько по времени врагу движаться вправо
    public float TimeToMoveLeft = 1f;
    private Rigidbody2D enemyrb;
    public Animator anim;
    public bool shouldMove = true; //должен ли наш вражеский персонаж в данный момент двигаться
    public bool shouldShoot = true;// должен ли наж вражеский персонаж стрелять в данный момент
    public GameObject Character; //объект нашего персонажа
    public float speed = 5f; //скорость вражеского персонажа
    public bool moveLeft = false;//должен ли вражэеский персонаж двигаться влево

    [SerializeField]
    private int sethealth;//установка опеределенного уровня здоровья

    private int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (value <= 0)
            {
                Die();
            }
        }
    }

    private void Die()//смерть конкретно этого врага
    {
        Destroy(this.gameObject, 1);//ровно через 1 секунду объект удалиться
        shouldMove = false;
        shouldShoot = false;
    }

    public void takeDamage(int damage)//для получения урона
    {
        Health -= damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyrb = GetComponent<Rigidbody2D>();
        this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -5);//изменяем позицию по оси z чтобы персонаж не ушел за сцену
        anim = GetComponent<Animator>();
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetInteger("Health", health);
        if (!RightEnd && shouldMove)
        {
            MoveRight();
        }
        else if (RightEnd && shouldMove)
        {
            MoveLeft();
        }

        if (!shouldMove)
        {
            StartCoroutine(WaitBeforeDelete());
        }
    }

    private void Flip()//разворачиваем персонажа
    {
        LookRight = !LookRight;
        transform.Rotate(0, 180, 0);
    }

    private void MoveRight()//перемещение персонажа вправо по прямой
    {
        moveLeft = false;
        enemyrb.transform.position = new Vector3(transform.position.x + 0.1f * speed, transform.position.y, transform.position.z); //изменяем позицию по оси х

    }
    private void MoveLeft()//перемещение персонажа влево по прямой
    {
        moveLeft = true;
        enemyrb.transform.position = new Vector3(transform.position.x - 0.1f * speed, transform.position.y, transform.position.z);
    }

    IEnumerator WaitBeforeDelete()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

    IEnumerator Move()
    {
        if (shouldMove)
        {
            yield return new WaitForSeconds(TimeToMoveRight);
            RightEnd = true;
            moveLeft = false;
            Flip();
            yield return new WaitForSeconds(TimeToMoveLeft);
            RightEnd = false;
            moveLeft = true;
            Flip();
            StartCoroutine(Move());
        }
    }
}
