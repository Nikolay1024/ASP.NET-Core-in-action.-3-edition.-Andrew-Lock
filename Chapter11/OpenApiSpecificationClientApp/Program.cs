using OpenApiSpecificationClientApp;

IFruitClient client = new FruitClient(new HttpClient() { BaseAddress = new Uri("https://localhost:7266")});

var fruits = new List<Fruit>();
fruits.Add(await client.PostFruit1Async("7", new Fruit() { Name = "Apple", Stock = 10 }));
fruits.Add(await client.PostFruit2Async("8", new Fruit() { Name = "Banana", Stock = 20 }));
fruits.Add(await client.PostFruit2Async("9", new Fruit() { Name = "Orange", Stock = 30 }));
Console.WriteLine($"Создано:{Environment.NewLine}{string.Join(Environment.NewLine, fruits)}");

fruits.Clear();
fruits.Add(await client.GetFruit1Async("7"));
fruits.Add(await client.GetFruit2Async("8"));
fruits.Add(await client.GetFruit3Async("9"));
Console.WriteLine($"Получено:{Environment.NewLine}{string.Join(Environment.NewLine, fruits)}");

Console.ReadKey();


namespace OpenApiSpecificationClientApp
{
    public partial class Fruit
    {
        public override string ToString() => $"Name: {Name}, Stock: {Stock}.";
    }
}
