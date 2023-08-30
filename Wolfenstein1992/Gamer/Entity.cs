namespace Wolfenstein1992.Gamer;

public class Entity
{
    public double PosX;
    public double PosY;
    public WolfTexture Texture;
    
    public Entity(double posX, double posY, WolfTexture texture)
    {
        PosX = posX;
        PosY = posY;
        Texture = texture;
    }

    public Entity()
    {
        
    }
}