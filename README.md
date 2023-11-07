# Instalação

```sh
dotnet new webapi -n backend002
cd backend002

# Listar pacotes
dotnet list package

# instalar pacotes pelo XML
dotnet restore

# Adicionar pacotes
dotnet add package nome

# Remover pacotes
dotnet remove package nome

# Limpar
dotnet clean

# Executar
dotnet run
dotnet run --urls "https://localhost:5000"
dotnet watch run

# Criar o executável
dotnet build

dotnet --version
dotnet --list-sdks
dotnet --list-runtimes
```

.prettierrc.json

```json
{
  "trailingComma": "none",
  "tabWidth": 4,
  "semi": true,
  "singleQuote": true,
  "useTabs": true
}
```

omnisharp.json

```json
{
  "FormattingOptions": {
    "newLine": "\n",
    "useTabs": true,
    "tabSize": 4,
    "indentationSize": 4
  }
}
```

backend002.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
		<PackageReference Include="Microsoft.EntityFrameworkCore" Version="7.0.13" />
		<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="7.0.13">
			<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
			<PrivateAssets>all</PrivateAssets>
		</PackageReference>
		<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.13" />
		<PackageReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.3" />
		<PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="6.5.0" />
		<PackageReference Include="System.Linq.Dynamic.Core" Version="1.3.5" />
  </ItemGroup>

</Project>
```

Program.cs

```c#
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

Properties/launchSettings.json

```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:61787",
      "sslPort": 44359
    }
  },
  "profiles": {
    "backend002": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "_applicationUrl": "https://localhost:7016;http://localhost:5273",
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

# VS Code - settings.json

```json
{
  "editor.defaultFormatter": "esbenp.prettier-vscode",
  "[csharp]": {
    "editor.formatOnSave": true,
    "editor.defaultFormatter": "ms-dotnettools.csharp"
  },
  "[c#]": {
    "editor.formatOnSave": true,
    "editor.defaultFormatter": "ms-dotnettools.csharp"
  },
  "[*.cs]": {
    "editor.formatOnSave": true,
    "editor.defaultFormatter": "ms-dotnettools.csharp"
  },
  "csharp.format.enable": true
}
```
