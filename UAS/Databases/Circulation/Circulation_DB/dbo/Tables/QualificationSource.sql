CREATE TABLE [dbo].[QualificationSource] (
    [QSourceID]       INT           IDENTITY (1, 1) NOT NULL,
    [QSourceTypeID]   INT           NOT NULL,
    [QSourceName]     VARCHAR (100) NOT NULL,
    [QSourceCode]     CHAR (10)     NOT NULL,
    [DisplayName]     VARCHAR (100) NOT NULL,
    [DisplayOrder]    INT           NOT NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    CONSTRAINT [PK_QualificationSource] PRIMARY KEY CLUSTERED ([QSourceID] ASC) WITH (FILLFACTOR = 80)
);

