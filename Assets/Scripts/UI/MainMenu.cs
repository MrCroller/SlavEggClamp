using SEC.Associations;
using SEC.Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineTimers;

namespace SEC.UI
{
    public class MainMenu : MonoBehaviour
    {

        public GameObject mainMenu;
        public GameObject optionMenu;

        public AudioSource Audio;
        public AudioClip clip1;
        public AudioClip clip2;

        public bool OptionOpen
        {
            set
            {
                mainMenu.SetActive(!value);
                optionMenu.SetActive(value);
            }
        }

        private void Start()
        {
            mainMenu.SetActive(true);
            optionMenu.SetActive(false);

            TimersPool.GetInstance().StartTimer(MusicEnd, clip1.length - .05f);
            Audio.Play(clip1);

            void MusicEnd()
            {
                Audio.Play(clip2);
                Audio.loop = true;
            }
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(SceneAssociations.Game);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}