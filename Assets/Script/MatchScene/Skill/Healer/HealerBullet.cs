using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class HealerBullet : BaseAttack
{
    [SerializeField]
    private int healValue = 3;

    public override bool BaseCollisionProcess(BasePlayer player)
    {
        if (IsAttackable(PhotonNetwork.LocalPlayer, player.photonView.Owner))
            player.OnDamaged(AttackDamage);
        else
        {
            Enums.TeamOption team = (Enums.TeamOption)player.photonView.Owner.CustomProperties[Enums.PlayerProperties.TEAM];
            Enums.TeamOption myTeam = (Enums.TeamOption)ownerPlayer.photonView.Owner.CustomProperties[Enums.PlayerProperties.TEAM];

            if (team == myTeam)
                player.OnHeal(healValue);
        }

        PhotonNetwork.Destroy(this.gameObject);

        return true;
    }
}
