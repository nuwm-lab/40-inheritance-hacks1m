using System;
using System.Globalization;

// ...existing code... (GeometricEquation, Line, Hyperplane have been simplified to avoid console logging in ctors/finalizers)

abstract class GeometricEquation : IDisposable
{
	// coeffs: [a0, a1, a2, ... aN]
	protected double[] _coeffs;
	protected bool _disposed = false;
	protected string Name { get; }

	protected GeometricEquation(int dimension, string name)
	{
		// dimension = number of coordinates (e.g., 2 for line, 4 for hyperplane)
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
				// free managed resources if any
			}
			_disposed = true;
		}
	}
}

class Line : GeometricEquation
{
	// equation: a1*x + a2*y + a0 = 0  (coeffs: a0, a1, a2)
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
		return Math.Abs(val) < 1e-6;
	}

	public override void PrintCoefficients()
	{
		Console.WriteLine($"Пряма виду: a1*x + a2*y + a0 = 0");
		base.PrintCoefficients();
	}
}

class Hyperplane : GeometricEquation
{
	// equation: a4*x4 + a3*x3 + a2*x2 + a1*x1 + a0 = 0
	// coeffs: a0, a1, a2, a3, a4
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
		return Math.Abs(sum) < 1e-6;
	}

	public override void PrintCoefficients()
	{
		Console.WriteLine($"Гіперплощина виду: a4*x4 + a3*x3 + a2*x2 + a1*x1 + a0 = 0");
		base.PrintCoefficients();
	}
}

static class Program
{
	// Reads a line with 'count' double values. Returns null if input ended (Ctrl+Z/Ctrl+D depending on system).
	static double[]? ReadDoublesFromConsole(int count, string prompt)
	{
		Console.WriteLine(prompt);
		while (true)
		{
			Console.Write("> ");
			string? line = Console.ReadLine();
			if (line == null) return null;
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
		while (true)
		{
			PrintHeader();
			Console.Write("> ");
			string? opt = Console.ReadLine();
			if (opt == null) break;
			if (opt == "0") break;
			try
			{
				if (opt == "1")
				{
					// Triangle: read three points
					Point.ReadThreePointsFromConsole(out Point p1, out Point p2, out Point p3);
					var tri = new Triangle(p1, p2, p3);
					tri.PrintVertices();
					double area = tri.Area();
					Console.WriteLine($"Площа трикутника = {area.ToString(CultureInfo.InvariantCulture)}");
				}
				else if (opt == "2")
				{
					// Quadrilateral: read four points
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
			catch (Exception ex)
			{
				Console.WriteLine($"Помилка: {ex.Message}");
			}
			Console.WriteLine();
		}
	}
}
