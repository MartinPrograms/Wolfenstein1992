using System.Numerics;

namespace Wolfenstein1992.Gamer;

public class MultiSidedEntity : ListEntity
{
    public override double PosX { get; set; }
    public override double PosY { get; set; }
    public WolfTexture[] Texture;
    public override Vector2 Size { get; set; }
    public override Vector2 Dir { get; set; }

    public MultiSidedEntity(double posX, double posY, string[] path, Vector2 size, float vMove)
    {
        PosX = posX;
        PosY = posY;
        Texture = new WolfTexture[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            Texture[i] = new WolfTexture(path[i]);
        }
        Size = size;
        VMove = vMove;
    }
    

    public MultiSidedEntity()
    {
        
    }

    public float VMove { get; set; }

    public override WolfTexture GetTexture(Player player, ListEntity entity)
    {
        // Calculate the direction of the entity
        // Example on how to do it: Then when rendering the sprite, you must choose which of the 8 angles to draw. This depends on the location of the player versus the location of the object in the 2D map. Calculate the differences dx and dy between player x,y coordinate and object x,y coordinate. Then take the atan2 of dx and dy to get the angle. Then round it to the nearest of the 8 supported angles, that is the index of the texture to choose.
        var dx = PosX - player.PosX;
        var dy = PosY - player.PosY;
        var angle = Math.Atan2(dy, dx);
        var angleDeg = angle * 180 / Math.PI;
        var angleDegRounded = Math.Round(angleDeg / 45) * 45;
        var index = (int)angleDegRounded / 45;
        if(index > 7)
        {
            index -= 8;
        }
        if(index < 0)
        {
            index += 8;
        }
        return Texture[index];
    }
    
}