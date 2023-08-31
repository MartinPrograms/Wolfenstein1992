using System.Numerics;

namespace Wolfenstein1992.Gamer.Entities;

public class OrbMonster : Enemy
{
    public int damage = 10;
    public int speed = 1;
    public int attackSpeed = 2;
    
    public OrbMonster(double posX, double posY, string[] path, Vector2 size, float vmove) : base(posX, posY, path, size, vmove)
    {
    }
    
    public OrbMonster()
    {
        
    }
    
    public override void Update(Game game, double deltaTime)
    {
        
    }
}