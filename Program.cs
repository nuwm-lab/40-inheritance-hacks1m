using System;
using System.Globalization;

// ----------------------------------------------------------------------------------------------------
// 1. Core Abstractions (GeometricEquation, Line, Hyperplane)
// ----------------------------------------------------------------------------------------------------

// Базовий абстрактний клас для геометричних рівнянь (Лінія, Гіперплощина тощо).
abstract class GeometricEquation : IDisposable
{
	// coeffs: [a0, a1, a2, ... aN]
	protected double[] _coeffs;
	protected bool _disposed = false;
	protected string Name { get; }

	protected GeometricEquation(int dimension, string name)
	{
		// dimension = кількість координат (наприклад, 2 для лінії (x, y), 4 для гіперплощини (x1..x4))
		// Коефіцієнтів завжди на 1 більше за розмірність (включаючи вільний член a0)
		_coeffs = new double[dimension + 1];
		Name = name;
	}

	public virtual void SetCoefficients(params double[] values)
	{
		if (values == null || values.Length != _coeffs.Length)
			throw new ArgumentException($"Потрібно передати {_coeffs.Length} коефіцієнтів (a0..a{_coeffs.Length - 1})");
		for (int i = 0; i < _coeffs.Length; i++) _coeffs[i] = values[i];
	}

	public virtual void PrintCoefficients()
	{
		Console.WriteLine($"Коефіцієнти для {Name}:");
		for (int i = 0; i < _coeffs.Length; i++)
			Console.WriteLine($"a{i} = {_coeffs[i].ToString(CultureInfo.InvariantCulture)}");
	}

	// Перевіряє, чи належить точка рівнянню (задовольняє рівняння)
	public abstract bool ContainsPoint(params double[] coords);

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				// звільнення керованих ресурсів, якщо вони є
			}
			_disposed = true;
		}
	}
}

// Представляє пряму на площині (2D)
class Line : GeometricEquation
{
	// рівняння: a1*x + a2*y + a0 = 0  (коефіцієнти: a0, a1, a2)
	public Line() : base(2, "Line") { }

	public Line(double a0, double a1, double a2) : base(2, "Line")
	{
		SetCoefficients(a0, a1, a2);
	}

	public override bool ContainsPoint(params double[] coords)
	{
		if (coords == null || coords.Length != 2) throw new ArgumentException("Потрібні координати x і y");
		double x = coords[0], y = coords[1];
		double val = _coeffs[1] * x + _coeffs[2] * y + _coeffs[0];
		// Порівняння з нулем з плаваючою комою
		return Math.Abs(val) < 1e-6;
	}

	public override void PrintCoefficients()
	{
		Console.WriteLine($"Пряма виду: a1*x + a2*y + a0 = 0");
		base.PrintCoefficients();
	}
}

// Представляє гіперплощину у 4-вимірному просторі
class Hyperplane : GeometricEquation
{
	// рівняння: a4*x4 + a3*x3 + a2*x2 + a1*x1 + a0 = 0
	// коефіцієнти: a0, a1, a2, a3, a4
	public Hyperplane() : base(4, "Hyperplane") { }

	public Hyperplane(params double[] values) : base(4, "Hyperplane")
	{
		SetCoefficients(values);
	}

	public override bool ContainsPoint(params double[] coords)
	{
		if (coords == null || coords.Length != 4) throw new ArgumentException("Потрібні 4 координати (x1..x4)");
		double sum = _coeffs[0];
		for (int i = 0; i < 4; i++) sum += _coeffs[i + 1] * coords[i];
		// Порівняння з нулем з плаваючою комою
		return Math.Abs(sum) < 1e-6;
	}

	public override void PrintCoefficients()
	{
		Console.WriteLine($"Гіперплощина виду: a4*x4 + a3*x3 + a2*x2 + a1*x1 + a0 = 0");
		base.PrintCoefficients();
	}
}

// ----------------------------------------------------------------------------------------------------
// 2. Geometric Shapes (Point, Triangle, ConvexQuadrilateral) - Доповнення
// ----------------------------------------------------------------------------------------------------

// Клас, що представляє точку на площині (2D)
class Point
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y) { X = x; Y = y; }

    public static double Distance(Point p1, Point p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }

    public static void ReadThreePointsFromConsole(out Point p1, out Point p2, out Point p3)
    {
        Console.WriteLine("Введіть координати першої точки (x, y):");
        double[]? coords1 = Program.ReadDoublesFromConsole(2, "");
        if (coords1 == null) throw new InvalidOperationException("Введення завершено.");
        p1 = new Point(coords1[0], coords1[1]);

        Console.WriteLine("Введіть координати другої точки (x, y):");
        double[]? coords2 = Program.ReadDoublesFromConsole(2, "");
        if (coords2 == null) throw new InvalidOperationException("Введення завершено.");
        p2 = new Point(coords2[0], coords2[1]);

        Console.WriteLine("Введіть координати третьої точки (x, y):");
        double[]? coords3 = Program.ReadDoublesFromConsole(2, "");
        if (coords3 == null) throw new InvalidOperationException("Введення завершено.");
        p3 = new Point(coords3[0], coords3[1]);
    }

    public static Point[] ReadNPointsFromConsole(int count)
    {
        Point[] points = new Point[count];
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine($"Введіть координати точки #{i + 1} (x, y):");
            double[]? coords = Program.ReadDoublesFromConsole(2, "");
            if (coords == null) throw new InvalidOperationException("Введення завершено.");
            points[i] = new Point(coords[0], coords[1]);
        }
        return points;
    }
}

// Клас, що представляє трикутник
class Triangle
{
    private readonly Point[] _vertices;

