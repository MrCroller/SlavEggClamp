using System;
using System.Collections.Generic;
using System.Linq;
using SEC.Associations;
using SEC.Character;
using SEC.Character.Controller;
using SEC.Enums;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngineTimers;
using PlayerInput = SEC.Character.Input.PlayerInput;


namespace SEC.Controller
{
    public partial class GameController : IDisposable
    {

        #region Fields

        public  UnityEvent<OrientationLR, Collider2D> OnFinish;
        private BoxCollider2D _leftBorder;
        private BoxCollider2D _rightBorder;
        private LayerMask _maskBorderOn;
        private LayerMask _maskBorderOff;
        private bool _killPassed;
        private bool _finishFlag = false;

        private readonly TimersPool Timers;
        private CameraController _cameraController;

        #endregion


        #region ClassLifeCicle

        public GameController(GameManager manager,
                              Dictionary<PlayerInput, CharacterController2D> playersList,
                              CameraController camera,
                              BoxCollider2D leftBorder,
                              BoxCollider2D rightBorder)
        {
            OnFinish = new();
            _playersDeathList = new(4);

            _cameraController = camera;

            _leftBorder = leftBorder;
            _rightBorder = rightBorder;
            _maskBorderOn = manager.MaskBorderOn;
            _maskBorderOff = manager.MaskBorderOff;
            _killPassed = manager.KillPassed;

            _spawnPointRight = manager.SpawnPointRight;
            _spawnPointLeft = manager.SpawnPointLeft;

            if (!_killPassed)
            {
                _leftBorder.excludeLayers = _maskBorderOff;
                _rightBorder.excludeLayers = _maskBorderOff;
            }

            _playersList = playersList;
            _egg = manager.Egg;

            Timers = TimersPool.GetInstance();

            Subscribe();
        }

        public void Dispose()
        {
            UnSubscribe();
        }

        private void Subscribe()
        {
            _cameraController.OnBorderExit.AddListener(OnBorderExit);
            _cameraController.OnAnimationEnd.AddListener(SpawnAllPlayers);

            foreach (var playerInput in _playersList.Keys)
            {
                playerInput.OnTakeEgg.AddListener(_egg.OnTakeHandler);
                playerInput.OnThrowEgg.AddListener(_egg.OnThrowHandler);
                playerInput.OnDeath.AddListener(KilledPlayer);
            }
        }

        private void UnSubscribe()
        {
            _cameraController.OnBorderExit.RemoveListener(OnBorderExit);
            _cameraController.OnAnimationEnd.RemoveListener(SpawnAllPlayers);

            foreach (var playerInput in _playersList.Keys)
            {
                playerInput.OnTakeEgg.RemoveListener(_egg.OnTakeHandler);
                playerInput.OnThrowEgg.RemoveListener(_egg.OnThrowHandler);
                playerInput.OnDeath.RemoveListener(KilledPlayer);
            }
        }

        #endregion


        #region Methods

        private void OnBorderExit(OrientationLR _)
        {
            MaskSwap(_maskBorderOn);

            KillPlayersWithoutEgg();
        }

        private void MaskSwap(LayerMask mask)
        {
            if (!_killPassed) return;

            _leftBorder.excludeLayers = mask;
            _rightBorder.excludeLayers = mask;
        }

        private void RestartGame(float timeForEnd)
        {
            _cameraController.LockTranslate = true;
            _finishFlag = true;
            Timers.StartTimer(
                () =>
                    {
                        Time.timeScale = 1f;
                        _cameraController.LockTranslate = false;
                        _finishFlag = false;
                        SceneManager.LoadScene(SceneAssociations.Game);
                    },
                timeForEnd);
        }

        #endregion

    }
}