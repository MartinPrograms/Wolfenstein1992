using System.Numerics;

namespace Wolfenstein1992.Gamer;

public class Entity : ListEntity
{
    public override double PosX { get; set; }
    public override double PosY { get; set; }
    public WolfTexture Texture;
    public override Vector2 Size { get; set; }
    public override Vector2 Dir { get; set; } = new Vector2(1, 0);
    
    public Entity(double posX, double posY, WolfTexture texture, Vector2 size, float vMove)
    {
        PosX = posX;
        PosY = posY;
        Texture = texture;
        Size = size;
        VMove = vMove;
    }
    

    public Entity()
    {
        
    }

    public override float VMove { get; set; }
    
    public override WolfTexture GetTexture(Player player, ListEntity entity)
    {
        return Texture;
    }
}

public class ListEntity
{
    public virtual double PosX { get; set; }
    public virtual double PosY { get; set; }
    public virtual float VMove { get; set; }
    public virtual Vector2 Size { get; set; }
    

    public virtual void Update(Game game, double deltaTime)
    {
        
    }
    
    public virtual WolfTexture GetTexture(Player player, ListEntity entity)
    {
        return null;
    }
    
    public virtual Vector2 Dir { get; set; }
}