🎨 SendColorBot
===============

SendColorBot is a telegram bot that helps to send colors through the Inline interface. 

![alt text](https://i.imgur.com/nPk9gjO.png "Inline interface") ![alt text](https://i.imgur.com/98trdfk.png "Final message")

## 🛠 Tools that project uses

[.NET Core](https://dot.net) — Cross-platform general-purpose development platform.

[.NET Extensions](https://github.com/aspnet/Extensions) — An open-source, cross-platform set of APIs for commonly used programming patterns and utilities.

[Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot) — .NET Client for Telegram Bot API.

[ImageSharp](https://github.com/SixLabors/ImageSharp) — Cross-platform library for image file processing.

## ▶️ Build
You need .NET Core SDK 2.2+. [Download it here](https://dotnet.microsoft.com/download/dotnet-core/3.0)

Also, you need a file server with SSL support. It used to send generated images. I use nginx for this, but you can try any other. For easy installation, you can use autoinstall script by [@angristan](https://github.com/angristan) from [here](https://github.com/angristan/nginx-autoinstall). In addition, you can use [config generator](https://nginxconfig.io/) by [@0xB4LINT](https://github.com/0xB4LINT).

1. Clone repository and open directory:
   ```sh
   git clone https://github.com/yet-another-devteam/SendColorBot.git && cd SendColorBot
2. Build the project. You can also use portable mode, which doesn't require .NET runtime. [Learn more](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build)
    ```sh
    dotnet publish -c Release
3. Navigate to the directory that specified in the console:
   ```sh
   ...
   // Here is the path
   SendColorBot -> /home/dubzer/SendColorBot/SendColorBot/bin/Release/netcoreapp2.2/publish/    
   ...
   
   cd ./SendColorBot/bin/Release/netcoreapp2.2/publish
4. Create **configuration.json**, paste this text and insert your values:
   ```json
   {
    "tokens": {
     "telegramapi": "Bot token. Get it at @BotFather",
     "fileserver-path": "Path to the file server public directory. This is where fileserver-domain should point",
     "fileserver-domain": "Public domain with pictures"
    } 
   }
5. Run:
    ```sh
    dotnet ./SendColorBot.dll
## 📝 License
The project is licensed under the [MIT license](https://github.com/yet-another-devteam/SendColorBot/blob/master/LICENSE).
