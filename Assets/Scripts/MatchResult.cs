using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchResult : MonoBehaviour
{
    public GameManager manager;
    public Image image;

    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f);
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);

    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (true)

        image.color = FailColor;
    }
}
