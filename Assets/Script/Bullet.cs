using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletSpeed = 5f;// �������� ����


    // Start is called before the first frame update 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * bulletSpeed; //�������� ����
        StartCoroutine(WaitBeforeDelete());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitBeforeDelete() //�������� ���� �� �����
    {
        yield return new WaitForSeconds(1f);//�������� ���� ����� 1 �������
        Destroy(this.gameObject);//�������� 
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
