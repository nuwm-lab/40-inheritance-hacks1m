using System;
using System.Globalization;

abstract class GeometricEquation : IDisposable
{
	// coeffs: [a0, a1, a2, ... aN]
	protected double[] coeffs;
	protected bool disposed = false;
	protected string name;

	protected GeometricEquation(int dimension, string name)
	{
		// dimension = number of coordinates (e.g., 2 for line, 4 for hyperplane)
		coeffs = new double[dimension + 1];
		this.name = name;
		Console.WriteLine($"Конструктор {name} викликано");
	}

	public virtual void SetCoefficients(params double[] values)
	{
		if (values == null || values.Length != coeffs.Length)
			throw new ArgumentException($"Потрібно передати {coeffs.Length} коефіцієнтів (a0..a{coeffs.Length - 1})");
		for (int i = 0; i < coeffs.Length; i++) coeffs[i] = values[i];
	}

	public virtual void PrintCoefficients()
	{
		Console.WriteLine($"Коефіцієнти для {name}:");
		for (int i = 0; i < coeffs.Length; i++)
			Console.WriteLine($"a{i} = {coeffs[i].ToString(CultureInfo.InvariantCulture)}");
	}

	public abstract bool ContainsPoint(params double[] coords);

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				// free managed resources if any
			}
			Console.WriteLine($"Dispose (деструктор) {name} викликано");
			disposed = true;
		}
	}

	~GeometricEquation()
	{
		Dispose(false);
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
		double val = coeffs[1] * x + coeffs[2] * y + coeffs[0];
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
		double sum = coeffs[0];
		for (int i = 0; i < 4; i++) sum += coeffs[i + 1] * coords[i];
		return Math.Abs(sum) < 1e-6;
	}

	public override void PrintCoefficients()
	{
		Console.WriteLine($"Гіперплощина виду: a4*x4 + a3*x3 + a2*x2 + a1*x1 + a0 = 0");
		base.PrintCoefficients();
	}
}

class Program
{
	static double[] ReadDoublesFromConsole(int count, string prompt)
	{
		Console.WriteLine(prompt);
		while (true)
		{
			Console.Write($"> ");
			string? line = Console.ReadLine();
			if (line == null) return new double[count];
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

	static void Main()
	{
		// Створюємо об'єкти і демонструємо виклик конструктора та деструктора (Dispose)
		using (var line = new Line(1.0, -2.0, 3.0)) // приклад коефіцієнтів
		using (var hyper = new Hyperplane(1.0, 0.5, -1.0, 2.0, -0.5)) // a0..a4
		{
			// Виведення коефіцієнтів
			line.PrintCoefficients();
			hyper.PrintCoefficients();

			// Перевірка точки для прямої
			var pt2 = ReadDoublesFromConsole(2, "Введіть координати точки для прямої (x y), розділені пробілом:");
			bool onLine = line.ContainsPoint(pt2);
			Console.WriteLine(onLine ? "Точка належить прямій." : "Точка не належить прямій.");

			// Перевірка точки для гіперплощини
			var pt4 = ReadDoublesFromConsole(4, "Введіть координати точки для гіперплощини (x1 x2 x3 x4), через пробіл:");
			bool onHyper = hyper.ContainsPoint(pt4);
			Console.WriteLine(onHyper ? "Точка належить гіперплощині." : "Точка не належить гіперплощині.");
		}

		// Затримка, щоб користувач побачив повідомлення в консолі
		Console.WriteLine("Натисніть Enter для виходу...");
		Console.ReadLine();
	}
}
