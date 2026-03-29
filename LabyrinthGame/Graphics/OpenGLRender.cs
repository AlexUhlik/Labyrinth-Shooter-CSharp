using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
//using OpenTK.Mathematics; // Обязательно для Matrix4 и Color4
using System;
using System.IO;

namespace Application.Graphics
{
    public class OpenGlRender : IDisposable
    {
        private int _vertexBufferObject;
        private int _vertexArrayObject;
        private int _shaderProgram;
        private Matrix4 _projection;

        public OpenGlRender(string vertPath, string fragPath)
        {
            // 1. Данные для одного квадрата (от -0.5f до 0.5f)
            float[] vertices = {
                -0.5f, -0.5f,
                 0.5f, -0.5f,
                 0.5f,  0.5f,
                -0.5f,  0.5f
            };

            // 2. Создаем и заполняем буферы
            _vertexArrayObject = GL.GenVertexArray();
            _vertexBufferObject = GL.GenBuffer();

            GL.BindVertexArray(_vertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Атрибут позиции: 0 — индекс в шейдере (layout location = 0)
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // 3. Компиляция шейдеров
            _shaderProgram = CreateProgram(vertPath, fragPath);
        }

        public void SetupView(int width, int height, int mapW, int mapH, int tileSize)
        {
            GL.Viewport(0, 0, width, height);
            // Ортографическая проекция: (0,0) в левом верхнем углу
            //_projection = Matrix4.CreateOrthographicOffCenter(0, mapW * tileSize, mapH * tileSize, 0, -1f, 1f);

            _projection = Matrix4.CreateOrthographicOffCenter(
                0,
                mapW * tileSize,
                mapH * tileSize,
                0,
                -1f, 1f);
        }

        public void DrawRect(float x, float y, float size, Color4 color)
        {
            GL.UseProgram(_shaderProgram);

            // Привязываем VAO, чтобы видеокарта знала структуру вершин
            GL.BindVertexArray(_vertexArrayObject);

            // Передаем цвет
            int colorLoc = GL.GetUniformLocation(_shaderProgram, "uColor");
            GL.Uniform4(colorLoc, color);

            // Передаем проекцию
            int projLoc = GL.GetUniformLocation(_shaderProgram, "uProjection");
            GL.UniformMatrix4(projLoc, false, ref _projection);

            // Создаем матрицу модели
            // x, y — это центр квадрата. 
            Matrix4 model = Matrix4.CreateScale(size) * Matrix4.CreateTranslation(x, y, 0.0f);
            int modelLoc = GL.GetUniformLocation(_shaderProgram, "uModel");
            GL.UniformMatrix4(modelLoc, false, ref model);

            // Рисуем
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            // Отвязываем всё (необязательно, но полезно для отладки)
            GL.BindVertexArray(0);
            GL.UseProgram(0);
        }

        private int CreateProgram(string vertPath, string fragPath)
        {
            int vertexShader = CompileShader(ShaderType.VertexShader, vertPath);
            int fragmentShader = CompileShader(ShaderType.FragmentShader, fragPath);

            int program = GL.CreateProgram();
            GL.AttachShader(program, vertexShader);
            GL.AttachShader(program, fragmentShader);
            GL.LinkProgram(program);

            // Проверка на ошибки линковки
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(program);
                throw new Exception($"Ошибка линковки программы: {infoLog}");
            }

            // После линковки шейдеры можно удалить
            GL.DetachShader(program, vertexShader);
            GL.DetachShader(program, fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);

            return program;
        }

        private int CompileShader(ShaderType type, string path)
        {
            string source = File.ReadAllText(path);
            int shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Ошибка компиляции {type}: {infoLog}");
            }
            return shader;
        }

        public void Clear(Color4 color)
        {
            GL.ClearColor(color);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vertexArrayObject);
            GL.DeleteProgram(_shaderProgram);
        }
    }
}