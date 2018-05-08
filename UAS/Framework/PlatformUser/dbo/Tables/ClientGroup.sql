CREATE TABLE [dbo].[ClientGroup] (
    [ClientGroupID]          INT           IDENTITY (1, 1) NOT NULL,
    [ClientGroupName]        VARCHAR (50)  NOT NULL,
    [ClientGroupDescription] VARCHAR (250) NULL,
    [Color]                  VARCHAR (50)  CONSTRAINT [DF__ClientGro__Color__6C190EBB] DEFAULT ('transparent') NOT NULL,
    [IsActive]               BIT           NOT NULL,
    [DateCreated]            DATETIME      CONSTRAINT [DF_ClientGroup_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]            DATETIME      NULL,
    [CreatedByUserID]        INT           NOT NULL,
    [UpdatedByUserID]        INT           NULL,
    CONSTRAINT [PK__ClientGr__1C856DD31BFD2C07] PRIMARY KEY CLUSTERED ([ClientGroupID] ASC)
);

