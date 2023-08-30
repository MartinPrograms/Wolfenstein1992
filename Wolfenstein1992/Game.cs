using System.Diagnostics;
using System.Drawing;
using Wolfenstein1992.Gamer;
using Wolfenstein1992.Render;

namespace Wolfenstein1992;

public class Game
{
    public int XWidth;
    public int YHeight;
    public byte[] Data;
    public Map map = new Map();
    public Player player = new Player();
    public Game(int xWidth, int yHeight)
    {
        Data = new byte[xWidth * yHeight * 4];
        XWidth = xWidth;
        YHeight = yHeight;
    }

    public bool CanRender = true;

    public WolfTexture[] Textures = new[]
    {
        new WolfTexture("Textures/redbrick.png"),
        new WolfTexture("Textures/walltexture.bmp"),
        new WolfTexture("Textures/bluestone.png"),
        new WolfTexture("Textures/greystone.png"),
        new WolfTexture("Textures/mossy.png"),
        new WolfTexture("Textures/colorstone.png"),
        new WolfTexture("Textures/wood.png"),
        new WolfTexture("Textures/eagle.png")
    };
    public long LastRenderTime = 0; 
    public unsafe void Render()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        var colData = Raycaster.Render(this, map, player);
        
        // Convert to byte using marshall and convert it to rgba
        for(int i = 0; i < colData.Length; i++)
        {
            Data[i * 4] = colData[i].R;
            Data[i * 4 + 1] = colData[i].G;
            Data[i * 4 + 2] = colData[i].B;
            Data[i * 4 + 3] = colData[i].A;
        }
        sw.Stop();
        LastRenderTime = sw.ElapsedMilliseconds;
    }

    public void MoveForward(double d)
    {
        if(map.MapData[(int)(player.PosX + player.DirX * d * player.MoveSpeed)][(int)player.PosY] == 0)
        {
            player.PosX += player.DirX * d * player.MoveSpeed;
        }
        
        if(map.MapData[(int)player.PosX][(int)(player.PosY + player.DirY * d * player.MoveSpeed)] == 0)
        {
            player.PosY += player.DirY * d * player.MoveSpeed;
        }
    }

    public void MoveBackward(double d)
    {
        if (map.MapData[(int)(player.PosX - player.DirX * d * player.MoveSpeed)][(int)player.PosY] == 0)
        {
            player.PosX -= player.DirX * d * player.MoveSpeed;
        }
        
        if (map.MapData[(int)player.PosX][(int)(player.PosY - player.DirY * d * player.MoveSpeed)] == 0)
        {
            player.PosY -= player.DirY * d * player.MoveSpeed;
        }
    }

    public void MoveRight(double d)
    {
        if (map.MapData[(int)(player.PosX + player.DirY * d * player.MoveSpeed)][(int)player.PosY] == 0)
        {
            player.PosX += player.DirY * d * player.MoveSpeed;
        }
        
        if (map.MapData[(int)player.PosX][(int)(player.PosY - player.DirX * d * player.MoveSpeed)] == 0)
        {
            player.PosY -= player.DirX * d * player.MoveSpeed;
        }
    }

    public void MoveLeft(double d)
    {
        if (map.MapData[(int)(player.PosX - player.DirY * d * player.MoveSpeed)][(int)player.PosY] == 0)
        {
            player.PosX -= player.DirY * d * player.MoveSpeed;
        }
        
        if (map.MapData[(int)player.PosX][(int)(player.PosY + player.DirX * d * player.MoveSpeed)] == 0)
        {
            player.PosY += player.DirX * d * player.MoveSpeed;
        }
    }
}