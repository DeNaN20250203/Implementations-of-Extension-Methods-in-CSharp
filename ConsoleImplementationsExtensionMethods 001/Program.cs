namespace ConsoleImplementationsExtensionMethods_001
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var res = Order.GetOrder();
			Console.WriteLine(String.Join(Environment.NewLine, res.Take(10)));
			Console.WriteLine(String.Join(Environment.NewLine, res.Take(10).GetDistinct()));

			Console.ReadKey();
		}
	}
}
