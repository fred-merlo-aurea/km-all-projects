CREATE TABLE [dbo].[DynamicTag] (
    [DynamicTagID]  INT          IDENTITY (1, 1) NOT NULL,
    [Tag]           VARCHAR (50) NULL,
    [ContentID]     INT          NULL,
    [CustomerID]    INT          NULL,
    [CreatedUserID] INT          NULL,
    [CreatedDate]   DATETIME     NULL,
    [UpdatedDate]   DATETIME     NULL,
    [UpdatedUserID] INT          NULL,
    [IsDeleted]     BIT          NULL,
    CONSTRAINT [PK_DynamicTag] PRIMARY KEY CLUSTERED ([DynamicTagID] ASC) WITH (FILLFACTOR = 80)
);

