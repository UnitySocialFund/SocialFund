
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/04/2014 14:07:39
-- Generated from EDMX file: D:\0011 - Outside of work\SocialFund\SocialFund\DataModel\SFModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SocialFund];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Group_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group] DROP CONSTRAINT [FK_Group_User];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupUser_Group]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group_User] DROP CONSTRAINT [FK_GroupUser_Group];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupUser_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Group_User] DROP CONSTRAINT [FK_GroupUser_User];
GO
IF OBJECT_ID(N'[dbo].[FK_Log_Group_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Log] DROP CONSTRAINT [FK_Log_Group_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Group]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Group];
GO
IF OBJECT_ID(N'[dbo].[Group_User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Group_User];
GO
IF OBJECT_ID(N'[dbo].[Log]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Log];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------
SET QUOTED_IDENTIFIER OFF;
GO
CREATE DATABASE SocialFund
GO
USE [SocialFund];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- Creating table 'Group'
CREATE TABLE [dbo].[Group] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nchar(255)  NOT NULL,
    [OwnerId] int  NOT NULL,
    [BlogId] uniqueidentifier  NULL
);
GO

-- Creating table 'Group_User'
CREATE TABLE [dbo].[Group_User] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GroupId] int  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Log'
CREATE TABLE [dbo].[Log] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Coins] decimal(18,2)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Comment] varchar(max)  NOT NULL,
    [Group_UserId] int  NOT NULL
);
GO

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nchar(255)  NOT NULL,
	[FirstName] nchar(255)  NOT NULL,
	[MiddleName] nchar(255)  NOT NULL,
	[LastName] nchar(255)  NOT NULL,
    [Password] nchar(255)  NOT NULL,
    [Email] nchar(255)  NULL,
    [Phone] nchar(15)  NULL,
    [Address] nchar(255)  NULL,
    [IsNotif] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Group'
