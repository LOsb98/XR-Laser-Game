using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _helpPanel;

    public void SetHelpPanelActive(bool newState)
    {
        _helpPanel.SetActive(newState);
    }

    public void LoadGameScene()
    {
        //Would need a more robust system for scene loading if more scenes were to be added
        //For now can assume 1 is the game scene
        SceneManager.LoadScene(1);
    }
}
