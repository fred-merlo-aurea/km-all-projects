CREATE TABLE [dbo].[CANON_PAIDPUB_Forms] (
    [FormID]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [Description]    TEXT          NULL,
    [Code]           VARCHAR (50)  NOT NULL,
    [CustomerID]     INT           NOT NULL,
    [HeaderHTML]     TEXT          NULL,
    [FooterHTML]     TEXT          NULL,
    [newsletterHTML] TEXT          NULL,
    [GroupIDs]       VARCHAR (200) NOT NULL,
    [IsActive]       BIT           CONSTRAINT [DF_CANON_PAIDPUB_Forms_IsActive] DEFAULT (1) NOT NULL,
    [IsTrial]        BIT           CONSTRAINT [DF_CANON_PAIDPUB_Forms_IsTrial] DEFAULT (0) NULL,
    CONSTRAINT [PK_CANON_PAIDPUB_Forms] PRIMARY KEY CLUSTERED ([FormID] ASC) WITH (FILLFACTOR = 80)
);

