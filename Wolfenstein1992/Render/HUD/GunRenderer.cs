using System.Drawing;
using Wolfenstein1992.Gamer.Guns;

namespace Wolfenstein1992.Render.HUD;

public class GunRenderer
{
    public Gun Gun;
    
    public Color GetPixel(int x, int y)
    {
        return Gun.Textures[Gun.CurrentTexture].GetPixel(x, y);
    }
}