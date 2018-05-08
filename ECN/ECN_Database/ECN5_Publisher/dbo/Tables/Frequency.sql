CREATE TABLE [dbo].[Frequency] (
    [FrequencyID] INT          IDENTITY (1, 1) NOT NULL,
    [Frequency]   VARCHAR (50) NOT NULL,
    [IsDeleted]   BIT          CONSTRAINT [DF_Frequency_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Frequency] PRIMARY KEY CLUSTERED ([FrequencyID] ASC) WITH (FILLFACTOR = 80)
);

