using System.Diagnostics;
using SEC.Controller;

namespace SEC.Helpers
{
    internal class Effects
    {
        private static CameraController _cameraController;

        public Effects(CameraController cameraController)
        {
            _cameraController = cameraController;
        }

        #region EffectMethods

        public static void CameraShake(float time, float strength)
        {
            if(_cameraController == null)
            {
                throw new System.NullReferenceException($"{nameof(CameraController)} не задан");
            }

            _cameraController.Shake(time, strength);
        }

        #endregion

    }
}
