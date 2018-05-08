CREATE TABLE [dbo].[AdHocDimensionGroupPubcodeMap]
(
	[AdHocDimensionGroupId] INT NOT NULL,
	[Pubcode] varchar(50) NOT NULL,
	[IsActive] bit Default('true') NOT NULL,
	[DateCreated]        DATETIME      NOT NULL,
    [DateUpdated]        DATETIME      NULL,
    [CreatedByUserID]    INT           NOT NULL,
    [UpdatedByUserID]    INT           NULL,
	CONSTRAINT [PK_AdHocDimensionGroupPubcodeMap] PRIMARY KEY CLUSTERED ([AdHocDimensionGroupId] ASC,[Pubcode] ASC)
)
