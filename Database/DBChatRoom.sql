use master;
go

create database Chatroom;
go

use Chatroom;
go

create table [dbo].[Profile]
(
	ProfileId		int identity(1,1) not null,
	Code			varchar(15) not null unique,
	Description		varchar(50),
	IsActive		bit

	primary key (ProfileId)
);
go


INSERT [dbo].[Profile] (Code, Description, IsActive) VALUES ('ADMINISTRATOR', '', 1);
INSERT [dbo].[Profile] (Code, Description, IsActive) VALUES ('USER', '', 1);
SELECT * FROM [dbo].[Profile];
go

create table [dbo].[User]
(
	UserId		int identity (1,1) not null,
	NickName	varchar(20) not null unique,
	Name		varchar(50),
	LastName	varchar(50),
	ProfileCode	varchar(15)

	primary key (UserId)
	foreign key (ProfileCode) references [dbo].[Profile](Code)
);
go

INSERT [dbo].[User] (NickName, Name, LastName, ProfileCode) VALUES ('jaimeyzv', 'Jaime', 'Zamora', 'ADMINISTRATOR');
INSERT [dbo].[User] (NickName, Name, LastName, ProfileCode) VALUES ('johao', 'Johao', 'Rosas', 'USER');
SELECT * FROM [dbo].[User];

create table [dbo].[Chatroom]
(
	ChatroomId		int identity (1,1) not null,
	Name			varchar(50) not null,
	Description		varchar(250),
	Owner			varchar(20) not null,
	IsActive		bit

	primary key (ChatroomId),
	foreign key (Owner) references [dbo].[User](NickName)
);
go

INSERT [dbo].[Chatroom] (Name, Description, Owner, IsActive) VALUES ('Pokemon Go Raids', 'Coordinates to join the raid battle', 'jaimeyzv', 1);
SELECT * FROM [dbo].[Chatroom];

create table [dbo].[Message]
(
	MessageId	int identity (1,1) not null,
	Content		varchar(500) not null,
	Date		Datetime not null,
	ChatroomId	int not null,
	UserSender	varchar(20) not null,
	IsBot		bit

	primary key (MessageId),
	foreign key (UserSender) references [dbo].[User](NickName)
);
go

INSERT [dbo].[Message] (Content, Date, ChatroomId, UserSender, IsBot) VALUES ('Hi, I would like to know where is the best pokestop for raids', GETDATE(), 1, 'johao', 0);
INSERT [dbo].[Message] (Content, Date, ChatroomId, UserSender, IsBot) VALUES ('Hey! for me the best one is the one in Central Pack, blue gym ex raid.', GETDATE(), 1, 'jaimeyzv', 0);
SELECT top 1 * FROM [dbo].[Message] WHERE ChatroomId = 1
SELECT * FROM [dbo].[Message] WHERE ChatroomId = 1
SELECT TOP 50 * FROM [dbo].[Message] WHERE ChatroomId = 1 ORDER BY DATE ASC

