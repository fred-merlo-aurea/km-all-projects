CREATE TABLE [dbo].[Form] (
	[Form_Seq_ID] [int] IDENTITY(1,1) NOT NULL,
	[TokenUID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ActivationDateFrom] [datetime] NULL,
	[ActivationDateTo] [datetime] NULL,
	[LastUpdated] [datetime] NULL,
	[LastPublished] [datetime] NULL,
	[OptInType] [int] NOT NULL,
	[CssUri] [nvarchar](200) NULL,
	[HeaderHTML] [nvarchar](max) NULL,
	[FooterHTML] [nvarchar](max) NULL,
	[UpdatedBy] [nvarchar](100) NULL,
	[CustomerName] [nvarchar](100) NOT NULL,
	[Active] [int] NOT NULL,
    [SubmitButtonText] [nvarchar](50) NOT NULL,
	[ParentForm_ID] [int] NULL,
	[CssFile_Seq_ID] [int] NULL,
	[GroupID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[StylingType] [int] NOT NULL,
	[CustomerAccessKey] [nvarchar](50) NOT NULL,
	[PublishAfter] [datetime] NULL,
	[LanguageTranslationType] [bit] NOT NULL,
	[Iframe] [bit] NOT NULL,
	[Delay] [int] NOT NULL,
	[FormType] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[HeaderJs] NVARCHAR(MAX) NULL, 
    [FooterJs] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_Form] PRIMARY KEY CLUSTERED ([Form_Seq_ID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_CssFile_Form] FOREIGN KEY ([CssFile_Seq_ID]) REFERENCES [dbo].[CssFile] ([CssFile_Seq_ID]),
    CONSTRAINT [FK_Form_Form] FOREIGN KEY ([ParentForm_ID]) REFERENCES [dbo].[Form] ([Form_Seq_ID]),
    CONSTRAINT [UQ_Form_TokenUID] UNIQUE NONCLUSTERED ([TokenUID] ASC, [ParentForm_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [NI_Form_TokenUID]
       ON [dbo].[Form]([TokenUID] ASC) WITH (FILLFACTOR = 80);

