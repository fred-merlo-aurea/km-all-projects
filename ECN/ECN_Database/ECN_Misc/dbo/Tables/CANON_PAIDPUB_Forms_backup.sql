CREATE TABLE [dbo].[CANON_PAIDPUB_Forms_backup] (
    [FormID]         INT           IDENTITY (1, 1) NOT NULL,
    [Name]           VARCHAR (50)  NOT NULL,
    [Description]    TEXT          NULL,
    [Code]           VARCHAR (50)  NOT NULL,
    [CustomerID]     INT           NOT NULL,
    [HeaderHTML]     TEXT          NULL,
    [FooterHTML]     TEXT          NULL,
    [newsletterHTML] TEXT          NULL,
    [GroupIDs]       VARCHAR (200) NOT NULL,
    [IsActive]       BIT           NOT NULL,
    [IsTrial]        BIT           NULL
);

