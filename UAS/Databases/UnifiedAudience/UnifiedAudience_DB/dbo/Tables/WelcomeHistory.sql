CREATE TABLE [dbo].[WelcomeHistory] (
    [BrandCode]     VARCHAR (50)  NOT NULL,
    [EmailAddress]  VARCHAR (100) NOT NULL,
    [EmailSentDate] DATETIME      NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_WelcomeHistory_Brandcode_Emailaddress]
    ON [dbo].[WelcomeHistory]([BrandCode] ASC, [EmailAddress] ASC);

