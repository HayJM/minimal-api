var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost ("/login",(minimal_api.Dominio.DTOs.LoginDTD loginDTD) =>{
    if (loginDTD.Email == "adm@teste.com" && loginDTD.Senha == "123456")
    {
        return Results.Ok("Login com successo");
    }
    else
    {
        return Results.Unauthorized();
    }
});

app.Run();

