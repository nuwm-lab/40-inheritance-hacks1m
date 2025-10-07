using System;
using System.Globalization;
using System.Threading;
using LabWork.Geometry;

static class Program
{
	// Зчитує рядок з 'count' дійсними числами. Повертає null, якщо введення закінчилось.
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
				if (!double.TryParse(parts[i], System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out res[i]))
				{
					Console.WriteLine($"Неможливо розпізнати число: '{parts[i]}'. Використовуйте формат з крапкою як роздільник (наприклад 1.5).");
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
		// встановлення культури для коректного введення чисел з плаваючою комою (використовую крапку)
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