CREATE TABLE [dbo].[PubTypes] (
    [PubTypeID]          INT          IDENTITY (1, 1) NOT NULL,
    [PubTypeDisplayName] VARCHAR (50) NOT NULL,
    [ColumnReference]    VARCHAR (50) NOT NULL,
    [IsActive]           BIT          NOT NULL,
    [SortOrder]          INT          NOT NULL,
    [DateCreated]        DATETIME     CONSTRAINT [DF_PubTypes_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]        DATETIME     NULL,
    [CreatedByUserID]    INT          NULL,
    [UpdatedByUserID]    INT          NULL,
    CONSTRAINT [PK_PubTypes] PRIMARY KEY CLUSTERED ([PubTypeID] ASC) WITH (FILLFACTOR = 90)
);



