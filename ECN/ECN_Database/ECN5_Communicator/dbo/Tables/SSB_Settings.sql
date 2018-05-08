CREATE TABLE [dbo].[SSB_Settings] (
    [Source]        [sysname]        NOT NULL,
    [Destination]   [sysname]        NOT NULL,
    [Contract]      [sysname]        NOT NULL,
    [dialog_handle] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_SSB_Setting] PRIMARY KEY CLUSTERED ([Source] ASC, [Destination] ASC, [Contract] ASC) WITH (FILLFACTOR = 80)
);

