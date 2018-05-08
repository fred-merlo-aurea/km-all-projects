CREATE TABLE [dbo].[AdHocDimensionGroup]
(
	[AdHocDimensionGroupId] INT IDENTITY(1,1) NOT NULL,
	[AdHocDimensionGroupName] varchar(50) NOT NULL,
	[ClientID] int NOT NULL,
	[SourceFileID] int NOT NULL,
	[IsActive] bit Default('true') NOT NULL,
	[OrderOfOperation] int Default(0) NOT NULL,
	[StandardField]      VARCHAR (50)  NOT NULL,
	[CreatedDimension]   VARCHAR (250) NOT NULL,
	[DefaultValue]		VARCHAR (MAX) NOT NULL,
	[IsPubcodeSpecific] bit Default('false') NOT NULL,
	[DateCreated]        DATETIME      NOT NULL,
    [DateUpdated]        DATETIME      NULL,
    [CreatedByUserID]    INT           NOT NULL,
    [UpdatedByUserID]    INT           NULL,
	CONSTRAINT [PK_AdHocDimensionGroup] PRIMARY KEY CLUSTERED ([AdHocDimensionGroupId] ASC)
)
