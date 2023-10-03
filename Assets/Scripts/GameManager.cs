using SEC.Character;
using SEC.Controller;
using SEC.Helpers;
using SEC.Input;
using SEC.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineTimers;
using PlayerInput = SEC.Character.Input.PlayerInput;

namespace SEC
{
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Проход только после убийства")] public bool KillPassed;

        /// <summary>
        /// Проход запрещен
        /// </summary>
        [field: SerializeField] public LayerMask MaskBorderOn { get; private set; }

        /// <summary>
        /// Проход возможен
        /// </summary>
        [field: SerializeField] public LayerMask MaskBorderOff { get; private set; }

        public CameraInput MainCamera;
        public EggInput Egg;

        public Transform SpawnPointRight;
        public Transform SpawnPointLeft;
        public PlayerInput[] Players;

        public InputAction OptionButton;
        public OptionManager OptionMenu;

        public AudioSource Audio;
        public AudioClip[] fightMusic;

        private GameController _gameController;
        private CameraController _cameraController;
        private EggController _eggController;

        private bool _isMenuOpen = false;

        private void Awake()
        {
            _cameraController = new CameraController(MainCamera);
            _gameController   = new   GameController(this,
                                                     MainCamera.OnBorderExit,
                                                     _cameraController.OnBorderExitAnim,
                                                     MainCamera.leftCollider,
                                                     MainCamera.rightCollider);
            _eggController    = new    EggController(Egg);

            OptionButton.started += OpenMenu;

            //TODO: Прокидывать события подбора и кидания яйца через менеджер а не инспектор
        }

        private void Start()
        {
            TimersPool.GetInstance().StartTimer(MusicPlay2, fightMusic[0].length - .05f);
            Audio.Play(fightMusic[0]);

            void MusicPlay2()
            {
                TimersPool.GetInstance().StartTimer(MusicPlay3, fightMusic[1].length - .05f);
                Audio.Play(fightMusic[1]);
            }

            void MusicPlay3()
            {
                TimersPool.GetInstance().StartTimer(MusicPlay2, fightMusic[2].length - .05f);
                Audio.Play(fightMusic[2]);
            }
        }

        private void OnEnable()
        {
            OptionMenu.gameObject.SetActive(false);
            OptionButton.Enable();
        }

        private void OnDisable()
        {
            OptionButton.Disable();  
        }

        private void OnDestroy()
        {
            OptionButton.started -= OpenMenu;
        }

        private void OpenMenu(InputAction.CallbackContext _)
        {
            _isMenuOpen = !_isMenuOpen;
            OptionMenu.gameObject.SetActive(_isMenuOpen);
        }
    }
}
