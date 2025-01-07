//we need to add a new namespace
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//we neew permission to use Addcors
builder.Services.AddCors(options=>
{
    options.AddPolicy("allowAll", policy=>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();


//we need to use the cors policy we created in the services for it to work
app.UseCors("allowAll");
// i want to create a list that contains the person instances
List<Person> people = new List<Person>()
{
    new Person(1, "Martin", 33 , "oslo"),
    new Person(2, "Devrim", 48, "drammen"),
    new Person(3, "Cosmo", 5, "Oslo"),
    new Person(4, "Milo", 3, "Oslo"),
    new Person(5, "Mia", 32, "Oslo"),
    new Person(6, "Mina", 30, "Oslo"),
};


app.MapGet("/", () => "Hello World!");


// get all people
app.MapGet("/people", () =>
{
    return Results.Ok(people);
});

//add a new person with post
app.MapPost("/addperson", ([FromForm] string name, [FromForm] int age, [FromForm] string city) =>
{
    if (string.IsNullOrEmpty(name) || age == 0)
    {
        return Results.BadRequest("Name and age are both required");
    }
    // tenery operator = short if else statement    this is the same as if(people.Any()) {people.Max(p=>p.Id)+1} else {1} or
    int lastIndex=people.Any() ? people.Max(p=>p.Id)+1 : 1;

    Person newPerson = new Person(lastIndex, name, age, city);
    people.Add(newPerson);
    return Results.Ok($"new person with the name {newPerson.Name} has been added");
}).DisableAntiforgery();

//find person
app.MapPost("/findperson", ([FromForm] int Id)=>
{
    var doesPersonExist = people.FirstOrDefault(p=>p.Id==Id); //FirstOrDefault returns the first element that satisfies the condition or a default value if no such element is found
    if (doesPersonExist==null)
    {
        //if the person is not found we return not found
        return Results.NotFound(new {message = $"person with the id {Id} does not exist"});// we can also use Results.NotFound("person with the id {Id} does not exist")
    }
    //if the person is found we return the person
    return Results.Json(new {Id = doesPersonExist.Id, Name = doesPersonExist.Name, Age = doesPersonExist.Age, City = doesPersonExist.City});
}).DisableAntiforgery();
// uppdate person
app.MapPut("/updateperson", ([FromForm] int Id, [FromForm] string updateName, [FromForm] int updateAge, [FromForm] string updateCity)=>
{
    var doesPersonExist = people.FirstOrDefault(p=>p.Id==Id);
    if (doesPersonExist==null)
    {
        return Results.NotFound(new {message = $"person with the id {Id} does not exist"});
    }
    //update the person by creating a new person with the same id and the new name and age since records are immutable
    var updateperson = doesPersonExist with {Name = updateName, Age = updateAge, City = updateCity};
    //find the index of the person in the list
    var index = people.FindIndex(p=>p.Id==Id);
    //replace the person in the list with the updated person
    people[index] = updateperson;
    return Results.Ok($"person with the id {Id} has been updated");
}).DisableAntiforgery();


// delete person
app.MapDelete("/deleteperson", ([FromForm] int Id) =>
{
    var doesPersonExist = people.FirstOrDefault(p=>p.Id==Id);
    //check if the person exists
    if (doesPersonExist==null)
    {
        return Results.NotFound(new {message = $"person with the id {Id} does not exist"});
    }
    //remove the person from the list
    people.Remove(doesPersonExist);
    return Results.Ok(new {message = $"person with the name {doesPersonExist.Name} has been removed"});
}).DisableAntiforgery();



app.Run();

record Person(int Id, string Name, int Age, string City);