using System.Collections.Generic;
using SEC.Character;
using SEC.Character.Controller;
using SEC.Controller;
using SEC.Enums;
using SEC.Helpers;
using SEC.Map;
using SEC.SO;
using SEC.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngineTimers;
using PlayerInput = SEC.Character.Input.PlayerInput;

namespace SEC
{
    public class GameManager : MonoBehaviour
    {

        #region Links

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

        [field: SerializeField] public EggInput Egg { get; private set; }

        [field: SerializeField] public Transform SpawnPointRight { get; private set; }
        [field: SerializeField] public Transform SpawnPointLeft { get; private set; }
        [field: SerializeField] public PlayerInput[] Players { get; private set; }
        [field: SerializeField] public MovementSetting DefaultMovementSetting { get; private set; }

        [field: SerializeField] public InputAction OptionButton { get; private set; }
        [field: SerializeField] public OptionManager OptionMenu { get; private set; }

        [field: SerializeField] public AudioSource AudioSourceMusic { get; private set; }
        [field: SerializeField] public AudioClip[] FightMusic { get; private set; } = new AudioClip[3];

        #endregion


        #region Fields

        public bool IsMenuOpen
        {
            get => _isMenuOpen;
            set
            {
                OptionMenu.gameObject.SetActive(value);
                foreach (var player in Players)
                {
                    player.IsControlable = !value;
                }
                _isMenuOpen = value;
            }
        }

        private GameController _gameController;
        private CameraController _cameraController;
        private EggController _eggController;

        private Dictionary<PlayerInput, CharacterController2D> _playersList;

        private readonly List<IExecute> _executes = new();
        private readonly List<IExecuteLater> _executesLaters = new();
        private TimersPool _timers;

        private bool _isMenuOpen = false;

        #endregion


        #region MONO

        private void Awake()
        {
            _cameraController = new CameraController(MainCamera);
            _eggController    = new EggController(Egg);
            _playersList      = new Dictionary<PlayerInput, CharacterController2D>();
            _timers           = TimersPool.GetInstance();

            foreach (var player in Players)
            {
                // Инициализация контроллеров игроков
                var controller = new CharacterController2D(player, DefaultMovementSetting, Egg);
                player.Controller = controller;
                _playersList.Add(player, controller);
                _executesLaters.Add(controller);
            }

            _gameController   = new GameController(this,
                                         _playersList,
                                         _cameraController,
                                         MainCamera.leftCollider,
                                         MainCamera.rightCollider);

            OptionButton.started += OpenMenu;
        }

        private void Start()
        {
            OptionMenu.gameObject.SetActive(false);

            _timers.StartTimer(MusicPlay2, FightMusic[0].length - .05f);
            AudioSourceMusic.Play(FightMusic[0]);

            void MusicPlay2()
            {
                _timers.StartTimer(MusicPlay3, FightMusic[1].length - .05f);
                AudioSourceMusic.Play(FightMusic[1]);
            }

            void MusicPlay3()
            {
                _timers.StartTimer(MusicPlay2, FightMusic[2].length - .05f);
                AudioSourceMusic.Play(FightMusic[2]);
            }
        }

        private void Update() => _executes.ForEach(ex => ex.Execute());

        private void FixedUpdate() => _executesLaters.ForEach(ex => ex.ExecuteLater());

        private void OnEnable()
        {
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

        #endregion

        public void EndTriggerRightHandler(Collider2D collider) => _gameController.WhereWinner(OrientationLR.Right, collider);
        public void EndTriggerLeftHandler(Collider2D collider) => _gameController.WhereWinner(OrientationLR.Left, collider);
        private void OpenMenu(InputAction.CallbackContext _) => IsMenuOpen = !_isMenuOpen;
    }
}
