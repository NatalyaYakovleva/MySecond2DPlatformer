using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float speed = 10f;

    void Start()
    {
        this.gameObject.GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)//касается ли какого-нибудь объекта у которого есть коллайдер
    {
        if (other.gameObject.CompareTag("Player"))//если касается нашего персонажа
        {
            CharacterController2D.health--; //то отнимаем здоровье персонажа
        }
        if (other.gameObject.CompareTag("Enemy"))// чтобы пуля не реагировала на самого врага
        { }
        else
        { 
            Destroy(this.gameObject);//уничтожаем объект
        }
    }
}
