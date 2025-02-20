<a id="anchor"></a>
# Изучение Расширения: Анализ Реализации Методов Расширения в C#

<a href="https://github.com/DeNaN20250203/Implementations-of-Extension-Methods-in-CSharp" target="_blank"><img src="Cover20250220_003.jpg" alt="Image" width="400" /></a>

В данной статье мы рассмотрим концепцию методов расширения (`extension methods`) в языке программирования C#, используя конкретный пример реализации двух методов расширения для коллекций заказов (`IEnumerable<Order>`). Мы проанализируем их назначение, различия в подходах к реализации и преимущества использования методов расширения.

## Что такое методы расширения?
Методы расширения — это особый тип статических методов, которые позволяют "добавлять" новые функции к существующим типам без изменения их исходного кода. Они особенно полезны, когда вы хотите расширить функциональность класса или интерфейса, к которому у вас нет доступа (например, сторонних библиотек).</br>

Синтаксис создания метода расширения требует:</br>
- Создания статического класса.</br>
- Определения статического метода с первым параметром, который указывает тип, к которому применяется метод. Этот параметр должен быть помечен ключевым словом `this`.</br>
Пример:

```csharp
public static class Extensions
{
    public static void MyExtensionMethod(this MyClass obj)
    {
        // Реализация метода
    }
}
```

## Анализ примера: Два метода расширения для коллекции заказов
### 1. Первый метод расширения: `GetDistinct` для `IList<Order>`

```csharp
public static IEnumerable<Order> GetDistinct(this IList<Order> orders)
{
    if (orders == null) throw new ArgumentNullException(nameof(orders));
    for (int i = orders.Count() - 1; i >= 0; i--)
    {
        yield return orders[i];
    }
}
```

**Назначение**: Этот метод принимает коллекцию заказов типа `IList<Order>` и возвращает элементы этой коллекции в обратном порядке.</br>
**Реализация**:</br>
- Используется цикл `for`, который начинает перебор с последнего элемента коллекции (`orders.Count() - 1`) и заканчивает на первом.</br>
- Ключевое слово `yield return` используется для поэлементной передачи результатов вызывающему коду.</br>
**Ограничения**:</br>
- Метод работает только с коллекциями, реализующими интерфейс `IList<Order>`. Это ограничивает его применимость, так как не все коллекции поддерживают этот интерфейс.</br>
- Требуется поддержка индексаторов (`[]`), что делает метод менее универсальным.</br>

### 2. Второй метод расширения: `GetDistinct` для `IEnumerable<Order>`

```csharp
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
```

**Назначение**: Этот метод также возвращает элементы коллекции заказов в обратном порядке, но работает с более общим типом `IEnumerable<Order>`.</br>
**Реализация**:</br>
- Используется структура данных `Stack<Order>` для временного хранения элементов коллекции.</br>
- Элементы добавляются в стек через метод `Push`, а затем извлекаются через метод `Pop`, что обеспечивает обратный порядок.</br>
**Преимущества**:</br>
- Метод поддерживает любой тип коллекции, реализующий интерфейс `IEnumerable<Order>`, что делает его более универсальным.</br>
- Не требует наличия индексаторов, что расширяет круг возможных применений.

### Сравнение двух реализаций

| Критерий | Метод для IList<Order> | Метод для IEnumerable<Order> |
|---------:|:-----------------------:|:------------------------------:|
| Область применения | Только коллекции, реализующие IList<Order> | Любые коллекции, реализующие IEnumerable<Order> |
| Универсальность | Низкая | Высокая |
| Производительность | Более быстрая, так как использует индексацию | Может быть медленнее из-за использования стека |
| Простота реализации | Простая | Сlightly сложнее из-за использования стека |

