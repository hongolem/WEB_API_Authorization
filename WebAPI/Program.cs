using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => { 
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "authority_server",
        ValidAudience = "authority_client",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecretkeywhichshouldbenevershared")),
        ValidateLifetime = true,
        ClockSkew = System.TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = async context =>
        {
            Console.WriteLine("Received Message: " + context.Result);
            Console.WriteLine("Received Token: " + context.Token);
            /*
            var tokenIntrospection = new IntrospectionClient("https://localhost:1111/introspection");
            var response = tokenIntrospection.ValidateToken(context.Token);
            if (!response.active)
            {
                context.Fail("Token is not active");
            }
            */
        },
        OnTokenValidated = async context =>
        {
            Console.WriteLine("Validating Token: " + context.SecurityToken);
            // context.Principal.AddIdentity(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Role, "Admin") }));
            /*
            if (!tokenIsActive)
            {
                context.Fail("Token is not active");
            }
            */
        }
    };
});
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("MyBearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Id = "MyBearer",
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                }
            },
            new string[] {}
        }
    }); 
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
