[![Build status](https://ci.appveyor.com/api/projects/status/x481radl4tt7o2nk?svg=true)](https://ci.appveyor.com/project/MetalMichael/evotoapi)

# EvotoApi

The repository is home to the two APIs responsible for running evoto, as well as the Management website. All APIs are written in C# using ASP.net framework v4.5. The Management website is written in [React](https://facebook.github.io/react), and utilises [npm](https://www.npmjs.com/). Additonally, [MailGun](https://www.mailgun.com/) is used for sending emails from the service.

## Registrar API

This API is responsible for all comunication with [evoto Clients](https://github.com/evoto-tech/EvotoClientNet). This is accomplished securely through OAuth2 and HTTPS protocols. Additionally, the Registrar API (aka Regi) is responsible for the management of server [MultiChain](http://www.multichain.com) processes.

## Management API

The Management API (aka Mana) is responsible for creating and drafting votes before they are published to Regi. Additionally, there are features to manage the application, such as disabling signups, confirming emails, or manually creating accounts.

## Management Website

The Manament Website (which doesn't have a catchy nickname) is the administrators' interface for the Management API. The Management API and Website are often considdered to be the same thing, however the Management Website is to Mana as the [Desktop Client](https://github.com/evoto-tech/EvotoClientNet) is to Regi. The two communicate over HTTPS, however use simpler cookies and local authentication. 

# Build

The first step in building, as with many projects, is to clone this repository. Additionally, two submodules are required, detailed below. These can be cloned recursively.

    git clone https://github.com/evoto-tech/EvotoApi.git
    cd EvotoApi
    git submodule update --init --recursive
    
The following files must be created on the filesystem to build the project. These are omitted from the repository for security reasons.

`Registrar.Api/ApiKey.config` and `Management.Api/ApiKey.config`
These are the secure settings files. There are 2 expected settings; the first is the secure key used for communicating between Regi and Mana; the other key is an ApiKey for [MailGun](https://www.mailgun.com). Whilst only Regi needs the MailGunKey, it's easiest just to copy this file for both. Here's a template:

    <appSettings>
      <add key="ApiKeys" value="longsecretkey" />
      <add key="MailGunKey" value="key-ask-mailgun-for-this" />
    </appSettings>

`Registrar.Api/User.config` and `Registrar.Api/User.config`
The next files you're going to need contain the all important connection strings. Again, only one string is needed by each site, but it's easier just to have a file for both and copy it over. Template time:

    <connectionStrings>
      <add name="ManagementConnectionString" connectionString="PASTE_CONNECTION_STRING_FOR_MANA" providerName="System.Data.SqlClient" />
      <add name="RegistrarConnectionString" connectionString="PASTE_CONNECTION_STRING_FOR_REGI" providerName="System.Data.SqlClient" />
    </connectionStrings>

Great. Now to create our databases, we can run our simple migration script .bat files found in `Management.Database` and `Registrar.Database`. First make sure that nuget packages have been downloaded, either through Visual Studio, or depending on your userpath `nuget restore`. This will construct all of the required tables and create any initial required data. Checkout [FluentMigrator](https://github.com/fluentmigrator/fluentmigrator) if you want to find out more.

Finally, we must install our npm packages to build the web front-end. By default, Visual Studio is setup to build the JavaScript project before it builds the solution, however installing all of the modules each time in addition is time consuming and unecessary.

`cd Management.Api/Web && npm install`

Now build the solution in Visual Studio, and you're ready to go!

# Submodules
The application relies on two submodules, both also written by [evoto-tech](https://github.com/evoto-tech):

## EvotoBlockchain
This is the common code repository for blockchain interractions for use by both the evoto client (this) and the Registrar API. This library also manages the [MultiChain](http://www.multichain.com) platform processes.

## MultiChainLib
Handler for communication with the MultiChain API via RPC. Forked from [PbjCloud/MultiChainLib](https://github.com/PbjCloud/MultiChainLib), with additional wrapper for new and missing commands, such as raw transactions.
