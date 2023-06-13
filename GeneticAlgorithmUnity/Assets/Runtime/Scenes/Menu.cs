using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private string _mainMapSceneName;

    public void GoToMainMap()
    {
        SceneManager.LoadScene(_mainMapSceneName);
    }
}
