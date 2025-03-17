using AutoBogus;
using AutoBogus.Templating;
using Bogus;
using Newtonsoft.Json;
using System.Text.Json;

namespace Practice;

public class BogusProcessor
{
    private readonly string[] _fruit = ["apple", "banana", "orange", "strawberry", "kiwi"];

    public User GenerateData()
    {
        Randomizer.Seed = new Random(123);
        var userFaker = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Int(1, 1000))
        .RuleFor(u => u.Name, f => f.Name.FullName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.BirthDate, f => f.Date.Past(30, DateTime.Now.AddYears(-18)));
        var listUser = userFaker.Generate(10);
        var result = userFaker.Generate();
        var user1 = userFaker.Generate();
        var frenchFaker = new Faker("fr");
        var names = frenchFaker.Name.FullName();
        var fakeJson = userFaker.Generate(3);
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = System.Text.Json.JsonSerializer.Serialize(fakeJson, options);
        string json2 = JsonConvert.SerializeObject(user1, Newtonsoft.Json.Formatting.Indented);
        return result;
    }

    public void TestBogus()
    {
        Randomizer.Seed = new Random(8675309);
        var orderIds = 0;


        var testOrders = new Faker<Order>()
        //Ensure all properties have rules. By default, StrictMode is false
        //Set a global policy by using Faker.DefaultStrictMode
        .StrictMode(true)
        //OrderId is deterministic
        .RuleFor(o => o.OrderId, f => orderIds++)
        //Pick some fruit from a basket
        .RuleFor(o => o.Item, f => f.PickRandom(_fruit))
        //A random quantity from 1 to 10
        .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
        //A nullable int? with 80% probability of being null.
        //The .OrNull extension is in the Bogus.Extensions namespace.
        .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f));

        var ord = testOrders.Generate();
        var userIds = 0;
        var testUsers = new Faker<User>()
            //Optional: Call for objects that have complex initialization
            .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))
            .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .RuleFor(u => u.BirthDate, f => f.DateTimeReference.GetValueOrDefault())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
            .RuleFor(u => u.Name, (f, u) => u.FirstName + " " + u.LastName)
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.SomethingUnique, f => $"###_{f.UniqueIndex}_###")
            .RuleFor(u => u.Orders, f => testOrders.Generate(3).ToArray())
            .RuleFor(u => u.CartId, f => Guid.NewGuid())
            .FinishWith((f, u) =>
            {
                Console.WriteLine("User Created! Id={0}", u.Id);
            });
        var person = new Bogus.Person();
        var user = testUsers.Generate();

        var persons = new AutoFaker<Person>().GenerateWithTemplate(@"
            Id | FirstName | LastName
            0  | John      | Smith
            1  | Jane      | Jones
            2  | Bob       | Clark
        ");
        var lorem = new Bogus.DataSets.Lorem(locale: "es_MX");
        var text = lorem.Sentences(5);

        var random = new Bogus.Randomizer();
        var lorem1 = new Bogus.DataSets.Lorem("en");
        var o = new Order()
        {
            OrderId = random.Number(1, 100),
            Item = lorem1.Sentence(),
            Quantity = random.Number(1, 10)
        };

        var fakeAddress = new Faker<Address>()
            .RuleFor(a => a.ZipCode, a => a.Address.ZipCode())
            .RuleFor(a => a.City, a => a.Address.City())
            .RuleFor(a => a.CitySuffix, a => a.Address.CitySuffix())
            .RuleFor(a => a.StreetAddress, a => a.Address.StreetAddress(true))
            .RuleFor(a => a.StreetName, a => a.Address.StreetName())
            .RuleFor(a => a.StreetSuffix, a => a.Address.StreetSuffix())
            .RuleFor(a => a.CityPrefix, a => a.Address.CityPrefix())
            .RuleFor(a => a.BuildingNumber, a => a.Address.BuildingNumber())
            .RuleFor(a => a.SecondaryAddress, a => a.Address.SecondaryAddress())
            .RuleFor(a => a.Latitude, a => a.Address.Latitude())
            .RuleFor(a => a.Longitude, a => a.Address.Longitude())
            .RuleFor(a => a.StateAbbr, a => a.Address.StateAbbr())
            .RuleFor(a => a.CardinalDirection, a => a.Address.CardinalDirection())
            .RuleFor(a => a.OrdinalDirection, a => a.Address.OrdinalDirection())
            .RuleFor(a => a.FullAddress, a => a.Address.FullAddress())
            .RuleFor(a => a.CountryCode, a => a.Address.CountryCode())
            .RuleFor(a => a.County, a => a.Address.County())
            .RuleFor(a => a.Country, a => a.Address.Country())
            .RuleFor(a => a.State, a => a.Address.State())
            .RuleFor(a => a.Direction, a => a.Address.Direction());
        var addResult = fakeAddress.Generate();

        var fakeDataBase = new Faker<DataBase>()
             .RuleFor(a => a.Column, a => a.Database.Column())
             .RuleFor(a => a.Type, a => a.Database.Type())
             .RuleFor(a => a.Collation, a => a.Database.Collation())
             .RuleFor(a => a.Engine, a => a.Database.Engine());

        var dt = fakeDataBase.Generate();
    }
}

public enum Gender
{
    Male,
    Female
}

public class DataBase
{
    public string Column { get; set; }
    public string Collation { get; set; }
    public string Type { get; set; }
    public string Engine { get; set; }
}

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Status { get; set; } // Will be auto generated by the underlying AutoBogus generator
}

public class Address
{
    public string ZipCode { get; set; }
    public string CountryCode { get; set; }
    public string City { get; set; }
    public string CityPrefix { get; set; }
    public string FullAddress { get; set; }
    public string StreetAddress { get; set; }
    public string StreetName { get; set; }
    public string Country { get; set; }
    public string BuildingNumber { get; set; }
    public string County { get; set; }
    public string SecondaryAddress { get; set; }
    public string StreetSuffix { get; set; }
    public string CitySuffix { get; set; }
    public string StateAbbr { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public string CardinalDirection { get; set; }
    public string OrdinalDirection { get; set; }
    public string State { get; set; }
    public string Direction { get; set; }
}


public class Order
{
    public int OrderId { get; set; }
    public string Item { get; set; }
    public int Quantity { get; set; }
    public int? LotNumber { get; set; }
}

public class User
{
    public int Id { get; set; }
    public Guid CartId { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string SomethingUnique { get; set; }
    public string FirstName { get; set; }
    public string UserName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Order[] Orders { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }

    public User(int userId, string format)
    {
        Id = userId;

    }
}
