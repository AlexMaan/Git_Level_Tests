using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour 
{
    public void PauseGame()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            transform.FindChild("Text").GetComponent<Text>().text = "Run";
        }
        else
        {
            Time.timeScale = 1f;
            transform.FindChild("Text").GetComponent<Text>().text = "Pause";
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Unpause();
        }
        
    }
}
