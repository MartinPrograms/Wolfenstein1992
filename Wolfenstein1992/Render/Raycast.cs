using System.Numerics;
using Wolfenstein1992.Gamer;
using Wolfenstein1992.Gamer.Entities;

namespace Wolfenstein1992.Render;

public class Raycast
{
    // This raycast is used by guns to determine if they hit an entity
    public ListEntity? HitEntity { get; set; }
    public bool WasEnemyHit { get; set; }

    public Raycast(Game game, Player player)
    {
        Vector2 dir = new Vector2((float)player.DirX, (float)player.DirY);

        var entities = game.map.Entities;
        var map = game.map;
        
        var rayStep = 0.1f;
        var rayLength = 0f;
        var MAX_RAY_LENGTH = 100f;
        
        Vector2 rayOrigin = new Vector2((float)player.PosX, (float)player.PosY);
        
        var rayDir = Vector2.Normalize(dir);

        while (rayLength < MAX_RAY_LENGTH)
        {
            Vector2 CurrentRayPos = rayOrigin + rayDir * rayLength;
            int wallX = (int)CurrentRayPos.X;
            int wallY = (int)CurrentRayPos.Y;

            if (map.MapData[wallX][wallY] != 0)
            {
                return; // we hit a wall
            }
                
            if(wallX > map.Width || wallY > map.Height || wallX < 0 || wallY < 0)
            {
                return; // we hit the edge of the map
            }
            foreach (var entity in entities)
            {
                
                
                
                var entitySize = entity.Size;
                var entityPos = new Vector2((float)entity.PosX, (float)entity.PosY);

                // if we are inside the entity
                if (CurrentRayPos.X > entityPos.X && CurrentRayPos.X < entityPos.X + entitySize.X &&
                    CurrentRayPos.Y > entityPos.Y && CurrentRayPos.Y < entityPos.Y + entitySize.Y)
                {
                    HitEntity = entity;
                    if (entity is Enemy)
                    {
                        WasEnemyHit = true;
                    }
                    
                    return;
                }
            }
            
            rayLength += rayStep;
        }
        
        HitEntity = null;
    }
}