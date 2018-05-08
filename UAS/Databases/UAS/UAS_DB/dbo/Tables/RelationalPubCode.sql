CREATE TABLE [RelationalPubCode](
	[ClientID] [int] NOT NULL,
	[SpecialFileName] [varchar](250) NOT NULL,
	[RelationalFileName] [varchar](250) NULL,
	[PubCode] [varchar](50) NULL,
	[CodeID] [varchar](250) NOT NULL,
	[CodeDateType] [varchar](50) NOT NULL,
	[FieldName] [varchar](100) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NULL,
	[CreatedByUserID] [int] NOT NULL,
	[UpdatedByUserID] [int] NULL
) 

GO
/****** Object:  Index [IDX_RPC_ClientID]    Script Date: 06/25/2014 05:44:48 ******/
CREATE NONCLUSTERED INDEX [IDX_RPC_ClientID] ON [RelationalPubCode] 
(
	[ClientID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
GO
/****** Object:  Index [IDX_RPC_RelationalFileName]    Script Date: 06/25/2014 05:45:06 ******/
CREATE NONCLUSTERED INDEX [IDX_RPC_RelationalFileName] ON [RelationalPubCode] 
(
	[RelationalFileName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
GO
/****** Object:  Index [IDX_RPC_SpecialFileName]    Script Date: 06/25/2014 05:45:20 ******/
CREATE NONCLUSTERED INDEX [IDX_RPC_SpecialFileName] ON [RelationalPubCode] 
(
	[SpecialFileName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) 
GO



