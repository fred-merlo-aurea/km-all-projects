CREATE TABLE [dbo].[Service] (
    [ServiceID]               INT             IDENTITY (1, 1) NOT NULL,
    [ServiceName]             VARCHAR (100)   NOT NULL,
    [Description]             VARCHAR (500)   NULL,
    [ServiceCode]             VARCHAR (20)    NOT NULL,
    [DisplayOrder]            INT             NOT NULL,
    [IsEnabled]               BIT             NOT NULL,
    [IsAdditionalCost]        BIT             NOT NULL,
    [HasFeatures]             BIT             NOT NULL,
    [DefaultRate]             DECIMAL (14, 2) NOT NULL,
    [DefaultDurationInMonths] INT             NOT NULL,
    [DefaultApplicationID]    INT             CONSTRAINT [DF__Service__Default__76969D2E] DEFAULT ((0)) NOT NULL,
    [DateCreated]             DATETIME        NOT NULL,
    [DateUpdated]             DATETIME        NULL,
    [CreatedByUserID]         INT             NOT NULL,
    [UpdatedByUserID]         INT             NULL,
    CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED ([ServiceID] ASC)
);



GO