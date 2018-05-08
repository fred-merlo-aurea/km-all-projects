CREATE TABLE [dbo].[LinkTrackingParam] (
    [LTPID]       INT          IDENTITY (1, 1) NOT NULL,
    [LTID]        INT          NULL,
    [DisplayName] VARCHAR (50) NULL,
    [IsActive]    BIT          NULL,
    CONSTRAINT [PK_LinkTrackingParam] PRIMARY KEY CLUSTERED ([LTPID] ASC) WITH (FILLFACTOR = 80)
);

