using System.Numerics;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

namespace Wolfenstein1992;

public class Renderer
{
    private int _xWidth;
    private int _yWidth;
    private string _name;
    private int _refreshRate;
    private static IWindow _window;
    
    private GL _gl;
    private ImGuiController _imGuiController;
    private IInputContext _inputContext;

    public Renderer(int xWidth, int yWidth, string name, int refreshRate)
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(xWidth, yWidth);
        options.Title = name;
        options.VSync = false;
        options.FramesPerSecond = refreshRate;
        _window = Window.Create(options);
        
        _window.Load += OnLoad;
        _window.Render += OnRender;
        _window.Update += OnUpdate;

        _window.Run();
        
        _window.Dispose();
    }

    private void OnUpdate(double obj)
    {
        if(_inputContext.Keyboards[0].IsKeyPressed(Key.W))
        {
            Game.MoveForward(obj);
        }
        if(_inputContext.Keyboards[0].IsKeyPressed(Key.S))
        {
            Game.MoveBackward(obj);
        }
        

        if (_inputContext.Keyboards[0].IsKeyPressed(Key.D))
        {
            Game.MoveRight(obj);
        }

        if (_inputContext.Keyboards[0].IsKeyPressed(Key.A))
        {
            Game.MoveLeft(obj);
        }

        if (_inputContext.Keyboards[0].IsKeyPressed(Key.Left))
        {
            Game.player.RotateLeft(obj);
        }
        
        if (_inputContext.Keyboards[0].IsKeyPressed(Key.Right))
        {
            Game.player.RotateRight(obj);
        }
    }

    private void OnRender(double obj)
    {
        _gl.Clear((uint)ClearBufferMask.ColorBufferBit);
        Game.Render();
        RenderResult.SetData(Game.Data, (uint)Game.XWidth, (uint)Game.YHeight);
        RenderResult.Bind();
        _imGuiController.Update((float)obj);
        ImguiStuff();
        _imGuiController.Render();
    }

    private Texture RenderResult;
    public Game Game;
    private void ImguiStuff()
    {
        ImGui.Begin("Render:");
        ImGui.Image((IntPtr)RenderResult._handle, new Vector2(Game.XWidth, Game.YHeight));
        ImGui.End();
        
        ImGui.Begin("Data:");
        ImGui.Checkbox("Can Render", ref Game.CanRender);
        ImGui.Text("Loaded map width and height: " + Game.map.Width + ", " + Game.map.Height);
        
        ImGui.Text("Render Time (ms): " + Game.LastRenderTime);
        ImGui.Text("FPS: " + 1000 / Game.LastRenderTime);
        ImGui.Text("Player X: " + Game.player.PosX);
        ImGui.Text("Player Y: " + Game.player.PosY);

        ImGui.End();

    }

    private void OnLoad()
    {
        _gl = GL.GetApi(_window);
        _inputContext = _window.CreateInput();
        _imGuiController = new ImGuiController(_gl, _window, _inputContext);
        Game = new Game(800, 800);
        RenderResult = new Texture(_gl, Game.Data, 800, 800);
        
        
        _inputContext.Keyboards[0].KeyDown += delegate(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
            {
                _window.Close();
            }
        };
    }
}