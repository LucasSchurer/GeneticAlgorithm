using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private string _mainMapSceneName;
        [SerializeField]
        private WeaponSelection _weaponSelection;
        [SerializeField]
        private Transform _quit;
        [SerializeField]
        private Transform _play;

        public void GoToMainMap()
        {
            SceneManager.LoadScene(_mainMapSceneName);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void ShowWeaponSelection()
        {
            _quit.gameObject.SetActive(false);
            _play.gameObject.SetActive(false);

            _weaponSelection.gameObject.SetActive(true);
            _weaponSelection.Enable();
        }
    } 
}
