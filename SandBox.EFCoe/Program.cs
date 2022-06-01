using Microsoft.EntityFrameworkCore;
using SandBox.EFCoe;



using (var ctx = new AppDbContext())
{
    var orange = new Fruit { Name = "Orange" };
    var apple = new Fruit { Name = "Apple" };

    ctx.Fruits.Add(orange);
    var orangeId = ctx.Entry(orange).Property<int>("Id").CurrentValue;
    ctx.Fruits.Add(apple);

    var address = new Address { PostCode = "Moon" };
    //ctx.Addresses.Add(address);
    orange.Address = address;
    ctx.SaveChanges(); 
}

using (var ctx = new AppDbContext())
{
    var fruits = ctx.Fruits.Select(x => new FruitVm
    {
        Id = EF.Property<int>(x, "Id"),
        Name = x.Name,
        Weight = x.Weight,
        PostCode=x.Address.PostCode
    }).ToList();


    var fullFruits = ctx.Fruits.ToList();
    var addresses = ctx.Addresses.ToList(); 
}
Console.ReadKey();