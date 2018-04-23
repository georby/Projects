-- Create Database

USE master
IF EXISTS(select * from sys.databases where name='ACCOUNTSDB_GEO')
DROP DATABASE ACCOUNTSDB_GEO

CREATE DATABASE ACCOUNTSDB_GEO
GO

-- Create Tables

USE ACCOUNTSDB_GEO

CREATE TABLE [dbo].[accountDetails]
(
	[accountId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [holderName] NVARCHAR(50) NOT NULL, 
    [accountBalance] DECIMAL(18,5) NOT NULL, 
    [addedOn] DATETIME2 NOT NULL, 
    [addedBy] NVARCHAR(50) NOT NULL, 
    [modifiedOn] DATETIME2 NOT NULL, 
    [modifiedBy] NVARCHAR(50) NOT NULL
)

CREATE TABLE [dbo].[currencyDetails]
(
	[currencyId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [currencyCode] NCHAR(10) NOT NULL, 
    [currencyName] NVARCHAR(50) NOT NULL, 
    [conversionRateToDollar] DECIMAL(18,5) NOT NULL, 
    [addedOn] DATETIME2 NOT NULL, 
    [addedBy] NVARCHAR(50) NOT NULL, 
    [modifiedOn] DATETIME2 NOT NULL, 
    [modifiedBy] NVARCHAR(50) NOT NULL
)

CREATE TABLE [dbo].[transactionLog]
(
    [transactionId]          INT             IDENTITY (1, 1) NOT NULL,
    [accountId]              INT             NOT NULL,
    [currencyCode]           NCHAR (10)      NOT NULL,
    [ConversionRateToDollar] DECIMAL (18, 5) NOT NULL,
    [balanceBefore]          DECIMAL (18, 5) NOT NULL,
    [balanceAfter]           DECIMAL (18, 5) NOT NULL,
    [transactionType]        NCHAR (10)      NOT NULL,
    [transactionAmount]      DECIMAL (18, 5) NOT NULL,
    [addedOn]                DATETIME2 (7)   NOT NULL,
    [addedBy]                NVARCHAR (50)   NOT NULL,
    PRIMARY KEY CLUSTERED ([transactionId] ASC)
)

GO

-- Insert Values to Created Tables

USE ACCOUNTSDB_GEO

IF EXISTS (SELECT * FROM accountDetails)
    BEGIN
        TRUNCATE TABLE accountDetails
    END

INSERT INTO accountDetails 
(holderName,accountBalance,addedOn,addedBy,modifiedOn,modifiedBy)
VALUES 
('John',50000.00,GETDATE(),'admin',GETDATE(),'admin'),
('Mary',50000.00,GETDATE(),'admin',GETDATE(),'admin'),
('Adam',50000.00,GETDATE(),'admin',GETDATE(),'admin'),
('Claire',50000.00,GETDATE(),'admin',GETDATE(),'admin'),
('William',50000.00,GETDATE(),'admin',GETDATE(),'admin'),
('Elsa',50000.00,GETDATE(),'admin',GETDATE(),'admin')


IF EXISTS (SELECT * FROM currencyDetails)
    BEGIN
        TRUNCATE TABLE currencyDetails
    END

INSERT INTO currencyDetails 
(currencyCode,currencyName,conversionRateToDollar,addedOn,addedBy,modifiedOn,modifiedBy)
VALUES 
('£','Pound',0.70,GETDATE(),'admin',GETDATE(),'admin'),
('S$','Singapore Dollar',1.31,GETDATE(),'admin',GETDATE(),'admin'),
('€','Euro',0.81,GETDATE(),'admin',GETDATE(),'admin'),
('Baht','Thai Baht',31.13,GETDATE(),'admin',GETDATE(),'admin')

GO