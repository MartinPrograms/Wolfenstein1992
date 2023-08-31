using System.Numerics;

namespace Wolfenstein1992.Gamer.Entities;

public class Enemy : MultiSidedEntity
{
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Speed { get; set; }
    public int AttackSpeed { get; set; }
    public float VisionRange { get; set; }
    public WolfTexture DeathTexture { get; set; }
    public Enemy(double posX, double posY, string[] path, Vector2 size, float vmove) : base(posX, posY, path, size, vmove)
    {
        Health = 100;
        Damage = 10;
        Speed = 1;
        AttackSpeed = 2;
        VisionRange = 10;

        DeathTexture = new WolfTexture("Assets/dead_placeholder.png");
    }
    
    public Enemy()
    {
        
    }
    
    public void TakeDamage(int damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            for(int i = 0; i < Texture.Length; i++)
            {
                Texture[i] = DeathTexture;
            }
        }
    }
}