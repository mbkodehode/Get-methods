var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var random = new Random();

app.MapGet("/", () => "Hello World!");

string[] people = new string[] { "Devrim", "Martin", "Cosmo", "Milo", "Mia", "Mina" };


//POST, PUT ,DELETE 

//this is text return 
app.MapGet("/gettext", () =>
{

    string myName = "Devrim";

    return Results.Text($"my name is {myName}");

});

//create a json return 

app.MapGet("/getjson", () =>
{

    string myName = "devrim";
    int myAge = 48;
    string myCity = "Drammen";

    return Results.Json(new { Name = myName, Age = myAge, City = myCity });


});






app.MapGet("/getjason", () => 
{
    string myName = "Martin";
    int myAge = 25;
    string myCity = "Oslo";
    return Results.Json(new 
    {
        name = myName,
        age = myAge,
        city = myCity
    });
});

app.MapGet("/mypet", () => 
{
    string myPet = "Cosmo";
    int petAge = 5;
    string petspecies = "Cat";

    return Results.Json(new 
    {
        petName = myPet,
        petAge = petAge,
        petSpecies = petspecies
    });
}); 
app.MapGet("/people", () => 
{
    return Results.Json(people);
});
// create an enpoint that returns the second person from the array
app.MapGet("/people/second", () =>
{
    string personName = people[1];
    return Results.Json(personName);
});
app.MapGet("/getselectedperson/{indexNumber}", (int indexNumber) =>
{
    // we want to use int.tryParse to check if the indexNumber is a valid number
    int indexSize = people.Length;
    if (indexNumber >= indexSize || indexNumber < 0)
    {
        return Results.BadRequest("maximun index number is " + (indexSize - 1));
    }
string personName = people[indexNumber];
return Results.Ok($"you have chosen the person {personName}");
});
//now we need to elinate the error if user enters a string instead of a number
app.MapGet("/getspersonerror/{indexNumberInput}", (string indexNumberInput) =>
{
    // try to parse the string to an integer
    if (!int.TryParse(indexNumberInput, out int indexNumber))
    {
        return Results.BadRequest("please enter a valid number");
    }
    // we get index size
    int indexSize = people.Length;
    // check if the index number is valid
    if (indexSize < 0 || indexNumber >= indexSize)
    {
        return Results.BadRequest("maximun index number is " + (indexSize - 1));
    }
    string selectedperson = people[indexNumber];
    return Results.Ok($"you have selected the person {selectedperson}");
});
app.MapGet("/errorpage", () =>
{
    string error = "this page does not exist";
    return Results.BadRequest(error);
});

app.MapGet("/randomperson" , () =>
{
    //get a random person from the array
    int randomIndex = random.Next(people.Length);
    string randomperson = people[randomIndex];
    return Results.Ok(randomperson);

});

// search for a person in the array
app.MapGet("/findperson/{name}", (string personName) =>
{
    //check if the person is in the array
    if (!people.Contains(personName))
    {
        //return a bad request if the person is not in the array
        return Results.BadRequest($"person {personName} is not in the array");
    }
    return Results.Ok($"person {personName} is in the array");
}

    
);

// create a calculator parameter will be operation, number1 and number2
// calculator operator/number1/number2
app.MapGet("/calculator/{operationType}/{number1}/{number2}", (string operationType, double number1, double number2) =>
{
    // lets use switch to check the operation type
    double result = 0;
    switch (operationType)
    {
        case "add": // add the two numbers
            result = number1 + number2;
            break;
        
        case "subtract": // subtract the two numbers
            result = number1 - number2;
            break;
        
        case "multiply": // multiply the two numbers
            result = number1 * number2;
            break;
        
        case "divide": // divide the two numbers
            if (number2 == 0) // check if the second number is 0
            {
                return Results.BadRequest("cannot divide by 0");
            }
            result = number1 / number2;
            break;
        default: // return a bad request if the operation type is not valid
            return Results.BadRequest("operation type is not valid");
    }
    return Results.Ok($"the result of {number1} {operationType} {number2} is {result}"); // return the result);
});    
    
app.Run();