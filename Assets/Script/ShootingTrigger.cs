using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrigger : MonoBehaviour
{
    public float speed = 10f; //скорость движения пули
    public Rigidbody2D bullet; //чем будем стрелять, пуля как физический объект
    public Transform gunPoint; //позиция с которой будет стрелять враг
    public float fireRate = 1; //частота стрельбы в секундах
    public Transform player; //персонаж по кому открываем огонь, наш игровой персонаж
    private Rigidbody2D clone; //клон создаваемой пули врагом
    float elapsedTime = 0.0f; //Таймер, время определяющее скорость пуль
    public GameObject Enemy;


    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)//проверяем нашего персонажа в радиусе зоны дейсвия коллайдера врага
    {
        if (other.gameObject.CompareTag("Player")) //если коллайдер с которым столкнулся с объектом который содержит тег Player
        {
            fireRate = Random.Range(0.5f, 3f); //скорости стрельбы будем задавать случайное значение в секундах
            elapsedTime += Time.deltaTime;

            if (elapsedTime > fireRate && Enemy.gameObject.GetComponent<Enemy>().shouldShoot) //если таймер показа время за которое он может выстрелить и наш враг момжет стрелять в этот момент
            {
                Enemy.gameObject.GetComponent<Enemy>().anim.SetBool("Attack", true);
                StartCoroutine(ShootingOnce());
                elapsedTime = 0.0f; //Обнулить переменную
                clone = Instantiate(bullet, gunPoint.position, gunPoint.rotation) as Rigidbody2D; //запустить клона и оставляем вращение которое есть на данный момент и спавним как твёрдое тело
                //if (Enemy.moveLeft)//если враг перемещается влево
                //{
                //    Flip();
                //}
                //clone.velocity = transform.right * speed;//скорость движения клона

            }
        }
    }

    IEnumerator ShootingOnce()
    {
        yield return new WaitForSeconds(0.3f);
        Enemy.gameObject.GetComponent<Enemy>().anim.SetBool("Attack", false);
    }

    //void Flip() //разворот пули
    //{
    //    clone.transform.Rotate(0, 180, 0);
    //}
}
