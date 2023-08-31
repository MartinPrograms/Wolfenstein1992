using System.Numerics;
using Wolfenstein1992.Gamer.Entities;

namespace Wolfenstein1992.Gamer;

public class Map
{
    public List<List<int>> MapData = new List<List<int>>();
    public int Width = 24;
    public int Height = 24;
    public List<ListEntity> Entities = new List<ListEntity>();

    public Map()
    {
        CreateMap();
        Entities.Add(new Barrel(18, 18, new WolfTexture("Assets/WallTextures/barrel.png"), new Vector2(1f,1f), 0f));
        Entities.Add(new OrbMonster(15,15, new []
        {
            "Assets/Enemies/Suzanne/0001.png",
            "Assets/Enemies/Suzanne/0002.png",
            "Assets/Enemies/Suzanne/0003.png",
            "Assets/Enemies/Suzanne/0004.png",
            "Assets/Enemies/Suzanne/0005.png",
            "Assets/Enemies/Suzanne/0006.png",
            "Assets/Enemies/Suzanne/0007.png",
            "Assets/Enemies/Suzanne/0008.png",
        }, Vector2.One, 0f));
    }
    
    public Map(string mapPath)
    {
        LoadMap(mapPath);
    }
    
    private void LoadMap(string mapPath)
    {
        
    }

    public void CreateMap()
    { 
        int[,] mapData= new int[,]
        {
            {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,7,7,7,7,7,7,7,7},
            {4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,7},
            {4,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
            {4,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
            {4,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,7},
            {4,0,4,0,0,0,0,5,5,5,5,5,5,5,5,5,7,7,0,7,7,7,7,7},
            {4,0,5,0,0,0,0,5,0,5,0,5,0,5,0,5,7,0,0,0,7,7,7,1},
            {4,0,6,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,0,0,0,8},
            {4,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,1},
            {4,0,8,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,0,0,0,8},
            {4,0,0,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,7,7,7,1},
            {4,0,0,0,0,0,0,5,5,5,5,0,5,5,5,5,7,7,7,7,7,7,7,1},
            {6,6,6,6,6,6,6,6,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,6},
            {8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
            {6,6,6,6,6,6,0,6,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,6},
            {4,4,4,4,4,4,0,4,4,4,6,0,6,2,2,2,2,2,2,2,3,3,3,3},
            {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,0,0,0,2},
            {4,0,0,0,0,0,0,0,0,0,0,0,6,2,0,0,5,0,0,2,0,0,0,2},
            {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,2,0,2,2},
            {4,0,6,0,6,0,0,0,0,4,6,0,0,0,0,0,5,0,0,0,0,0,0,2},
            {4,0,0,5,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,2,0,2,2},
            {4,0,6,0,6,0,0,0,0,4,6,0,6,2,0,0,5,0,0,2,0,0,0,2},
            {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,0,0,0,2},
            {4,4,4,4,4,4,4,4,4,4,1,1,1,2,2,2,2,2,2,3,3,3,3,3}
        };
        
        for (int x = 0; x < Width; x++)
        {
            //MapData.Add(new List<int>());
            for (int y = 0; y < Height; y++)
            {
                //MapData[x].Add(mapData[x,y]);
            }
        }
        
        // Make the map empty
        for (int x = 0; x < Width; x++)
        {
            MapData.Add(new List<int>());
            for (int y = 0; y < Height; y++)
            {
                MapData[x].Add(0);
                if (x == 0 || y == 0 || x == Width - 1 || y == Height - 1)
                {
                    MapData[x][y] = 1;
                }
            }
        }
    }
}