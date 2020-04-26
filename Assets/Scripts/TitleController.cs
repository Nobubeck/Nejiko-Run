using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Text highScoreText;
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }
    // Start is called before the first frame update
    void Start()
    {
        highScoreText.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "m";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
