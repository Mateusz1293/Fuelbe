﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dbo].[user] (
    [id] int NOT NULL IDENTITY,
    [login] nvarchar(max) NOT NULL,
    [email] nvarchar(max) NULL,
    [password] nvarchar(max) NOT NULL,
    [first_name] nvarchar(max) NULL,
    [last_name] nvarchar(max) NULL,
    CONSTRAINT [PK_user] PRIMARY KEY ([id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230609072029_Initial', N'6.0.16');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [dbo].[vehicle] (
    [id] int NOT NULL IDENTITY,
    [model] nvarchar(max) NOT NULL,
    [register_number] nvarchar(max) NOT NULL,
    [vin] nvarchar(max) NOT NULL,
    [vehicle_type] nvarchar(max) NOT NULL,
    [gearbox] nvarchar(max) NOT NULL,
    [production_year] int NOT NULL,
    [fuel_type] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_vehicle] PRIMARY KEY ([id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230626093229_add-vehicles-table', N'6.0.16');
GO

COMMIT;
GO

