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

            Audio.clip = clip1;
            TimersPool.GetInstance().StartTimer(MusicEnd, clip1.length);

            void MusicEnd()
            {
                Audio.loop = true;
                Audio.clip = clip2;
            }
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}