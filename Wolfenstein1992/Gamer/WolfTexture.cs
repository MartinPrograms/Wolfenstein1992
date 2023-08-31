using System.Drawing;
using SkiaSharp;

namespace Wolfenstein1992.Gamer;

public class WolfTexture
{
    public int Width = 64;
    public int Height = 64;
    public Color[,] Data = new Color[64 ,64];

    public WolfTexture()
    {
        for (int i = 0; i < 64 * 64; i++)
        {
            Data[i,i] = Color.FromArgb(255, 255, 0, 255);
        }
    }
    
    public WolfTexture(Color[,] data, int width, int height)
    {
        Data = data;
        Width = width;
        Height = height;
    }
    
    public WolfTexture(string path)
    {
        Data = WolfLoader.LoadFile(path, Width, Height);
    }

    public Color GetPixel(int texX, int texY)
    {
        if(texX < 0 || texX >= Width || texY < 0 || texY >= Height)
        {
            return Color.FromArgb(255, 0, 0, 0);
        }
        else
        {
            return Data[texX, texY];
        }
    }
}

public class WolfLoader
{
    public static Color[,] LoadFile(string path, int ResX, int ResY)
    {
        var bmp = new SKBitmap();
        bmp = SKBitmap.Decode(path);
        
        var data = new Color[ResX , ResY];
        
        for (int x = 0; x < ResX; x++)
        {
            for (int y = 0; y < ResY; y++)
            {
                var skCol = bmp.GetPixel(x, y);
                data[x,y] = Color.FromArgb(skCol.Alpha, skCol.Red, skCol.Green, skCol.Blue);
            }
        }
        
        return data;
    }
}