using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(-1.5f, 1.5f);
        float y = 6;

        transform.position = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -6.0f)
        {
            Destroy(gameObject);
        }
    }
}
