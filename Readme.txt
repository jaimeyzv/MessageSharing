.NEt Framework 4.6
SQL SERVER 2012

Installation in PC
	Install RabbitMQ Server
		Go to: https://www.rabbitmq.com/install-windows.html and select "Installer for Windows systems (from GitHub, recommended)". Install that .exe
		If installation ask for some dependencies. go to the page that installer suggests: http://www.erlang.org/downloads and select "OTP 21.2 Windows 64-bit Binary File"
	Setup Job => ChatRoom.MessageBroker.Consumer
		https://www.c-sharpcorner.com/UploadFile/manas1/console-application-using-windows-scheduler/
		This is to make the worker runs and validate the message leave in RabbitMQ. Dependen on that, worker will execute any of those 2 apis.
		
	Dependencies per projects:

	ChatRoom.DataAccess
		install-package NPoco -version 3.9.4
		Install-Package Unity -Version 5.5
		
	ChatRoom.DataAccess.Dtos
		install-package NPoco -version 3.9.4
		
	ChatRoom.DataAccess.Interfaces
		install-package NPoco -version 3.9.4
		
	ChatRoom.Injector	
		Install-Package Unity -Version 5.5
		
	ChatRoom.Insfrastucture	

	ChatRoom.Api
		Install-Package Unity.WebAPI -Version 5.3.0
		Install-Package Unity -Version 5.5
		Install-Package Swashbuckle -Version 5.6.0

	ChatRoom.MessageBroker
		Install-Package RabbitMQ.Client -Version 5.1.0
		
	ChatRoom.MessageBroker.Interfaces
		Install-Package RabbitMQ.Client -Version 5.1.0
		
	ChatRoom.Common
		Install-Package Newtonsoft.Json -Version 12.0.1

	ChatRoom.Api
		Install-Package Newtonsoft.Json -Version 12.0.1
		
	ChatRoom.Api.UnitTests
		Install-Package NUnit -Version 3.9.0
		Install-Package Moq -Version 4.10.1
		Install-Package Newtonsoft.Json -Version 12.0.1
		
	ChatRoom.MessageBroker.Processor
		Install-Package RabbitMQ.Client -Version 5.1.0
		Install-Package Newtonsoft.Json -Version 12.0.1	
			
	ChatRoom.DataAccess.UnitTests		
		Install-Package NUnit -Version 3.9.0
		Install-Package NUnit3TestAdapter -Version 3.9.0
		Install-Package NPoco -version 3.9.4
		Install-Package Moq -Version 4.10.1
		
	ChatRoom.MessageBroker.UnitTests
		Install-Package NUnit -Version 3.9.0
		Install-Package RabbitMQ.Client -Version 5.1.0
		Install-Package Moq -Version 4.10.1
		
Reference Urls
	https://www.red-gate.com/simple-talk/dotnet/net-development/visual-studio-2017-swagger-building-documenting-web-apis/
	https://somostechies.com/agregando-swagger-a-asp-net-web-api-2/
	
		