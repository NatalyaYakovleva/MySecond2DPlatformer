using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rb2d; //������ ���������
    private SpriteRenderer spriterend; //���������� ���������� �� ��� �
    private bool IsGrounded = false; //�������� ��������� �������� ��� ���, ���� true - �������� ���������, false - �� ��������
    public float runSpeed = 5f; //�������� ���� ���������
    public float jumpSpeed = 25f; //�������� ������ ���������

    private Animator anim; //���������� ��������
    private bool isMoving = false; // ��� �������� �� idle � run = true � �� run � idle = false

    private bool lookRight = true;//������� �� �������� ������
    public GameObject bullet; //�������� ������ ��� ����
    public Transform bulletPos; //������� ����
    public static bool canShoot = true;//��������� ������ ��������
    public static int health = 5; //�������� ���������
    public float fireCooldown = 0.5f; //�������� �������� � 0.5 �������


    //��������� 3 GroundCheck, �������� ���������� ���� ���������, ��� ����������� ������� ���������
    [SerializeField]
    Transform GroundCheck;
    [SerializeField]
    Transform GroundCheck_L;
    [SerializeField]
    Transform GroundCheck_R;
    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); //������ �� ������ Rigidbody2D ������ ���������
        spriterend = GetComponent<SpriteRenderer>(); //������ �� ������ SpriteRenderer ������ ���������
        anim = GetComponent<Animator>(); //������ �� ������ �������� ��� ��������� ��������� ��������
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)//��� ������������ ������
        {
            StartCoroutine(Death());
        }
        float move = Input.GetAxis("Horizontal"); //������ ���������� ����������� �� �����������
        anim.SetBool("isMoving", isMoving); //���������� ���������� ���������� �� ��������� ��������� � ���������� �� ������� 
        //anim.SetBool("isGrounded", IsGrounded);
        anim.SetInteger("Health", health); //���������� �������� death
        //�������� ��������� � ������������
        if(Physics2D.Linecast(transform.position, GroundCheck.position, 1 << LayerMask.NameToLayer("Ground")) ||
           Physics2D.Linecast(transform.position, GroundCheck_L.position, 1 << LayerMask.NameToLayer("Ground")) ||
           Physics2D.Linecast(transform.position, GroundCheck_R.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            IsGrounded = true; //�������� �� �����
        }
        else
        {
            IsGrounded = false; //�������� � ������������
        }

        //���� �������� ��������� ������ ��� ����� � �������� ��������
        if(Input.GetKey(KeyCode.D) || Input.GetKey("right")) //���� ������ ������� D ��� ������� ������ ��������� ����.�������
        {
            //spriterend.flipX = false; //������� ��������� ������
            rb2d.velocity = new Vector2(runSpeed, rb2d.velocity.y);//������� ��������� �� ��������� runSpeed, ���������� Y �������� ����������
            isMoving = true;
        }
        else if(Input.GetKey(KeyCode.A) || Input.GetKey("left"))
        {
            //spriterend.flipX = true; //������� ��������� �����
            rb2d.velocity = new Vector2(-runSpeed, rb2d.velocity.y);// �������� �������� ��� �������� �����
            isMoving = true;
        }

        //������ ���������
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetKey("up")) && Mathf.Abs(rb2d.velocity.y) < 0.01f) //���� ����� ������ ��� ������ ������� ������� ����� � ����������� ������ ����� Mathf ������ ��� �
        {
            anim.SetTrigger("jump");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed); // � ��������� ��� ���������, � ������� ��������� ����� ���������� jumpSpeed
        }

        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp("right") || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp("left")) //����� ��������� ������ D ��� ������� ������ ��� ������ A ��� ������� �����
        {
            isMoving = false;
        }

        if (move > 0 && !lookRight)//�������� ������� �� �������� ������
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

    void Flip() //�������� ��������� ��� �������
    {
        lookRight = !lookRight;
        transform.Rotate(0, 180f, 0);
    }

    IEnumerator FireCoolDown() //������ ����� ��������, �� ��������� 1 ������� ��������
    {
        yield return new WaitForSeconds(fireCooldown); //���� 1 �������
        canShoot = true;
    }

    private void OnTriggerEnter2D(Collider2D other)//� ���� ������ ����� �������� �������� � �������� ���� �� ����������� � �����
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            health--;
        }
    }

    private IEnumerator Death()//������������ ������ ����� ������ ���������
    {
        yield return new WaitForSeconds(1.5f);
        health = 5;
        SceneManager.LoadScene(0);
    }
}
