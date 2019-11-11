-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/23/2019 00:51:10
-- Generated from EDMX file: C:\Users\zhy\Downloads\FIT5032_Week08A\FIT5032_Week08A\Models\FIT5032_Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [C:\USERS\ZHY\DOWNLOADS\FIT5032_ASSIGN-MASTER\FIT5032_ASSIGN\FIT5032_ASSIGN\APP_DATA\DATABASE.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO



-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'HotelSet'
CREATE TABLE [dbo].[Hotel] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Latitude] NUMERIC(10,8)  NOT NULL,
    [Longitutde] NUMERIC(11,8)  NOT NULL,
    [HotelAddress] nvarchar(max)  NOT NULL,
    [HotelDescription] nvarchar(max)  NOT NULL,
    [HotelRating] nvarchar(max)  NOT NULL,
    [HotelName] nvarchar(max)  NOT NULL,
    [HotelCity] nvarchar(max)  NOT NULL

);
GO

-- Creating table 'RoomSet'
CREATE TABLE [dbo].[Room] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RoomType] nvarchar(max)  NOT NULL,
    [RoomDescription] nvarchar(max)  NOT NULL,
    [TotalRoom] INT  NOT NULL,
    [RoomPrice]  FLOAT (53)  NOT NULL,
    [HotelId] int  NOT NULL
);
GO

-- Creating table 'BookingSet'
CREATE TABLE [dbo].[Booking] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CheckinDate] DATETIME  NOT NULL,
    [CheckoutDate] DATETIME  NOT NULL,
    [ASPUserId] nvarchar(max)  NOT NULL,
    [Rating] INT  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [RoomId] int  NOT NULL,
    [Price] FLOAT (53)  NOT NULL,
	[RoomNumber] INT NOT NULL
);
GO

-- Creating table 'ImageSet'
CREATE TABLE [dbo].[Image] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HotelId] int  NOT NULL,
    [ImageName] nvarchar(max)  NOT NULL,
    [ImagePath] nvarchar(max)  NOT NULL
);
GO

CREATE TABLE [dbo].[RoomStates] (
    [Id]          INT        IDENTITY (1, 1) NOT NULL,
    [RoomId]  INT    NOT NULL,
    [Date]        DATETIME   NOT NULL,
    [PriceChange] FLOAT (53) NOT NULL,
    [AvaibleRoom] INT        NOT NULL,
    CONSTRAINT [PK_RoomStates] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [CK_AvaibleRoom] CHECK ([AvaibleRoom]>=(0))
);

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'HotelSet'
ALTER TABLE [dbo].[Hotel]
ADD CONSTRAINT [PK_Hotel]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoomSet'
ALTER TABLE [dbo].[Room]
ADD CONSTRAINT [PK_Room]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BookingSet'
ALTER TABLE [dbo].[Booking]
ADD CONSTRAINT [PK_Booking]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ImageSet'
ALTER TABLE [dbo].[Image]
ADD CONSTRAINT [PK_Image]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [HotelId] in table 'RoomSet'
ALTER TABLE [dbo].[Room]
ADD CONSTRAINT [FK_HotelRoom]
    FOREIGN KEY ([HotelId])
    REFERENCES [dbo].[Hotel]
        ([Id])
    ON DELETE CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelRoom'
CREATE INDEX [IX_FK_HotelRoom]
ON [dbo].[Room]
    ([HotelId]);
GO

-- Creating foreign key on [HotelId] in table 'RoomSet'
ALTER TABLE [dbo].[RoomStates]
ADD CONSTRAINT [FK_RoomRoomStates]
    FOREIGN KEY ([RoomId])
    REFERENCES [dbo].[Room]
        ([Id])
    ON DELETE CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelRoom'
CREATE INDEX [IX_FK_RoomRoomStates]
ON [dbo].[RoomStates]
    ([RoomId]);
GO

-- Creating foreign key on [RoomId] in table 'BookingSet'
ALTER TABLE [dbo].[Booking]
ADD CONSTRAINT [FK_RoomBooking]
    FOREIGN KEY ([RoomId])
    REFERENCES [dbo].[Room]
        ([Id])
    ON DELETE CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoomBooking'
CREATE INDEX [IX_FK_RoomBooking]
ON [dbo].[Booking]
    ([RoomId]);
GO

-- Creating foreign key on [HotelId] in table 'ImageSet'
ALTER TABLE [dbo].[Image]
ADD CONSTRAINT [FK_HotelImage]
    FOREIGN KEY ([HotelId])
    REFERENCES [dbo].[Hotel]
        ([Id])
    ON DELETE CASCADE;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HotelImage'
CREATE INDEX [IX_FK_HotelImage]
ON [dbo].[Image]
    ([HotelId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------