namespace ConsoleImplementationsExtensionMethods_001
{
	public static class Extensions
	{
		/// <summary>
		/// <para>Extension...</para>
		/// <para>Возвращает элементы коллекции в обратном порядке.</para>
		/// </summary>
		/// <param name="orders">Исходная коллекция заказов.</param>
		/// <returns>Перечисление заказов в обратном порядке относительно исходной коллекции.</returns>
		/// <exception cref="ArgumentNullException">Вызывается, 
		/// если входная коллекция <paramref name="orders"/> равна null.</exception>
		public static IEnumerable<Order> GetDistinct(this IList<Order> orders)
		{
			if (orders == null) throw new ArgumentNullException(nameof(orders));

			for (int i = orders.Count() - 1; i >= 0; i--)
			{
				yield return orders[i];
			}
		}

		/// <summary>
		/// <para>Extension...</para>
		/// <para>Возвращает элементы коллекции в обратном порядке.</para>
		/// </summary>
		/// <param name="orders">Исходная коллекция заказов.</param>
		/// <returns>Перечисление заказов в обратном порядке относительно исходной коллекции.</returns>
		/// <exception cref="ArgumentNullException">Вызывается, 
		/// если входная коллекция <paramref name="orders"/> равна null.</exception>
		/// <remarks>
		/// Метод использует стек (Stack) для сбора элементов коллекции 
		/// и их последующего извлечения в обратном порядке.
		/// Подходит для любых реализаций <see cref="IEnumerable{T}"/>, 
		/// так как не требует преобразования в список.
		/// </remarks>
		public static IEnumerable<Order> GetDistinct(this IEnumerable<Order> orders)
		{
			if (orders == null) throw new ArgumentNullException(nameof(orders));

			var stack = new Stack<Order>();

			foreach (var order in orders)
			{
				stack.Push(order);
			}

			while (stack.Count > 0)
			{
				yield return stack.Pop();
			}
		}
	}
}
