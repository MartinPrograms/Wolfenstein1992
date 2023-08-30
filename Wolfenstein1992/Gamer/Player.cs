namespace Wolfenstein1992.Gamer;

public class Player
{
    public double PosX = 19.5, PosY = 15; //x and y start position
    public double DirX = -1, DirY = 0; //initial direction vector
    public double PlaneX = 0, PlaneY = 0.66; //the 2d raycaster version of camera plane
    
    public double MoveSpeed = 1; //the constant value is in squares/second
    public double RotSpeed = 1; //the constant value is in radians/second


    public void RotateRight(double d)
    {
        double oldDirX = DirX;
        DirX = DirX * Math.Cos(-RotSpeed * d) - DirY * Math.Sin(-RotSpeed * d);
        DirY = oldDirX * Math.Sin(-RotSpeed * d) + DirY * Math.Cos(-RotSpeed * d);
        double oldPlaneX = PlaneX;
        PlaneX = PlaneX * Math.Cos(-RotSpeed * d) - PlaneY * Math.Sin(-RotSpeed * d);
        PlaneY = oldPlaneX * Math.Sin(-RotSpeed * d) + PlaneY * Math.Cos(-RotSpeed * d);
    }
    
    public void RotateLeft(double d)
    {
        double oldDirX = DirX;
        DirX = DirX * Math.Cos(RotSpeed * d) - DirY * Math.Sin(RotSpeed * d);
        DirY = oldDirX * Math.Sin(RotSpeed * d) + DirY * Math.Cos(RotSpeed * d);
        double oldPlaneX = PlaneX;
        PlaneX = PlaneX * Math.Cos(RotSpeed * d) - PlaneY * Math.Sin(RotSpeed * d);
        PlaneY = oldPlaneX * Math.Sin(RotSpeed * d) + PlaneY * Math.Cos(RotSpeed * d);
    }
}