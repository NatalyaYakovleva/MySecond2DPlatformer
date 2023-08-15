using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundMove : MonoBehaviour
{
    public Transform cameraTransform; //переменная для отслеживания движения камеры
    private Transform[] layers; //массив для заднего фона, для замещения картинок

    public float vievZona = 5f; //видимая зона камеры, размер видимой зоны который указан в настройках камеры Orthographic Size = 5
    private int leftIndex; //обращение к левому индексу  
    private int rightIndex; //обращение к правому индексу
    public float backgroundSize = 19f; //размер изображения заднего фона

    public float parralaxSpeed = 0.3f; //скорость движения заднего фона
    private float lastCameraX; //последняя позиция камеры


    // Start is called before the first frame update
    void Start()
    {
        lastCameraX = cameraTransform.position.x; //обращаемся при старте игры к позиции нашей камеры
        layers = new Transform[transform.childCount]; //сколько у объекта Background дочерних объектов
        for (int i = 0; i < transform.childCount; i++) //считаем кол-во элементов
        {
            layers[i] = transform.GetChild(i); // присваиваем массиву layers кол-во дочерних элементов объекта Background 
            leftIndex = 0; //начальная позиция элементов
            rightIndex = layers.Length - 1; //конечная позиция элементов 
        }
    }

    //движение фона
    void ScrollRight() //движение в право
    {
        float lastLeft = leftIndex;
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
    }

    void ScrollLeft()
    {
        float lastIndex = rightIndex;
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0) rightIndex = layers.Length - 1;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, cameraTransform.position.y);
        layers[leftIndex].transform.position = new Vector2(layers[leftIndex].transform.position.x, cameraTransform.position.y);
        layers[rightIndex].transform.position = new Vector2(layers[rightIndex].transform.position.x, cameraTransform.position.y);

        float deltaX = cameraTransform.position.x - lastCameraX;
        lastCameraX = cameraTransform.position.x;

        transform.position += Vector3.right * (deltaX * parralaxSpeed);

        if (cameraTransform.position.x < layers[leftIndex].transform.position.x + vievZona)
        {
            ScrollLeft();
        }
        if (cameraTransform.position.x > layers[rightIndex].transform.position.x - vievZona)
        {
            ScrollRight();
        }
    }
}
