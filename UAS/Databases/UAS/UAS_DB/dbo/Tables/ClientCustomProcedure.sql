CREATE TABLE [dbo].[ClientCustomProcedure] (
    [ClientCustomProcedureID] INT           IDENTITY (1, 1) NOT NULL,
    [ClientID]                INT           NOT NULL,
    [IsActive]                BIT           NOT NULL,
    [ProcedureName]           VARCHAR (50)  NOT NULL,
    [ExecutionOrder]          INT           NOT NULL,
    [DateCreated]             DATETIME      NOT NULL,
    [DateUpdated]             DATETIME      NULL,
    [CreatedByUserID]         INT           NOT NULL,
    [UpdatedByUserID]         INT           NULL,
    [ProcedureType]           VARCHAR (100) NOT NULL,
    [ServiceID]               INT           NOT NULL,
    [ServiceFeatureID]        INT           NOT NULL,
    [ExecutionPointID]        INT           NOT NULL,
    [IsForSpecialFile]        BIT           NOT NULL,
    CONSTRAINT [PK_ClientCustomProcedure] PRIMARY KEY CLUSTERED ([ClientCustomProcedureID] ASC) WITH (FILLFACTOR = 90)
);



