CREATE TABLE [dbo].[ResponseType] (
    [ResponseTypeID]      INT           IDENTITY (1, 1) NOT NULL,
    [PublicationID]       INT           NOT NULL,
    [ResponseTypeName]    VARCHAR (100) NOT NULL,
    [DisplayName]         VARCHAR (100) NOT NULL,
    [DisplayOrder]        INT           NOT NULL,
    [IsMultipleValue]     BIT           NOT NULL,
    [IsRequired]          BIT           NOT NULL,
    [IsActive]            BIT           NOT NULL,
    [DateCreated]         DATETIME      CONSTRAINT [DF_ResponseType_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]         DATETIME      NULL,
    [CreatedByUserID]     INT           NOT NULL,
    [UpdatedByUserID]     INT           NULL,
    [WQT_ResponseGroupID] INT           NULL,
    CONSTRAINT [PK_ResponseType] PRIMARY KEY CLUSTERED ([ResponseTypeID] ASC)
);





