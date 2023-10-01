using System;
using System.Collections.Generic;
using System.Linq;
using SEC.Associations;
using SEC.Character.Input;
using SEC.Enum;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineTimers;


namespace SEC.Controller
{
    public class GameController : IDisposable
    {
        private UnityEvent<OrientationLR> _onBorderExitAnim;
        private UnityEvent<OrientationLR> _onBorderExit;
        private BoxCollider2D _leftBorder;
        private BoxCollider2D _rightBorder;
        private LayerMask _maskBorderOn;
        private LayerMask _maskBorderOff;

        private Transform _spawnPointRight;
        private Transform _spawnPointLeft;
        private PlayerInput[] _players;

        private List<PlayerInput> _playersDeathList;
        private IStop _timerKillPlayer;

        public GameController(GameManager manager,
                              UnityEvent<OrientationLR> onBorderExit,
                              UnityEvent<OrientationLR> onBorderExitAnim,
                              BoxCollider2D leftBorder,
                              BoxCollider2D rightBorder)
        {
            _players = manager.Players;

            foreach (var player in _players)
            {
                player.OnDeath.AddListener(() => KillPlayer(player));
            }

            _leftBorder = leftBorder;
            _rightBorder = rightBorder;
            _maskBorderOn = manager.MaskBorderOn;
            _maskBorderOff = manager.MaskBorderOff;

            _spawnPointRight = manager.SpawnPointRight;
            _spawnPointLeft = manager.SpawnPointLeft;
            _onBorderExit = onBorderExit;
            _onBorderExitAnim = onBorderExitAnim;

            _onBorderExit.AddListener(OnBorderExit);
            _onBorderExitAnim.AddListener(SpawnAllPlayer);
        }

        public void Dispose()
        {
            _onBorderExit.RemoveListener(OnBorderExit);
            _onBorderExitAnim.RemoveListener(SpawnAllPlayer);

            foreach (var player in _players)
            {
                player.OnDeath.RemoveAllListeners();
            }
        }

        private void OnBorderExit(OrientationLR _)
        {
            _timerKillPlayer?.Stop();
            MaskSwap(_maskBorderOn);

            _playersDeathList = _players.Where(player => player.gameObject.layer != LayerAssociations.PlayerTakeEgg).ToList();
            foreach (var player in _playersDeathList)
            {
                player.gameObject.SetActive(false);
            }
        }

        private void KillPlayer(PlayerInput player)
        {
            MaskSwap(_maskBorderOff);

            player.gameObject.SetActive(false);
            _timerKillPlayer = TimersPool.GetInstance().StartTimer(EndMethod, player.DeathTime);

            void EndMethod()
            {
                SpawnPlayer(player, player.HomeSide);
            }
        }

        private void SpawnPlayer(PlayerInput player, OrientationLR orientation)
        {
            MaskSwap(_maskBorderOn);

            Vector2 save = orientation == OrientationLR.Right ? _spawnPointRight.position : _spawnPointLeft.position;
            player.gameObject.SetActive(true);
            player.transform.position = save;
        }

        private void SpawnAllPlayer(OrientationLR orientation)
        {


            Vector2 save = orientation == OrientationLR.Right ? _spawnPointRight.position : _spawnPointLeft.position;
            foreach (var player in _playersDeathList)
            {
                player.gameObject.SetActive(true);
                player.transform.position = save;
            }
            _playersDeathList.Clear();
        }

        private void MaskSwap(LayerMask mask)
        {
            _leftBorder.excludeLayers = mask;
            _rightBorder.excludeLayers = mask;
        }
    }
}