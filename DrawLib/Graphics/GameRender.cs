using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

/// <summary>
/// Класс отвечающий за отрисовку 2D графики с использованием OpenGL.
/// </summary>
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

    // Идентификаторы объектов OpenGL
    private int _vao; // Vertex Array Object
    private int _vbo; // Vertex Buffer Object
    private int _ebo; // Element Buffer Object
    private int _shaderProgram;

    // Кэш локаций uniform-переменных для оптимизации производительности
    private int _modelLoc, _viewLoc, _projLoc, _colorLoc, _useTexLoc;

    // Координаты вершин (X, Y, Z, U, V)
    private readonly float[] _vertices = {
         0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // Точка право-верх
         0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // Точка право-низ
        -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // Точка лево-низ
        -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // Точка лево-верх
    };

    // Порядок отрисовки треугольников
    private readonly uint[] _indices = { 0, 1, 3, 1, 2, 3 };

    /// <summary>
    /// Инициализирует буферы, шейдеры и настраивает параметры смешивания цветов.
    /// </summary>
    public void Init()
    {
        // Настройка прозрачности
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        GL.BindVertexArray(_vao);

        // Загрузка данных вершин
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        // Загрузка индексов
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        _shaderProgram = CreateProgram(VertexShaderSource, FragmentShaderSource);

        // Предварительное кэширование локаций uniform-переменных
        _modelLoc = GL.GetUniformLocation(_shaderProgram, "model");
        _viewLoc = GL.GetUniformLocation(_shaderProgram, "view");
        _projLoc = GL.GetUniformLocation(_shaderProgram, "projection");
        _colorLoc = GL.GetUniformLocation(_shaderProgram, "uColor");
        _useTexLoc = GL.GetUniformLocation(_shaderProgram, "uUseTexture");

        // Установка указателей атрибутов (позиция и текстурные координаты)
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.BindVertexArray(0);
    }

    /// <summary>
    /// Устанавливает матрицы камеры и проекции для всех последующих вызовов отрисовки.
    /// </summary>
    /// <param name="view">Матрица вида.</param>
    /// <param name="projection">Матрица проекции.</param>
    public void SetMatrices(Matrix4 view, Matrix4 projection)
    {
        GL.UseProgram(_shaderProgram);
        GL.UniformMatrix4(_viewLoc, false, ref view);
        GL.UniformMatrix4(_projLoc, false, ref projection);
    }

    /// <summary>
    /// Рисует квадрат с заданными параметрами.
    /// </summary>
    /// <param name="x">Позиция X центра объекта.</param>
    /// <param name="y">Позиция Y центра объекта.</param>
    /// <param name="width">Ширина объекта.</param>
    /// <param name="height">Высота объекта.</param>
    /// <param name="color">Множитель цвета или основной цвет без текстуры.</param>
    /// <param name="textureId">ID текстуры из LoadTexture. Если -1, используется заливка цветом.</param>
    /// <param name="rotation">Угол поворота в радианах.</param>
    public void DrawSquare(float x, float y, float width, float height, Color4 color, int textureId = -1, float rotation = 0f)
    {
        GL.UseProgram(_shaderProgram);
        GL.BindVertexArray(_vao);
        GL.Uniform4(_colorLoc, color);

        // Работа с текстурой
        if (textureId != -1)
        {
            GL.Uniform1(_useTexLoc, 1);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, textureId);
        }
        else
        {
            GL.Uniform1(_useTexLoc, 0);
        }

        // Вычисление матрицы модели: Масштаб -> Поворот -> Перемещение
        Matrix4 model = Matrix4.CreateScale(width, height, 1.0f);
        if (rotation != 0f)
            model *= Matrix4.CreateRotationZ(rotation);
        model *= Matrix4.CreateTranslation(x, y, 0.0f);

        GL.UniformMatrix4(_modelLoc, false, ref model);

        GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0);
    }

    /// <summary>
    /// Загружает изображение в видеопамять и возвращает уникальный ID текстуры.
    /// </summary>
    /// <param name="path">Путь к файлу изображения.</param>
    /// <returns>ID текстуры или -1 в случае ошибки.</returns>
    public int LoadTexture(string path)
    {
        if (!System.IO.File.Exists(path)) return -1;

        int id = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, id);

        using (Bitmap bmp = new Bitmap(path))
        {
            // Переворачиваем Bitmap, так как в OpenGL координаты Y идут снизу вверх
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
        }

        // Настройка фильтрации 
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        // Настройка повторения текстуры
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

        return id;
    }

    /// <summary>
    /// Компилирует и линкует шейдерную программу.
    /// </summary>
    private int CreateProgram(string vSource, string fSource)
    {
        int vShader = CompileShader(ShaderType.VertexShader, vSource);
        int fShader = CompileShader(ShaderType.FragmentShader, fSource);

        int program = GL.CreateProgram();
        GL.AttachShader(program, vShader);
        GL.AttachShader(program, fShader);
        GL.LinkProgram(program);

        // Очистка ресурсов после линковки
        GL.DetachShader(program, vShader);
        GL.DetachShader(program, fShader);
        GL.DeleteShader(vShader);
        GL.DeleteShader(fShader);

        return program;
    }

    /// <summary>
    /// Вспомогательный метод для компиляции отдельного шейдера.
    /// </summary>
    private int CompileShader(ShaderType type, string source)
    {
        int shader = GL.CreateShader(type);
        GL.ShaderSource(shader, source);
        GL.CompileShader(shader);

        // Проверка на ошибки компиляции
        string infoLog = GL.GetShaderInfoLog(shader);
        if (!string.IsNullOrEmpty(infoLog))
            Console.WriteLine($"Error in {type}: {infoLog}");

        return shader;
    }

    /// <summary>
    /// Освобождает ресурсы OpenGL, занятые рендерером.
    /// </summary>
    public void Dispose()
    {
        GL.DeleteProgram(_shaderProgram);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);
        GL.DeleteVertexArray(_vao);
    }
}