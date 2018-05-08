CREATE TABLE [dbo].[Code] (
    [CodeID]          INT           IDENTITY (1, 1) NOT NULL,
    [CodeType]        VARCHAR (50)  NULL,
    [CodeName]        VARCHAR (150) NULL,
    [CodeValue]       VARCHAR (50)  NULL,
    [CodeDescription] TEXT          CONSTRAINT [DF_Codes_CodeDescription] DEFAULT ('') NULL,
    [SystemFlag]      VARCHAR (10)  NULL,
    [SortCode]        INT           NULL,
    [CreatedUserID]   INT           NULL,
    [CreatedDate]     DATETIME      CONSTRAINT [DF_Code_CreatedDate] DEFAULT (getdate()) NULL,
    [UpdatedUserID]   INT           NULL,
    [UpdatedDate]     DATETIME      NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF_Code_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [Codes_PK] PRIMARY KEY CLUSTERED ([CodeID] ASC) WITH (FILLFACTOR = 80)
);

