IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [BankDetails] (
    [AccountId] uniqueidentifier NOT NULL,
    [AccountName] nvarchar(max) NOT NULL,
    [Balance] float NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_BankDetails] PRIMARY KEY ([AccountId])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250918064550_migration1', N'9.0.9');

COMMIT;
GO

