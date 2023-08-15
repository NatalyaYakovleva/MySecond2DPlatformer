using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    private Transform[] enemies; //массив позиции врагов
    public GameObject whoIsEnemy; //кто наш враг


    // Start is called before the first frame update
    void Start()
    {
        enemies = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            enemies[i] = transform.GetChild(i);
            Instantiate(whoIsEnemy, enemies[i]);
        }
    }


}
