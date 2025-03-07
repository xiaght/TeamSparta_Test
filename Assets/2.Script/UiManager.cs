using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI timeScale;
    public GameObject dieUi;

    public void TimeSpeedToogle() {
        if (Time.timeScale == 1f)
        {

            Time.timeScale = 2;
            timeScale.text = "x2";
        }
        else if (Time.timeScale == 2f) {
            Time.timeScale = 3;

            timeScale.text = "x3";
        }
        else
        {
            Time.timeScale = 1f;

            timeScale.text = "x1";
        }

    }
    public void OnDieUi() {
        dieUi.SetActive(true);
        Time.timeScale = 0;
    }

    public void ExitGame() {
        UnityEditor.EditorApplication.isPlaying = false;

    }

    public void ReStart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
