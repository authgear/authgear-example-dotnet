# Authentication for ASP.NET apps with Authgear and OpenID Connect

This repo provides a basic demo web application, created using [ASP.NET](http://ASP.NET), and demonstrates how to add authentication features with [Authgear](https://www.authgear.com/) by implementing an [OpenID Connect](https://docs.authgear.com/concepts/identity-fundamentals#open-id-connect) flow, then retrieving OAuth tokens, in order to call APIs.

## What is Authgear?

[Authgear](https://www.authgear.com/) acts as an IAM provider that is a **gatekeeper to the resources** you provide to customers as web and mobile applications, APIs, etc. The gatekeeper initiates authorization as outlined in [OAuth 2.0](https://www.notion.so/concepts/identity-fundamentals#oauth-2.0). The addition of the [OpenID Connect](https://www.notion.so/concepts/identity-fundamentals#open-id-connect) layer adds authentication to secure your users’ digital identities and your product.

## How to run the project

## Prerequisites

Before you get started, you will need the following:

- A **free Authgear account**. [Sign up](https://oursky.typeform.com/to/S5lvI8rN) if you don't have one already.
- [.NET 7](https://dotnet.microsoft.com/en-us/download) downloaded and installed on your machine. You can also use [Visual Studio](https://visualstudio.microsoft.com/) and [VS code](https://code.visualstudio.com/) to automatically detect the .NET version.
- Create [Authgear OIDC Client App](https://docs.authgear.com/how-to-guide/authenticate/oidc-provider#setting-up-authgear-in-the-portal) to use it as an OpenID Connect Provider.

Start by cloning the project into your local machine:

```bash
git clone https://github.com/authgear/authgear-example-dotnet.git
```

Make the project directory your current working directory:

```bash
cd authgear-example-dotnet
```

Update the following configuration variables in the `appsettings.json` file with your Authgear app settings values from **Authgear OIDC Client application** such as `Issuer`, `ClientId`, `ClientSecret`, and Authgear endpoint:

```bash
{
    "OpenIDConnect": {
        "ClientId": "{your-client-id}",
        "ClientSecret": "{your-client-secret}",
        "Issuer": "{your-authgear-app-endpoint}",
        "Scope": "openid",
        "PostLogoutRedirectUri": "http://localhost:5002",
        "TokenEndpoint": "{your-authgear-app-endpoint}/oauth2/token"
    },
    "Urls": "http://localhost:5002",
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    }
}
```

Execute the following command to run the [ASP.NET](http://asp.net/) Core web application:

```bash
dotnet build
dotnet run
```

You can now visit [](http://localhost:5002)[http://localhost:5002](http://localhost:5002/) to access the application. When you click on the **"View Protected Data"** button, ASP.NET Core takes you to the **Authgear’s Login page**.
