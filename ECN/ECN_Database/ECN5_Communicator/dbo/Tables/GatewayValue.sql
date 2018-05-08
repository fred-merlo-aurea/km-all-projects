CREATE TABLE [dbo].[GatewayValue]
(
	[GatewayValueID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GatewayID] INT NULL, 
    [Field] VARCHAR(100) NULL, 
    [IsLoginValidator] BIT NULL, 
    [IsCaptureValue] BIT NULL, 
    [Value] VARCHAR(255) NULL, 
    [IsDeleted] BIT NULL, 
    [Comparator] VARCHAR(50) NULL, 
    [NOT] BIT NULL, 
    [FieldType] VARCHAR(100) NULL, 
    [DatePart] VARCHAR(50) NULL, 
    [CreatedUserID] INT NULL, 
    [CreatedDate] DATETIME NULL, 
    [UpdatedUserID] INT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [IsStatic] BIT NULL, 
    [Label] VARCHAR(MAX) NULL
)
