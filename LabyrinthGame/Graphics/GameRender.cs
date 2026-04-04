using GameCore.Map;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
//using System.Diagnostics;

public class GameRenderer : IDisposable
{
    private const string VertexShaderSource = @"#version 330 core
        layout (location = 0) in vec3 aPosition;
        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;
        void main() {
            gl_Position = projection * view * model * vec4(aPosition, 1.0);
        }";

    private const string FragmentShaderSource = @"#version 330 core
        out vec4 FragColor;
        uniform vec4 uColor;
        void main() {
            FragColor = uColor;
        }";

    private int _vao, _vbo, _ebo, _shaderProgram;

    private readonly float[] _vertices = {
         0.5f,  0.5f, 0.0f,
         0.5f, -0.5f, 0.0f,
        -0.5f, -0.5f, 0.0f,
        -0.5f,  0.5f, 0.0f
    };

    private readonly uint[] _indices = { 0, 1, 3, 1, 2, 3 };
    
    public void Init()
    {
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        _shaderProgram = CreateProgram(VertexShaderSource, FragmentShaderSource);

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.BindVertexArray(0);
    }

    public void Draw(LabyrinthMap map, Color4 wallColor, Color4 emptyColor, Matrix4 view, Matrix4 projection)
    {
        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(_vao);

        SetMatrix("view", view);
        SetMatrix("projection", projection);

        for (int x = 0; x < map.Width(); x++)
        {
            for (int y = 0; y < map.Height(); y++)
            {
                Color4 color = map.IsWall(x, y) ? wallColor : emptyColor;
                var worldPos = map.ConvertToWorldCoordinates(x, y);
                DrawSquare(worldPos.X, worldPos.Y, LabyrinthMap.TileSize, LabyrinthMap.TileSize, color);
            }
        }
    }

    public void DrawSquare(float x, float y, float width, float height, Color4 color)
    {
        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(_vao);

        GL.Uniform4(GL.GetUniformLocation(_shaderProgram, "uColor"), color);

        Matrix4 model = Matrix4.CreateScale(width, height, 1.0f) * Matrix4.CreateTranslation(x, y, 0.0f);
        SetMatrix("model", model);

        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    }

    private void SetMatrix(string name, Matrix4 matrix)
    {
        GL.UniformMatrix4(GL.GetUniformLocation(_shaderProgram, name), false, ref matrix);
    }

    private int CreateProgram(string vSource, string fSource)
    {
        int vShader = CompileShader(ShaderType.VertexShader, vSource);
        int fShader = CompileShader(ShaderType.FragmentShader, fSource);
        int program = GL.CreateProgram();
        GL.AttachShader(program, vShader);
        GL.AttachShader(program, fShader);
        GL.LinkProgram(program);
        GL.DeleteShader(vShader);
        GL.DeleteShader(fShader);
        return program;
    }

    private int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);
        return shader;
    }

    public void Dispose()
    {
        GL.DeleteProgram(_shaderProgram);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        GL.DeleteVertexArray(_vao);
    }
}