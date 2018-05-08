CREATE TABLE [dbo].[ResponseTemp] (
    [ResponseID]      INT           NOT NULL,
    [ResponseTypeID]  INT           NOT NULL,
    [PublicationID]   INT           NOT NULL,
    [ResponseName]    VARCHAR (250) NOT NULL,
    [ResponseCode]    CHAR (10)     NOT NULL,
    [DisplayName]     VARCHAR (250) NOT NULL,
    [DisplayOrder]    INT           NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    CONSTRAINT [PK__Response__1AAA640C60CDB258] PRIMARY KEY CLUSTERED ([ResponseID] ASC)
);

