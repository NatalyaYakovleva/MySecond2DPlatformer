using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletSpeed = 5f;// скорость пули


    // Start is called before the first frame update 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed; //движение пули
        StartCoroutine(WaitBeforeDelete());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitBeforeDelete() //удаление пули со сцены
    {
        yield return new WaitForSeconds(1f);//удаление пули через 1 секунду
        Destroy(this.gameObject);//удаление 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.CompareTag("Enemy"))
        //{
        //    Enemy.health--;
        //    if (Enemy.health < 1)
        //    {
        //        Enemy.shouldMove = false;
        //        Enemy.shouldShoot = false;
        //    }
        //    Destroy(this.gameObject);
        //}

        var enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.takeDamage(1);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        { }
        else
        { 
            Destroy(this.gameObject);
        }
    }
}