ALTER TABLE [dbo].[Group]
ADD CONSTRAINT [PK_Group]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Group_User'
ALTER TABLE [dbo].[Group_User]
ADD CONSTRAINT [PK_Group_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Log'
ALTER TABLE [dbo].[Log]
ADD CONSTRAINT [PK_Log]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User]
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [OwnerId] in table 'Group'
ALTER TABLE [dbo].[Group]
ADD CONSTRAINT [FK_Group_User]
    FOREIGN KEY ([OwnerId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Group_User'
CREATE INDEX [IX_FK_Group_User]
ON [dbo].[Group]
    ([OwnerId]);
GO

-- Creating foreign key on [GroupId] in table 'Group_User'
ALTER TABLE [dbo].[Group_User]
ADD CONSTRAINT [FK_GroupUser_Group]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Group]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupUser_Group'
CREATE INDEX [IX_FK_GroupUser_Group]
ON [dbo].[Group_User]
    ([GroupId]);
GO

-- Creating foreign key on [UserId] in table 'Group_User'
ALTER TABLE [dbo].[Group_User]
ADD CONSTRAINT [FK_GroupUser_User]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupUser_User'
CREATE INDEX [IX_FK_GroupUser_User]
ON [dbo].[Group_User]
    ([UserId]);
GO

-- Creating foreign key on [Group_UserId] in table 'Log'
ALTER TABLE [dbo].[Log]
ADD CONSTRAINT [FK_Log_Group_User]
    FOREIGN KEY ([Group_UserId])
    REFERENCES [dbo].[Group_User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Log_Group_User'
CREATE INDEX [IX_FK_Log_Group_User]
ON [dbo].[Log]
    ([Group_UserId]);
GO


-- --------------------------------------------------
-- Insetring demo data
-- --------------------------------------------------


INSERT INTO [SocialFund].[dbo].[User]
           ([Name]
           ,[FirstName]
           ,[MiddleName]
           ,[LastName]
           ,[Password]
           ,[Email]
           ,[Phone]
           ,[Address]
           ,[IsNotif])
     VALUES
		   ('admin','Admin','Admin','Admin','ANNrWBKCKSkKavUP1FEHpq702Bqir8kBEDj4Q58/gJdOn/TA05VXcUU8oy1PHDafPg==','admin@gmail2.com','9630215487','admin address',0),
           ('user1','����','�������������','���������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user1@gmail1.com','9630215487','address',0),
           ('user2','����','������','������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user2@gmail1.com','9630215487','address',0),
           ('user3','����','������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user3@gmail1.com','9630215487','address',0),
           ('user4','�������','����������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user4@gmail1.com','9630215487','address',0),
           ('user5','����','�������������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user5@gmail1.com','9630215487','address',0),
           ('user6','�����','��������','�������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user6@gmail1.com','9630215487','address',0),
           ('user7','������','���������','�������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user7@gmail1.com','9630215487','address',0),
           ('user8','�����','���������','�����','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user8@gmail1.com','9630215487','address',0),
           ('user9','���������','������������','����������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user9@gmail1.com','9630215487','address',0),
		   
		   ('user10','����','������','������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user10@gmail1.com','9630215487','address',0),
           ('user11','����','������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user11@gmail1.com','9630215487','address',0),
           ('user12','�������','����������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user12@gmail1.com','9630215487','address',0),
           ('user13','����','�������������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user13@gmail1.com','9630215487','address',0),
           ('user14','�����','��������','�������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user14@gmail1.com','9630215487','address',0),
           ('user15','������','���������','�������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user15@gmail1.com','9630215487','address',0),
           ('user16','�����','���������','�����','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user16@gmail1.com','9630215487','address',0),
           ('user17','���������','������������','����������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user17@gmail1.com','9630215487','address',0),
		   ('user18','����','������','������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user18@gmail1.com','9630215487','address',0),
           ('user19','����','������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user19@gmail1.com','9630215487','address',0),

           ('user20','�������','����������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user20@gmail1.com','9630215487','address',0),
           ('user21','����','�������������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user21@gmail1.com','9630215487','address',0),
           ('user22','�����','��������','�������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user22@gmail1.com','9630215487','address',0),
           ('user23','������','���������','�������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user23@gmail1.com','9630215487','address',0),
           ('user24','�����','���������','�����','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user24@gmail1.com','9630215487','address',0),
           ('user25','���������','������������','����������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user25@gmail1.com','9630215487','address',0),
           ('user26','�����','���������','�����','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user26@gmail1.com','9630215487','address',0),
           ('user27','���������','������������','����������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user27@gmail1.com','9630215487','address',0),
		   ('user28','����','������','������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user28@gmail1.com','9630215487','address',0),
           ('user29','����','������','��������','AHe3Auu5vW8vaDjzAedNii/LWRrqgNuvrHVJjE+HBTqE0v0j2dX2gQb6APOV8xPqgA==','user29@gmail1.com','9630215487','address',0)
GO

INSERT INTO [SocialFund].[dbo].[Group]
           ([Name]
           ,[OwnerId]
           ,[BlogId])
     VALUES
		   ('AdminGroup',1,null),
           ('�������',2,null),
           ('�������',5,null),
           ('�������',8,null),
           ('Gans''n''Roses',10,null)
GO

INSERT INTO [SocialFund].[dbo].[Group_User]
           ([GroupId]
           ,[UserId])
     VALUES
		   (1,1),
		   (1,11),
		   (1,12),
		   (1,13),
		   (1,14),
		   (1,15),
		   (1,16),
		   (1,17),
		   (1,18),
		   (1,19),

           (2,2),
           (2,3),
           (2,4),
           (3,5),
           (3,6),
           (3,7),
           (4,8),
           (4,9),
		   (5,10)
GO

INSERT INTO [SocialFund].[dbo].[Log]
           ([Coins]
           ,[Date]
           ,[Comment]
           ,[Group_UserId])
     VALUES
           (32,'10/7/2014 6:21:42 PM','Comment',1),
           (48,'10/7/2014 6:21:42 PM','Comment',1),
           (300,'10/7/2014 6:21:42 PM','Comment',2),
           (-50,'10/7/2014 6:21:42 PM','Comment',1),
           (32,'10/7/2014 6:21:42 PM','Comment',3),
           (68,'10/7/2014 6:21:42 PM','Comment',4),
           (-100,'10/7/2014 6:21:42 PM','Comment',1),
           (150,'10/7/2014 6:21:42 PM','Comment',8)
GO
-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------