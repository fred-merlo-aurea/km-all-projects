CREATE TABLE [dbo].[CategoryCode] (
    [CategoryCodeID]     INT          IDENTITY (1, 1) NOT NULL,
    [CategoryCodeTypeID] INT          NOT NULL,
    [CategoryCodeName]   VARCHAR (50) NOT NULL,
    [CategoryCodeValue]  INT    NOT NULL,
    [IsActive]           BIT          CONSTRAINT [DF_CategoryCode_IsActive] DEFAULT ((0)) NOT NULL,
    [DateCreated]        DATETIME     NOT NULL,
    [DateUpdated]        DATETIME     NULL,
    [CreatedByUserID]    INT          NOT NULL,
    [UpdatedByUserID]    INT          NULL,
    CONSTRAINT [PK_CategoryCode] PRIMARY KEY CLUSTERED ([CategoryCodeTypeID] ASC, [CategoryCodeValue] ASC) WITH (FILLFACTOR = 80)
);

