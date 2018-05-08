CREATE TABLE [dbo].[Api]
(
	[ApiId] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [ServiceID] INT NOT NULL, 
    [ServiceFeatureID] INT NOT NULL, 
    [Entity] VARCHAR(100) NULL, 
    [Method] VARCHAR(100) NULL, 
    [IsEnabled] BIT NOT NULL, 
    [IsOnlyKM] BIT NOT NULL, 
    [IsClientSpecific] BIT NOT NULL
)
