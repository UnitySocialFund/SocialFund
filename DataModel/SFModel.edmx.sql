
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/09/2014 14:38:29
-- Generated from EDMX file: D:\Projects\Unity\SocialFund\DataModel\SFModel.edmx
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

-- Creating table 'Group'
CREATE TABLE [dbo].[Group] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nchar(255)  NOT NULL,
    [OwnerId] int  NOT NULL
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
    [Password] nchar(255)  NOT NULL,
    [Email] nchar(255)  NULL
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
-- Script has ended
-- --------------------------------------------------