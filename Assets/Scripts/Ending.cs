using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public float speed = 10f;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rectTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);

        if (rectTransform.anchoredPosition.y >= 1950f)
        {

           MasterControl._Instance.SceneChange("StartScene", MasterControl.GameState.start);
        }
    }
}
