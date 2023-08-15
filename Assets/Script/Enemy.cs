using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 3;
    public Transform startPos; //��������� ������� �����, ������ ������ �������� ��� ��������
    private bool LookRight = true; //������ �� ���� ������
    private bool RightEnd = false;//������ �� ���� ����� ������ �������� �� ������ �������
    public float TimeToMoveRight = 1f;// ������� �� ������� ����� ��������� ������
    public float TimeToMoveLeft = 1f;
    private Rigidbody2D enemyrb;
    public Animator anim;
    public bool shouldMove = true; //������ �� ��� ��������� �������� � ������ ������ ���������
    public bool shouldShoot = true;// ������ �� ��� ��������� �������� �������� � ������ ������
    public GameObject Character; //������ ������ ���������
    public float speed = 5f; //�������� ���������� ���������
    public bool moveLeft = false;//������ �� ���������� �������� ��������� �����

    [SerializeField]
    private int sethealth;//��������� �������������� ������ ��������

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

    private void Die()//������ ��������� ����� �����
    {
        Destroy(this.gameObject, 1);//����� ����� 1 ������� ������ ���������
        shouldMove = false;
        shouldShoot = false;
    }

    public void takeDamage(int damage)//��� ��������� �����
    {
        Health -= damage;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyrb = GetComponent<Rigidbody2D>();
        this.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -5);//�������� ������� �� ��� z ����� �������� �� ���� �� �����
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

    private void Flip()//������������� ���������
    {
        LookRight = !LookRight;
        transform.Rotate(0, 180, 0);
    }

    private void MoveRight()//����������� ��������� ������ �� ������
    {
        moveLeft = false;
        enemyrb.transform.position = new Vector3(transform.position.x + 0.1f * speed, transform.position.y, transform.position.z); //�������� ������� �� ��� �

    }
    private void MoveLeft()//����������� ��������� ����� �� ������
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
