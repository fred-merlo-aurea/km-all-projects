CREATE TABLE [dbo].[CategoryCodeType] (
    [CategoryCodeTypeID]   INT          IDENTITY (1, 1) NOT NULL,
    [CategoryCodeTypeName] VARCHAR (50) NOT NULL,
    [IsActive]             BIT          CONSTRAINT [DF_CategoryCodeType_IsActive] DEFAULT ((0)) NOT NULL,
    [IsFree]               BIT          NULL,
    [DateCreated]          DATETIME     NOT NULL,
    [DateUpdated]          DATETIME     NULL,
    [CreatedByUserID]      INT          NOT NULL,
    [UpdatedByUserID]      INT          NULL,
    CONSTRAINT [PK_CategoryCodeType] PRIMARY KEY CLUSTERED ([CategoryCodeTypeID] ASC) WITH (FILLFACTOR = 80)
);


