CREATE TABLE [dbo].[Suppressed] (
    [STRecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [SFRecordIdentifier] UNIQUEIDENTIFIER NOT NULL,
    [Source]             VARCHAR (250)    NOT NULL,
    [IsSuppressed]       BIT              NOT NULL,
    [IsEmailMatch]       BIT              NOT NULL,
    [IsPhoneMatch]       BIT              NOT NULL,
    [IsAddressMatch]     BIT              NOT NULL,
    [IsCompanyMatch]     BIT              NOT NULL,
    [ProcessCode]        VARCHAR (50)     NULL,
    [DateCreated]        DATETIME         CONSTRAINT [DF_Suppressed_DateCreated] DEFAULT (getdate()) NOT NULL,
    [DateUpdated]        DATETIME         NULL,
    [CreatedByUserID]    INT              NOT NULL,
    [UpdatedByUserID]    INT              NULL,
    CONSTRAINT [PK__Suppressed] PRIMARY KEY CLUSTERED ([STRecordIdentifier] ASC, [SFRecordIdentifier] ASC) WITH (FILLFACTOR = 90)
);
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_suppressed_by_SFRecordIentifier] ON [dbo].[Suppressed] 
(
      [SFRecordIdentifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



