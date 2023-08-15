using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrigger : MonoBehaviour
{
    public float speed = 10f; //�������� �������� ����
    public Rigidbody2D bullet; //��� ����� ��������, ���� ��� ���������� ������
    public Transform gunPoint; //������� � ������� ����� �������� ����
    public float fireRate = 1; //������� �������� � ��������
    public Transform player; //�������� �� ���� ��������� �����, ��� ������� ��������
    private Rigidbody2D clone; //���� ����������� ���� ������
    float elapsedTime = 0.0f; //������, ����� ������������ �������� ����
    public GameObject Enemy;


    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)//��������� ������ ��������� � ������� ���� ������� ���������� �����
    {
        if (other.gameObject.CompareTag("Player")) //���� ��������� � ������� ���������� � �������� ������� �������� ��� Player
        {
            fireRate = Random.Range(0.5f, 3f); //�������� �������� ����� �������� ��������� �������� � ��������
            elapsedTime += Time.deltaTime;

            if (elapsedTime > fireRate && Enemy.gameObject.GetComponent<Enemy>().shouldShoot) //���� ������ ������ ����� �� ������� �� ����� ���������� � ��� ���� ������ �������� � ���� ������
            {
                Enemy.gameObject.GetComponent<Enemy>().anim.SetBool("Attack", true);
                StartCoroutine(ShootingOnce());
                elapsedTime = 0.0f; //�������� ����������
                clone = Instantiate(bullet, gunPoint.position, gunPoint.rotation) as Rigidbody2D; //��������� ����� � ��������� �������� ������� ���� �� ������ ������ � ������� ��� ������ ����
                //if (Enemy.moveLeft)//���� ���� ������������ �����
                //{
                //    Flip();
                //}
                //clone.velocity = transform.right * speed;//�������� �������� �����

            }
        }
    }

    IEnumerator ShootingOnce()
    {
        yield return new WaitForSeconds(0.3f);
        Enemy.gameObject.GetComponent<Enemy>().anim.SetBool("Attack", false);
    }

    //void Flip() //�������� ����
    //{
    //    clone.transform.Rotate(0, 180, 0);
    //}
}
