namespace Wolfenstein1992.Gamer.Guns;

public class Gun
{
    public  int Damage { get; set; }
    public  int Range { get; set; }
    public  int Ammo { get; set; }
    public  int MaxAmmo { get; set; }
    public  float ReloadTime { get; set; }
    public  int FireRate { get; set; }
    public  bool Automatic { get; set; }
    public  GunType GunType { get; set; }
    public bool canFire = true;
    public  WolfTexture[] Textures { get; set; }
    public int CurrentTexture { get; set; }
    public int Width { get; set; }

    public virtual void Fire(Game game, Player player)
    {
        
    }
    
    public virtual void UnFire(Game game, Player player)
    {
        
    }
    
    public virtual void ResetAnimation()
    {
        CurrentTexture = 0;
    }
    
    public virtual void Reload(Game game, Player player)
    {
        
    }
}

public enum GunType
{
    Hitscan,
    Projectile,
    Melee // Melee's are just guns with a range of 1
}