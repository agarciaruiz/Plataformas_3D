using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ListenerMethods
{
    // Cambia de escena
    public static void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    // Cierra la app
    public static void QuitApp()
    {
        Debug.Log("APPLICATION CLOSED!");
        Application.Quit();
    }

    // Elimina los listeners
    public static void RemoveListeners(List<Button> buttons)
    {
        if (buttons != null)
        {
            foreach (Button button in buttons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }
}