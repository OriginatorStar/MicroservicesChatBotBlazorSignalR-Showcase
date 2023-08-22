# .Net Microservices Chat and Bot with Blazor and SignalR Challenge Showcase

### Index

 - Intro.
 - Initial/Original Resolution Strategy.
 - Further improvements I would like to keep working on.
 - Complete Requirement Challenge Document.

### Intro

Hi! I am Ariel Altamirano, a .Net developer with more than 15 years of experience in Microsoft .Net all around stacks and technologies. I live in Buenos Aires, Argentina, with my wife and my little child.

This application can be launched by:
(If by setting as startup project the "docker-compose" one and pressing the "Docker Compose" button up there does not work because I did not fix it yet, do the following. I'll fix it when you ask me and push the changes.)

 - Right click on the "dcoker-compose" project, and "Unload Project".
 - Launch a rabbitmq container by running in terminal:
  - docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq (wait for it to be up and running. You should have Docker Desktop installed, if not, you may install RabbitMQ on you computer).
 - Set up a Data Source=(localdb)\MSSQLLocalDB DB localhost. Instructions [here](https://www.sqlshack.com/install-microsoft-sql-server-express-localdb/).
 - Go to main menu Project > "Configure Startup Projects" > "Multiple Startup Projects" and select as start projects: Chat.BlazorChat, Chat.DecoupledBot, Chat.SignalRHub.
 - Press the Start button up there, or the F5 key.
 - Go with the UI/UX.

### Initial/Original Resolution Strategy.

The general layout of the project is the following: A Blazor FE application, the communication framework for the chat will be Signal R, and the Bot will be located in an isolated decoupled service project. Every project component, the web FE app, the SignalR Hub, the message broker, the Bot service and the DB, will be each one of them in their individual containers, and they will be orchastrated by a docker-compose command.

 I have pseudocoded quickly the main intention and structure of the tests for the projects Chat.BlazorChat and Chat.DecoupledBot.

 RabbitMQ is managed by the MassTransit wrapper client. I've used a direct communication with an asynchronous request/response pattern, leveraged by TPL syntax in the code-behind of the .razor files for the UI.

### Further improvements I would like to keep working on

 - Extract all hard/fixed values to appsettings.json, for example URLs and the like.
 - Tweak tests to make them all pass since they are test intentions pseudocode in this first iteration.
 - Refactor classes and signatures extracting interfaces and decoupling further.
 - [Kaizen](https://www.wikiwand.com/en/Kaizen).

### Complete Requirement Challenge Document.

### Description
This project is designed to showcase my knowledge of back-end web technologies, specifically in
.NET and showcase my ability to create back-end products with attention to details, standards,
and reusability.

### Assignment
The goal of this exercise is to create a simple browser-based chat application using .NET.
This application should allow several users to talk in a chatroom and also to get stock quotes
from an API using a specific command.

### Features
- Allow registered users to log in and talk with other users in a chatroom.
- Allow users to post messages as commands into the chatroom with the following format
/stock=stock_code (like /stock=aapl.us)
- Create a decoupled bot that will call an API using the stock_code as a parameter
(https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv, here aapl.us is the
stock_code)
- The bot should parse the received CSV file and then it should send a message back into
the chatroom using a message broker like RabbitMQ. The message will be a stock quote
using the following format: “APPL.US quote is $93.42 per share”. The post owner will be
the bot.
- Have the chat messages ordered by their timestamps and show only the last 50
messages.
- Unit tests.
- Have more than one chatroom.
- Use .NET identity for users authentication
- Handle messages that are not understood or any exceptions raised within the bot.
- Visual Studio Docker Tools.

### Considerations
- The stock command won’t be saved on the database as a post.



