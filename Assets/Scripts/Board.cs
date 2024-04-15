using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        int[] cardArray = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7};
        cardArray = ShuffleArray(cardArray);
        Debug.Log($"{string.Join(", ", cardArray)}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int[] ShuffleArray(int[] array)
    {
        int random1, random2, temp;
        for(int i = 0; i < array.Length; i++)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }
        return array;
    }
}