### Преимущества использования методов расширения
1. **Удобство использования**: Методы расширения позволяют писать более читаемый и понятный код. Например, вместо вызова `SomeClass.SomeStaticMethod(collection)` можно использовать `collection.SomeMethod()`.</br>
2. **Не изменяет исходный код**: Методы расширения не требуют модификации классов, к которым они применяются. Это особенно важно при работе с библиотечными или сторонними типами.</br>
3. **Гибкость**: Можно легко добавить новую функциональность к уже существующим типам, не нарушая принципы инкапсуляции.

### Практическое применение
В данном примере методы расширения используются для обработки коллекции заказов, прочитанных из CSV-файла. Файл содержит данные о заказах в следующем формате:</br>

```
ID;Наименование товара;Цена
1;Товар 1;100.50
2;Товар 2;200.75
/.../
```
Код чтения файла и создания коллекции заказов:</br>

```csharp
public static IEnumerable<Order> GetOrder()
{
    string[] lines = File.ReadAllLines(@"C:\Work\Visual Studio 2013\Work\ЕГЭ 2025 Информатика 1 Вариант\1_9.csv");
    foreach (string line in lines)
    {
        string[] splitLine = line.Split(";");
        var order = new Order(int.Parse(splitLine[0]), splitLine[1], decimal.Parse(splitLine[2]));
        yield return order;
    }
}
```

Затем методы расширения применяются для вывода первых 10 заказов и их уникальных значений в обратном порядке:</br>

```csharp
var res = Order.GetOrder();
Console.WriteLine(String.Join(Environment.NewLine, res.Take(10)));
Console.WriteLine(String.Join(Environment.NewLine, res.Take(10).GetDistinct()));
```
## Сложность кода
### Шаги анализа:
#### 1. Проверка на `null`:
- Проверка на `null` выполняется за `O(1)`, так как это простая операция сравнения.
	
#### 2. Перебор коллекции (`foreach`):
- В этом цикле каждый элемент из входной коллекции `orders` добавляется в стек с помощью метода `Push`.</br>
- Если в коллекции `n` элементов, то этот цикл выполнится `n раз`.
Сложность этой части: `O(n)`.
	
#### 3. Добавление элементов в стек (`Push`):
- Операция `Push` для стека выполняется за `O(1)` для каждого элемента.</br>
- Поскольку мы выполняем Push для всех n элементов, общая сложность для этого шага также составляет `O(n)`.
	
#### 4. Извлечение элементов из стека (`while + Pop`):
- В этом цикле мы извлекаем все элементы из стека с помощью метода `Pop` и возвращаем их через `yield return`.</br>
- Если в стеке было `n` элементов, то этот цикл также выполнится `n раз`.</br>
- Операция Pop для стека выполняется за `O(1)` для каждого элемента.</br>
- Общая сложность этого шага: `O(n)`.

### Итоговая сложность:
Первый цикл (`foreach`) имеет сложность `O(n)`.</br>
Второй цикл (`while`) также имеет сложность `O(n)`.</br>
Таким образом, общая временная сложность метода равна сумме этих двух частей:</br>
`O(n) + O(n) = O(n)`.

### Пространственная сложность:
- Мы используем дополнительный стек для хранения элементов коллекции.</br>
- Если в коллекции `n` элементов, то размер стека также будет `n`.</br>
- Следовательно, пространственная сложность метода равна `O(n)`.

### Итог:
- Временная сложность: `O(n)`</br>
- Пространственная сложность: `O(n)`</br>

### Заключение
> **Методы расширения** являются мощным инструментом в `C#`, позволяющим расширять функциональность существующих типов без изменения их исходного кода. В данном примере мы рассмотрели две реализации метода `GetDistinct` для коллекций заказов, каждая из которых имеет свои преимущества и недостатки. Первый метод более производителен, но менее универсален, второй — наоборот. Выбор между ними зависит от конкретных требований задачи.</br>
> Использование методов расширения способствует написанию более гибкого, читаемого и поддерживаемого кода, что особенно важно в крупных проектах.</br>

<a href="https://github.com/DeNaN20250203" target="_blank"><img src="GitHubDeJra.png" alt="Image" width="300" /></a>
[Верх](#anchor)
