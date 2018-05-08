CREATE TABLE [dbo].[ServiceMethod] (
    [ServiceMethodID]   INT          IDENTITY (1, 1) NOT NULL,
    [ApplicationID]     INT          NOT NULL,
    [ServiceMethodName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ServiceMethod] PRIMARY KEY CLUSTERED ([ServiceMethodID] ASC)
);

