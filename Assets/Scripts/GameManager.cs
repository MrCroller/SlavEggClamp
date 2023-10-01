using SEC.Controller;
using SEC.Input;
using UnityEngine;


namespace SEC
{
    public class GameManager : MonoBehaviour
    {
        public CameraInput MainCamera;

        private GameController _gameController;
        private CameraController _cameraController;

        private void Awake()
        {
            _gameController = new GameController();
            _cameraController = new CameraController(MainCamera);

            //LeftBorder.callbackLayers
        }

        public void OnLeftEnter()
        {
            Debug.Log("LeftTranslate");
            _cameraController.Translate(Enum.OrientationLR.Left);
        }

        public void OnRightEnter()
        {
            Debug.Log("RightTranslate");
            _cameraController.Translate(Enum.OrientationLR.Right);
        }
    }
}
