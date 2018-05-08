CREATE TABLE [dbo].[Par3c] (
    [Par3CID]         INT           IDENTITY (1, 1) NOT NULL,
    [DisplayName]     VARCHAR (250) NOT NULL,
    [DisplayOrder]    INT           NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    PRIMARY KEY CLUSTERED ([Par3CID] ASC) WITH (FILLFACTOR = 80)
);

