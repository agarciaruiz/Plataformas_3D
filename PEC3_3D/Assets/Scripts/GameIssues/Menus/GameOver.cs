using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private Button mainMenuButton;
    private Button quitButt;

    private List<Button> startMenuButtons = new List<Button>();

    void Start()
    {
        mainMenuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
        quitButt = GameObject.Find("QuitButton").GetComponent<Button>();

        // Añadimos botones a la lista para eliminar los listeners
        startMenuButtons.Add(mainMenuButton);
        startMenuButtons.Add(quitButt);

        // Desactivamos el panel principal y activamos el panel de selección de circuito
        mainMenuButton.onClick.AddListener(() => ListenerMethods.ChangeScene(Scenes.mainMenu));
        quitButt.onClick.AddListener(() => ListenerMethods.QuitApp());
    }

    private void OnDestroy()
    {
        ListenerMethods.RemoveListeners(startMenuButtons);
    }
}
