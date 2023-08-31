using Wolfenstein1992.Gamer.Entities;
using Wolfenstein1992.Render;

namespace Wolfenstein1992.Gamer.Guns;

public class Rifle : Gun
{
    public Rifle()
    {
        Ammo = 17;
        MaxAmmo = 17;
        Damage = 500;
        Range = 100;
        ReloadTime = 3; // Seconds
        FireRate = 350; // fire every 350ms
        Automatic = false;
        GunType = GunType.Hitscan;
        Textures = new WolfTexture[2];
        Textures[0] = new WolfTexture("Assets/Guns/Rifle/0002.png"); // idle
        Textures[1] = new WolfTexture("Assets/Guns/Rifle/0001.png"); // fire
        Width = 512;
    }

    public override void Fire(Game game, Player player)
    {
        if (canFire)
        {
            CurrentTexture = 1;
            canFire = false;
            
            var raycast = new Raycast(game, player);
            if (raycast.HitEntity != null)
            {
                if (raycast.WasEnemyHit)
                {
                    var enemy = raycast.HitEntity as Enemy;
                    enemy.TakeDamage(Damage);
                    game.lastShotHitEnemy = true;
                }
                
            }
            else
            {
                game.lastShotHitEnemy = false;
            }
        }
    }

    public override void ResetAnimation()
    {
        CurrentTexture = 0;
    }

    public override void UnFire(Game game, Player player)
    {
        
        canFire = true;
    }
}