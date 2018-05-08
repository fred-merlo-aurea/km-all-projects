CREATE TABLE [dbo].[LinkTracking] (
    [LTID]        INT          IDENTITY (1, 1) NOT NULL,
    [DisplayName] VARCHAR (50) NULL,
    [IsActive]    BIT          NULL,
    CONSTRAINT [PK_LinkTracking] PRIMARY KEY CLUSTERED ([LTID] ASC) WITH (FILLFACTOR = 80)
);

