namespace ConsoleImplementationsExtensionMethods_001
{
	public class Order
	{
		public int Id { get; set; }
		public string CustomerName { get; set; }
		public decimal Amount { get; set; }

		public Order(int id, string customerName, decimal amount)
			=> (Id, CustomerName, Amount) = (id, customerName, amount);

		public override string ToString()
			=> $"{Id}. Товар: {CustomerName}. Цена: {Amount}";

		/// <summary>
		/// Читает данные о заказах из CSV-файла и возвращает их 
		/// как перечисление объектов <see cref="Order"/>.
		/// </summary>
		/// <returns>Перечисление заказов (<see cref="IEnumerable{Order}"/>) 
		/// прочитанных из файла.</returns>
		/// <exception cref="FileNotFoundException">Вызывается, 
		/// если указанный файл не найден.</exception>
		/// <exception cref="FormatException">Вызывается, если формат данных 
		/// в файле некорректен (например, невозможно преобразовать строку в число).</exception>
		/// <remarks>
		/// Метод читает файл CSV, где каждая строка содержит информацию о заказе в следующем формате:
		/// - Первый столбец: Идентификатор заказа (целое число).
		/// - Второй столбец: Описание заказа (строка).
		/// - Третий столбец: Стоимость заказа (десятичное число).
		/// Файл должен быть расположен по пути "1_9.csv" (в папке программы...).
		/// </remarks>
		public static IEnumerable<Order> GetOrder()
		{
			string[] lines = File.ReadAllLines(@"1_9.csv");
			foreach (string line in lines)
			{
				string[] splitLine = line.Split(";");

				var order = new Order(int.Parse(splitLine[0]),
								splitLine[1],
								decimal.Parse(splitLine[2]));
				yield return order;
			}
		}
	}
}
