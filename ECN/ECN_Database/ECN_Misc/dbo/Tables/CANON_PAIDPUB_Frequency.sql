CREATE TABLE [dbo].[CANON_PAIDPUB_Frequency] (
    [FrequencyID] INT          IDENTITY (1, 1) NOT NULL,
    [Frequency]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Frequency] PRIMARY KEY CLUSTERED ([FrequencyID] ASC) WITH (FILLFACTOR = 80)
);

