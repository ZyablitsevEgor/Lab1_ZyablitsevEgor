using System;
using System.IO;
using Serilog;

public class TriangleCalculator
{
    private const int FieldSize = 100;

    public static void CalculateAndLogTriangle(string sideA, string sideB, string sideC)
    {
        Log.Information($"Запрос: сторона А = {sideA}, сторона Б = {sideB}, сторона С = {sideC}");
        if (!float.TryParse(sideA, out float a) || !float.TryParse(sideB, out float b) || !float.TryParse(sideC, out float c))
        {
            Log.Error("Невалидные входные данные");
            return;
        }

        var triangleType = "";
        var vertices = new[] { (-1, -1), (-1, -1), (-1, -1) };

        if (a <= 0 || b <= 0 || c <= 0)
        {
            triangleType = "не треугольник";
        }
        else if (a == b && b == c)
        {
            triangleType = "равносторонний";
            vertices = GetTriangleVertices(0, 0, a, b, c);
        }
        else if (a == b || b == c || a == c)
        {
            triangleType = "равнобедренный";
            vertices = GetTriangleVertices(0, 0, a, b, c);
        }
        else
        {
            triangleType = "разносторонний";
            vertices = GetTriangleVertices(0, 0, a, b, c);
        }

        Log.Information($"Тип треугольника: {triangleType}");
        Log.Information($"Координаты вершин: A({vertices[0].Item1}, {vertices[0].Item2}), " +
                        $"B({vertices[1].Item1}, {vertices[1].Item2}), " +
                        $"C({vertices[2].Item1}, {vertices[2].Item2})");
    }

    private static (int, int)[] GetTriangleVertices(int xA, int yA, float a, float b, float c)
    {
        var xB = (int)(FieldSize * a / (a + b));
        var yB = 0;
        var xC = (int)(FieldSize * ((b * b + c * c - a * a) / (2 * b * c)));
        var yC = (int)(FieldSize * Math.Sqrt(b * b - xC * xC / (FieldSize * FieldSize)));
        if (yC < 0)
        {
            yC = 0;
        }
        else if (yC >= FieldSize)
        {
            yC = FieldSize - 1;
        }
        return new[] { (xA, yA), (xB, yB), (xC, yC) };
    }

}
public class Program
{
    public static void Main(string[] args)
    {
        string template = "{Timestamp:HH:mm:ss}|[{Level:u3}]|{Message:lj}{NewLine}{Exception}";
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: template)
            .WriteTo.File("C:/Logs/log.txt", outputTemplate: template)
            .CreateLogger();

        Log.Verbose("Логгер сконфигурирован!");
        Log.Information("Приложение запущено!");

        string sideA = "3";
        string sideB = "4";
        string sideC = "3";

        TriangleCalculator.CalculateAndLogTriangle(sideA, sideB, sideC);


        Log.CloseAndFlush();
    }
}
