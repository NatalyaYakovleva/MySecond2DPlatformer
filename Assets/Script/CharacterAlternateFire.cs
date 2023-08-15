using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAlternateFire : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        float dx = mouse.x - this.transform.position.x;
        float dy = mouse.y - this.transform.position.y;
        float a = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, a);

        if (Input.GetKeyDown(KeyCode.Mouse0) && CharacterController2D.canShoot)
        {
            CharacterController2D.canShoot = false;
            Instantiate(bullet, this.transform.position, Quaternion.Euler(0, 0, a));
            StartCoroutine(FireCooldown());
        }
    }

    IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        CharacterController2D.canShoot = true;
    }
}
