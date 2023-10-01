using SEC.Character;
using SEC.Character.Controller;
using SEC.Character.Input;
using SEC.Controller;
using SEC.Input;
using UnityEngine;


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

        private GameController _gameController;
        private CameraController _cameraController;
        private EggController _eggController;

        private void Awake()
        {
            _cameraController = new CameraController(MainCamera);
            _gameController   = new   GameController(this,
                                                     MainCamera.OnBorderExit,
                                                     _cameraController.OnBorderExitAnim,
                                                     MainCamera.leftCollider,
                                                     MainCamera.rightCollider);
            _eggController    = new    EggController(Egg);
            //TODO: Прокидывать события подбора и кидания яйца через менеджер а не инспектор
        }
    }
}
