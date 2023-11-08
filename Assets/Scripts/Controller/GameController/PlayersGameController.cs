using System.Collections.Generic;
using SEC.Associations;
using SEC.Character;
using SEC.Character.Controller;
using SEC.Character.Input;
using SEC.Enums;
using SEC.Helpers;
using UnityEngine;

namespace SEC.Controller
{
    public partial class GameController
    {

        #region Fields

        private readonly Transform _spawnPointRight;
        private readonly Transform _spawnPointLeft;
        private Dictionary<PlayerInput, CharacterController2D> _playersList;

        private DeadList<PlayerInput> _playersDeathList;

        private readonly EggInput _egg;

        #endregion


        #region Methods

        public void WhereWinner(OrientationLR orientation, Collider2D collider)
        {
            if (_finishFlag) return;

            if (collider.gameObject.layer == LayerAssociations.PlayerTakeEgg)
            {
                var player = collider.GetComponent<PlayerInput>();
                if (player.HomeSide == orientation)
                {
                    RestartGame(_playersList[player].Win());
                    MaskSwap(_maskBorderOn);
                }
            }
        }

        private void KilledPlayer(PlayerInput player)
        {
            MaskSwap(_maskBorderOff);

            Effects.CameraShake(_cameraShakeSetting.PlayerKill_Time, _cameraShakeSetting.PlayerKill_Forse);

            _playersDeathList.Add(player, EndMethod, _playersList[player].MovementSetting.DeathTime);

            void EndMethod()
            {
                SpawnPlayer(player);
            }
        }

        private void SpawnPlayer(PlayerInput player)
        {
            MaskSwap(_maskBorderOn);

            var position = player.HomeSide == OrientationLR.Left ? (Vector2)_spawnPointRight.position : (Vector2)_spawnPointLeft.position;
            _playersList[player].Spawn(position);
        }

        private void SpawnPlayer(PlayerInput player, OrientationLR orientation)
        {
            MaskSwap(_maskBorderOn);

            Vector2 position = orientation == OrientationLR.Left ? (Vector2)_spawnPointRight.position : (Vector2)_spawnPointLeft.position;
            _playersList[player].Spawn(position);
        }

        private void SpawnAllPlayers()
        {
            MaskSwap(_maskBorderOn);

            foreach (PlayerInput player in _playersDeathList)
            {
                var position = player.HomeSide == OrientationLR.Left ? (Vector2)_spawnPointRight.position : (Vector2)_spawnPointLeft.position;
                _playersList[player].Spawn(position);
            }
            _playersDeathList.Dispose();
        }

        private void KillPlayersWithoutEgg()
        {
            _playersDeathList.StopAll();
            foreach (var player in _playersList.Keys)
            {
                if (player.gameObject.layer != LayerAssociations.PlayerTakeEgg)
                {
                    _playersDeathList.Add(player);
                    _playersList[player].Death();
                }
            }
        }

        #endregion

    }
}
