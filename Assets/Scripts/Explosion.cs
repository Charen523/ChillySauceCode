using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(-1.5f, 1.5f);
        float y = Random.Range(-1f, 1f);

        transform.position = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("DestroyExplosion", 2f);
    }

    void DestroyExplosion()
    {
        Destroy(gameObject);
    }
}
