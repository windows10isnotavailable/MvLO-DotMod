using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace NSMB.Utils
{

    public static class IceRunModeUtils
    {
        public static Color HideButtonColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        public static Color SkinRunnerColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        public static Color ScoreboardRunnerColor = new Color(1f, 1f, 1f, 0.5f);

        public static List<Enums.PowerupState> initPowerups = new()
        {
            Enums.PowerupState.Small,
            Enums.PowerupState.MiniMushroom,
            Enums.PowerupState.Mushroom,
            Enums.PowerupState.FireFlower,
            Enums.PowerupState.IceFlower,
            Enums.PowerupState.PropellerMushroom,
            Enums.PowerupState.BlueShell
        };

        public static GameObject GetGameObjectClone(GameObject obj)
        {
            var clone = GameObject.Instantiate(obj) as GameObject;
            clone.transform.parent = obj.transform.parent;
            clone.transform.localPosition = obj.transform.localPosition;
            clone.transform.localScale = obj.transform.localScale;
            return clone;
        }

        public static bool IsExistSpecial(Enums.LevelSpecial levelSpecial)
        {
            return GameManager.Instance.levelSpecialSets.Contains(levelSpecial);
        }

        public static void SetRandomRunner(PlayerController invokePl = null)
        {
            List<PlayerController> candidateRunners = new List<PlayerController>();

            foreach (PlayerController pl in GameManager.Instance.players)
            {
                if (pl == null || pl.photonView == null || pl.photonView.Owner == null) continue;
                if (pl == invokePl) continue;
                if (pl.lives == 0) continue;
                if (NetworkUtils.IsSpectator(pl.photonView.Owner)) continue;
                if (pl.isRunner) continue;

                candidateRunners.Add(pl);
            }

            int targetRunnerIndex = Random.Range(0, candidateRunners.Count);

            if (invokePl != null)
                candidateRunners[targetRunnerIndex].photonView.RPC("ForceChangeState", RpcTarget.All);
            candidateRunners[targetRunnerIndex].photonView.RPC("SetRunner", RpcTarget.All, true);
        }
    }
}