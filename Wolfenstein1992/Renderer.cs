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


        foreach (var entity in Game.map.Entities)
        {
            if(Game.CanRender)
                entity.Update(Game, obj);
        }

        if (Game.CanRender)
        {
            if (_inputContext.Mice[0].IsButtonPressed(MouseButton.Left))
            {
                if (Game.gunRenderer.Gun.canFire)
                {
                    Game.gunRenderer.Gun.Fire(Game, Game.player);
                    Thread animReset = new Thread(() =>
                    {
                        Thread.Sleep(200);
                        Game.gunRenderer.Gun.ResetAnimation();
                    });
                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(Game.gunRenderer.Gun.FireRate);
                        Game.gunRenderer.Gun.UnFire(Game, Game.player);
                    });

                    thread.Start();
                    animReset.Start();
                }
            }
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
    private int[,] TempMapData;
    private void ImguiStuff()
    {
        // ImGui Docking
        ImGui.BeginMainMenuBar();
        if (ImGui.BeginMenu("File"))
        {
            if (ImGui.MenuItem("Save ImGui Settings"))
            {
                ImGui.SaveIniSettingsToDisk("imgui.ini");
            }
            if (ImGui.MenuItem("Exit"))
            {
                _window.Close();
            }
            ImGui.EndMenu();
        }
        
        if (ImGui.BeginMenu("Edit"))
        {
            ImGui.EndMenu();
        }
        
        if (ImGui.BeginMenu("View"))
        {
            ImGui.EndMenu();
        }
        
        ImGui.EndMainMenuBar();

        ImGui.DockSpaceOverViewport();

        ImGui.Begin("Main");
        ImGui.Text("Wolfenstein 1992");
        ImGui.End();
        
        ImGui.Begin("Render:");
        ImGui.Image((IntPtr)RenderResult._handle, new Vector2(Game.XWidth, Game.YHeight));
        ImGui.End();
        
        ImGui.Begin("Data:");
        ImGui.Checkbox("Can Render", ref Game.CanRender);
        ImGui.Text("Last Shot Hit Enemy: " + Game.lastShotHitEnemy);
        if(ImGui.Button("RENDER!"))
        {
            Game.ForcedRender = true;
        }
        ImGui.Text("Loaded map width and height: " + Game.map.Width + ", " + Game.map.Height);
        
        ImGui.Text("Render Time (ms): " + Game.LastRenderTime);
        ImGui.Text("FPS: " + 1000 / (Game.LastRenderTime+0.01));
        ImGui.Text("Player X: " + Game.player.PosX);
        ImGui.Text("Player Y: " + Game.player.PosY);
        ImGui.End();

        ImGui.Begin("Map Editor:");
        if (Game.CanRender)
        {
            ImGui.Text("Disable rendering to edit map");
        }
        else
        {
            ImGui.Text("Map Width: " + Game.map.Width);
            ImGui.Text("Map Height: " + Game.map.Height);
        }
        ImGui.End();
    }

    private void OnLoad()
    {
        _gl = GL.GetApi(_window);
        _inputContext = _window.CreateInput();
        _imGuiController = new ImGuiController(_gl, _window, _inputContext);
        Game = new Game(800, 800);
        RenderResult = new Texture(_gl, Game.Data, 800, 800);
        // Enable docking
        ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        
        // Load imgui from file
        if(File.Exists("imgui.ini"))
            ImGui.LoadIniSettingsFromDisk("imgui.ini");
        
        _inputContext.Keyboards[0].KeyDown += delegate(IKeyboard keyboard, Key key, int arg3)
        {
            if (key == Key.Escape)
            {
                _window.Close();
            }

            if (key == Key.M)
            {
                LockMouse = !LockMouse;
            }
        };
        
        _inputContext.Mice[0].MouseMove += OnMouseMove;
    }

    private bool LockMouse = false;
    private Vector2 Delta;
    private Vector2 LastDelta;
    private Vector2 Middle;
    private void OnMouseMove(IMouse arg1, Vector2 arg2)
    {
        if (LockMouse)
        {
            var delta = arg2 - Delta;
            Delta = arg2;
            Game.player.MouseMove(delta.X);
            
            arg1.Cursor.CursorMode = CursorMode.Hidden;
            arg1.Position = new Vector2(_window.Size.X / 2, _window.Size.Y / 2);
            Delta = new Vector2(_window.Size.X / 2, _window.Size.Y / 2);
        }
        else
        {
            arg1.Cursor.CursorMode = CursorMode.Normal;
        }
    }
}