CREATE TABLE [dbo].[SmartFormsPrePopFields] (
    [PrePopFieldID]    INT           IDENTITY (1, 1) NOT NULL,
    [SFID]             INT           NULL,
    [ProfileFieldName] VARCHAR (50)  NULL,
    [DisplayName]      VARCHAR (255) NULL,
    [DataType]         VARCHAR (50)  NULL,
    [ControlType]      VARCHAR (50)  NULL,
    [DataValues]       VARCHAR (500) NULL,
    [Required]         CHAR (1)      NULL,
    [PrePopulate]      CHAR (1)      NULL,
    [SortOrder]        INT           NULL,
    [CreatedDate]      DATETIME      CONSTRAINT [DF_SmartFormsPrePopFields_CREATEDDATE] DEFAULT (getdate()) NULL,
    [UpdatedDate]      DATETIME      NULL,
    [CreatedUserID]    INT           NULL,
    [IsDeleted]        BIT           CONSTRAINT [DF_SmartFormsPrePopFields_IsDeleted] DEFAULT ((0)) NULL,
    [UpdatedUserID]    INT           NULL,
    CONSTRAINT [PK_SmartFormsPrePopFields] PRIMARY KEY CLUSTERED ([PrePopFieldID] ASC) WITH (FILLFACTOR = 80)
);

