CREATE TABLE [dbo].[BlastActivityConversion] (
    [ConversionID]   INT            IDENTITY (1, 1) NOT NULL,
    [BlastID]        INT            NOT NULL,
    [EmailID]        INT            NOT NULL,
    [ConversionTime] DATETIME       NOT NULL,
    [URL]            VARCHAR (2048) NULL,
    [EAID]           INT            NULL,
    CONSTRAINT [PK_BlastActivityConversion] PRIMARY KEY CLUSTERED ([ConversionID] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityConversion_BlastID]
    ON [dbo].[BlastActivityConversion]([BlastID] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IDX_BlastActivityConversion_EmailID]
    ON [dbo].[BlastActivityConversion]([EmailID] ASC) WITH (FILLFACTOR = 80);

