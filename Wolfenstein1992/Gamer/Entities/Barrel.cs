using System.Numerics;

namespace Wolfenstein1992.Gamer.Entities;

public class Barrel : Entity
{
    public Barrel(double posX, double posY, WolfTexture texture, Vector2 size, float vmove) : base(posX, posY, texture, size, vmove)
    {
    }
    
    public Barrel()
    {
        
    }
}