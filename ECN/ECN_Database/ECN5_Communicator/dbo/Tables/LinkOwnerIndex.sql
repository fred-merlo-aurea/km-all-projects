CREATE TABLE [dbo].[LinkOwnerIndex] (
    [LinkOwnerIndexID] INT           IDENTITY (1, 1) NOT NULL,
    [CustomerID]       INT           NOT NULL,
    [LinkOwnerName]    VARCHAR (50)  NOT NULL,
    [LinkOwnerCode]    VARCHAR (10)  NULL,
    [ContactFirstName] VARCHAR (50)  NULL,
    [ContactLastName]  VARCHAR (50)  NULL,
    [ContactPhone]     VARCHAR (50)  NULL,
    [ContactEmail]     VARCHAR (50)  NULL,
    [Address]          VARCHAR (100) NULL,
    [City]             VARCHAR (50)  NULL,
    [State]            VARCHAR (50)  NULL,
    [IsActive]         BIT           CONSTRAINT [DF_LinkOwnerIndex_Status] DEFAULT (1) NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_LinkOwnerIndex_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID]    INT           NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_LinkOwnerIndex_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]      DATETIME      NULL,
    [UpdatedUserID]    INT           NULL,
    CONSTRAINT [PK_LinkOwnerIndex] PRIMARY KEY CLUSTERED ([LinkOwnerIndexID] ASC) WITH (FILLFACTOR = 80, STATISTICS_NORECOMPUTE = ON)
);

