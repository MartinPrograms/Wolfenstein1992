using System.Diagnostics;
using System.Drawing;
using Wolfenstein1992.Gamer;
using Wolfenstein1992.Gamer.Guns;
using Wolfenstein1992.Render;
using Wolfenstein1992.Render.HUD;

namespace Wolfenstein1992;

public class Game
{
    public int XWidth;
    public int YHeight;
    public byte[] Data;
    public Map map = new Map();
    public Player player = new Player();
    public GunRenderer gunRenderer = new GunRenderer();
    public bool lastShotHitEnemy = false;
    public Game(int xWidth, int yHeight)
    {
        Data = new byte[xWidth * yHeight * 4];
        XWidth = xWidth;
        YHeight = yHeight;
        gunRenderer.Gun = new Rifle();
    }

    public bool CanRender = false;
    public bool ForcedRender = false;

    public WolfTexture[] Textures = new[]
    {
        new WolfTexture("Assets/WallTextures/redbrick.png"),
        new WolfTexture("Assets/WallTextures/walltexture.bmp"),
        new WolfTexture("Assets/WallTextures/bluestone.png"),
        new WolfTexture("Assets/WallTextures/greystone.png"),
        new WolfTexture("Assets/WallTextures/mossy.png"),
        new WolfTexture("Assets/WallTextures/colorstone.png"),
        new WolfTexture("Assets/WallTextures/wood.png"),
        new WolfTexture("Assets/WallTextures/eagle.png")
    };
    public long LastRenderTime = 0; 
    public void Render()
    {
        if (!CanRender && !ForcedRender)
        {
            return;
        }
        ForcedRender = false;
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