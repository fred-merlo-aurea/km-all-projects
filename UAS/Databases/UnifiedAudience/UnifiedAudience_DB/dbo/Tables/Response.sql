CREATE TABLE [dbo].[Response] (
    [ResponseID]      INT           IDENTITY (1, 1) NOT NULL,
    [ResponseGroupID] INT           NULL,
    [PublicationID]   INT           NOT NULL,
    [ResponseName]    VARCHAR (250) NOT NULL,
    [ResponseCode]    VARCHAR (250) NOT NULL,
    [DisplayName]     VARCHAR (250) NOT NULL,
    [DisplayOrder]    INT           NOT NULL,
    [ReportGroupID]   INT           NULL,
    [IsActive]        BIT           NOT NULL,
    [DateCreated]     DATETIME      CONSTRAINT [DF_Response_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NOT NULL,
    [UpdatedByUserID] INT           NULL,
    [WQT_ResponseID]  INT           NULL,
    [IsOther]         BIT           CONSTRAINT [DF_Response_IsOther] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Response] PRIMARY KEY CLUSTERED ([ResponseID] ASC)
);





