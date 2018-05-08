CREATE TABLE [dbo].[BlastFields] (
    [BlastID]       INT           NOT NULL,
    [Field1]        VARCHAR (255) NULL,
    [Field2]        VARCHAR (255) NULL,
    [Field3]        VARCHAR (255) NULL,
    [Field4]        VARCHAR (255) NULL,
    [Field5]        VARCHAR (255) NULL,
    [CreatedDate]   DATETIME      CONSTRAINT [DF_BlastFields_CREATEDDATE] DEFAULT (getdate()) NULL,
    [CreatedUserID] INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_BlastFields_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedDate]   DATETIME      NULL,
    [UpdatedUserID] INT           NULL,
    CONSTRAINT [PK_BlastFields] PRIMARY KEY CLUSTERED ([BlastID] ASC) WITH (FILLFACTOR = 80)
);

