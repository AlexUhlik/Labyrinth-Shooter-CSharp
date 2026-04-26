using GameCore.Map;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Drawing;
using System.Drawing.Imaging;

public class GameRenderer : IDisposable
{
    private const string VertexShaderSource = @"#version 330 core
        layout (location = 0) in vec3 aPosition;
        layout (location = 1) in vec2 aTexCoord;
        out vec2 TexCoord;
        uniform mat4 model;
        uniform mat4 view;
        uniform mat4 projection;
        void main() {
            gl_Position = projection * view * model * vec4(aPosition, 1.0);
            TexCoord = aTexCoord;
        }";

    private const string FragmentShaderSource = @"#version 330 core
        out vec4 FragColor;
        in vec2 TexCoord;
        uniform vec4 uColor;
        uniform sampler2D uTexture;
        uniform bool uUseTexture;
        void main() {
            if (uUseTexture)
                FragColor = texture(uTexture, TexCoord) * uColor;
            else
                FragColor = uColor;
        }";

    private int _vao, _vbo, _ebo, _shaderProgram;

    private readonly float[] _vertices = {
         0.5f,  0.5f, 0.0f, 1.0f, 1.0f,
         0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f,
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f
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

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.BindVertexArray(0);
    }

    public void SetMatrices(Matrix4 view, Matrix4 projection)
    {
        GL.UseProgram(_shaderProgram);
        SetMatrix("view", view);
        SetMatrix("projection", projection);
    }

    public void DrawSquare(float x, float y, float width, float height, Color4 color, int textureId = -1, float rotation = 0f)
    {
        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(_vao);
        GL.Uniform4(GL.GetUniformLocation(_shaderProgram, "uColor"), color);

        int useTexLoc = GL.GetUniformLocation(_shaderProgram, "uUseTexture");
        if (textureId != -1)
        {
            GL.Uniform1(useTexLoc, 1);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }
        else
        {
            GL.Uniform1(useTexLoc, 0);
        }

        Matrix4 model = Matrix4.CreateScale(width, height, 1.0f);

        if (rotation != 0)
        {
            model *= Matrix4.CreateRotationZ(rotation);
        }

        model *= Matrix4.CreateTranslation(x, y, 0.0f);

        SetMatrix("model", model);
        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    }

    public int LoadTexture(string path)
    {
        if (!System.IO.File.Exists(path)) return -1;

        int id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, id);

        using (Bitmap bmp = new Bitmap(path))
        {
            BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        return id;
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