    public Triangle(Point p1, Point p2, Point p3)
    {
        _vertices = new Point[] { p1, p2, p3 };
        // Додаткова перевірка на виродженість (колінеарність) може бути додана тут.
    }

    public void PrintVertices()
    {
        Console.WriteLine($"Вершини трикутника: A({_vertices[0].X.ToString(CultureInfo.InvariantCulture)}, {_vertices[0].Y.ToString(CultureInfo.InvariantCulture)}), B({_vertices[1].X.ToString(CultureInfo.InvariantCulture)}, {_vertices[1].Y.ToString(CultureInfo.InvariantCulture)}), C({_vertices[2].X.ToString(CultureInfo.InvariantCulture)}, {_vertices[2].Y.ToString(CultureInfo.InvariantCulture)})");
    }

    // Обчислення площі за формулою шнурків (Shoelace formula)
    public double Area()
    {
        double x1 = _vertices[0].X, y1 = _vertices[0].Y;
        double x2 = _vertices[1].X, y2 = _vertices[1].Y;
        double x3 = _vertices[2].X, y3 = _vertices[2].Y;

        double area = 0.5 * Math.Abs(
            (x1 * y2 + x2 * y3 + x3 * y1) -
            (y1 * x2 + y2 * x3 + y3 * x1)
        );
        return area;
    }
}

// Клас, що представляє опуклий чотирикутник
class ConvexQuadrilateral
{
    private readonly Point[] _vertices; // Повинно бути 4 точки

    public ConvexQuadrilateral(Point[] points)
    {
        if (points == null || points.Length != 4)
            throw new ArgumentException("Опуклий чотирикутник вимагає рівно 4 вершини.");
        // Тут повинна бути реальна перевірка на опуклість (convexity).
        _vertices = points;
    }

    public void PrintVertices()
    {
        Console.WriteLine("Вершини чотирикутника:");
        for (int i = 0; i < _vertices.Length; i++)
        {
            Console.WriteLine($"P{i + 1}({_vertices[i].X.ToString(CultureInfo.InvariantCulture)}, {_vertices[i].Y.ToString(CultureInfo.InvariantCulture)})");
        }
    }

    // Обчислення площі за формулою шнурків (Shoelace formula) для чотирьох точок
    public double Area()
    {
        double sum1 = 0;
        double sum2 = 0;
        int n = _vertices.Length;

        for (int i = 0; i < n; i++)
        {
            Point p1 = _vertices[i];
            Point p2 = _vertices[(i + 1) % n]; // Перехід до наступної точки, включаючи замикання (P4 до P1)
            sum1 += p1.X * p2.Y;
            sum2 += p1.Y * p2.X;
        }

        return 0.5 * Math.Abs(sum1 - sum2);
    }
}


// ----------------------------------------------------------------------------------------------------
// 3. Program Entry Point
// ----------------------------------------------------------------------------------------------------

static class Program
{
	// Зчитує рядок з 'count' дійсними числами. Повертає null, якщо введення закінчилося.
	public static double[]? ReadDoublesFromConsole(int count, string prompt)
	{
        if (!string.IsNullOrEmpty(prompt))
		    Console.WriteLine(prompt);
		while (true)
		{
			Console.Write("> ");
			string? line = Console.ReadLine();
			if (line == null) return null;
			// Роздільники: пробіл, табуляція, кома
			string[] parts = line.Split(new[] { ' ', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length != count)
			{
				Console.WriteLine($"Потрібно ввести {count} чисел, повторіть введення.");
				continue;
			}
			double[] res = new double[count];
			bool ok = true;
			for (int i = 0; i < count; i++)
			{
				// Використання InvariantCulture для коректного парсингу чисел з плаваючою комою (крапка як роздільник)
				if (!double.TryParse(parts[i], NumberStyles.Float, CultureInfo.InvariantCulture, out res[i]))
				{
					Console.WriteLine($"Неможливо розпізнати число: '{parts[i]}'. Використовуйте формат з крапкою як роздільником (наприклад 1.5).");
					ok = false; break;
				}
			}
			if (ok) return res;
		}
	}

	static void PrintHeader()
	{
		Console.WriteLine("Виберіть фігуру:");
		Console.WriteLine("1 - Triangle");
		Console.WriteLine("2 - ConvexQuadrilateral");
		Console.WriteLine("0 - Вихід");
	}

	static void Main()
	{
		// Встановлення культури для коректного виведення чисел з плаваючою комою (використовуючи крапку)
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

		while (true)
		{
			PrintHeader();
			Console.Write("> ");
			string? opt = Console.ReadLine();
			if (opt == null || opt == "0") break;
			try
			{
				if (opt == "1")
				{
					// Трикутник: зчитуємо три точки
					Point.ReadThreePointsFromConsole(out Point p1, out Point p2, out Point p3);
					var tri = new Triangle(p1, p2, p3);
					tri.PrintVertices();
					double area = tri.Area();
					Console.WriteLine($"Площа трикутника = {area.ToString(CultureInfo.InvariantCulture)}");
				}
				else if (opt == "2")
				{
					// Чотирикутник: зчитуємо чотири точки
					Point[] pts = Point.ReadNPointsFromConsole(4);
					var quad = new ConvexQuadrilateral(pts);
					quad.PrintVertices();
					double area = quad.Area();
					Console.WriteLine($"Площа опуклого чотирикутника = {area.ToString(CultureInfo.InvariantCulture)}");
				}
				else
				{
					Console.WriteLine("Невідома опція, спробуйте ще раз.");
				}
			}
			catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка введення даних: {ex.Message}");
            }
			catch (Exception ex)
			{
				Console.WriteLine($"Непередбачена помилка: {ex.Message}");
			}
			Console.WriteLine();
		}
	}
}