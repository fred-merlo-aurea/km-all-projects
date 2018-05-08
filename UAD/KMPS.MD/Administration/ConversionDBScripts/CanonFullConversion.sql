--backup CanonMasterDB from .198 and archive it
--copy the archive to .251
--delete the CanonMasterDB from .251
--restore the archive to CanonMasterDB
--change properties>options recovery model to simple

--create master groups table
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MasterGroups](
	[MasterGroupID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](100) NULL,
	[DisplayName] [varchar](50) NULL,
	[ColumnReference] [varchar](50) NULL,
	[SortOrder] [int] NULL,
	[IsActive] [bit] NULL,
	[EnableSubReporting] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO

--set initial data in master groups
INSERT INTO MasterGroups (DisplayName)
SELECT DISTINCT ResponseGroup from Mastercodesheet

UPDATE MasterGroups
SET SortOrder = MasterGroupID, 
	Name = 'Master_' + DisplayName,
	Description = DisplayName + ' description',
	ColumnReference =  'Master_' + DisplayName,
	IsActive = 1,
	EnableSubReporting = 1

--add MasterGroupID to Mastercodesheet
ALTER TABLE Mastercodesheet
ADD MasterGroupID int NULL 
GO 

--link Mastercodesheet to MasterGroup
update Mastercodesheet
    set Mastercodesheet.MasterGroupID = MasterGroups.MasterGroupID
    from Mastercodesheet, MasterGroups
    where Mastercodesheet.ResponseGroup = MasterGroups.DisplayName

--add PubCode to Pubs
ALTER TABLE Pubs
ADD PubCode VARCHAR(50) NULL 
GO 

--populate pub code
update pubs set pubcode='3C08' where pubID = 1
update pubs set pubcode='3C09' where pubID = 2
update pubs set pubcode='3C10' where pubID = 3
update pubs set pubcode='ADE07' where pubID = 6
update pubs set pubcode='ADE08' where pubID = 7
update pubs set pubcode='ADE09' where pubID = 8
update pubs set pubcode='ADE10' where pubID = 9
update pubs set pubcode='AMEX06' where pubID = 11
update pubs set pubcode='APP17865' where pubID = 12
update pubs set pubcode='APP19972' where pubID = 13
update pubs set pubcode='APPL' where pubID = 14
update pubs set pubcode='ASNE07' where pubID = 16
update pubs set pubcode='ASNE08' where pubID = 17
update pubs set pubcode='ASST06' where pubID = 18
update pubs set pubcode='ASST07' where pubID = 19
update pubs set pubcode='ASST08' where pubID = 20
update pubs set pubcode='ASST09' where pubID = 21
update pubs set pubcode='ATXE07' where pubID = 24
update pubs set pubcode='ATXE08' where pubID = 25
update pubs set pubcode='ATXE09' where pubID = 26
update pubs set pubcode='ATXE10' where pubID = 27
update pubs set pubcode='ATXS07' where pubID = 28
update pubs set pubcode='ATXS08' where pubID = 29
update pubs set pubcode='ATXS09' where pubID = 30
update pubs set pubcode='ATXS10' where pubID = 31
update pubs set pubcode='ATXW07' where pubID = 33
update pubs set pubcode='ATXW08' where pubID = 34
update pubs set pubcode='ATXW09' where pubID = 35
update pubs set pubcode='ATXW10' where pubID = 36
update pubs set pubcode='BIOB09' where pubID = 37
update pubs set pubcode='BIOB10' where pubID = 38
update pubs set pubcode='BIOM07' where pubID = 39
update pubs set pubcode='BIOM08' where pubID = 40
update pubs set pubcode='BIOM09' where pubID = 41
update pubs set pubcode='CPCK09' where pubID = 42
update pubs set pubcode='CPCP' where pubID = 43
update pubs set pubcode='DDS09' where pubID = 44
update pubs set pubcode='DES36233' where pubID = 45
update pubs set pubcode='DES36234' where pubID = 46
update pubs set pubcode='DES36235' where pubID = 47
update pubs set pubcode='DES36236' where pubID = 48
update pubs set pubcode='DES36237' where pubID = 49
update pubs set pubcode='DES36238' where pubID = 50
update pubs set pubcode='DES36239' where pubID = 51
update pubs set pubcode='DES36240' where pubID = 52
update pubs set pubcode='DES36241' where pubID = 53
update pubs set pubcode='DES36242' where pubID = 54
update pubs set pubcode='DES36243' where pubID = 55
update pubs set pubcode='DESN' where pubID = 56
update pubs set pubcode='DMNE09' where pubID = 59
update pubs set pubcode='DMNE10' where pubID = 60
update pubs set pubcode='EAST07' where pubID = 63
update pubs set pubcode='EAST08' where pubID = 64
update pubs set pubcode='EAST09' where pubID = 65
update pubs set pubcode='EAST10' where pubID = 66
update pubs set pubcode='EDN' where pubID = 67
update pubs set pubcode='EDN35456' where pubID = 68
update pubs set pubcode='EDN35457' where pubID = 69
update pubs set pubcode='EDN35458' where pubID = 70
update pubs set pubcode='EDN35459' where pubID = 71
update pubs set pubcode='EDN35460' where pubID = 72
update pubs set pubcode='EDN35461' where pubID = 73
update pubs set pubcode='EDN35462' where pubID = 74
update pubs set pubcode='EDN35463' where pubID = 75
update pubs set pubcode='EDN35465' where pubID = 76
update pubs set pubcode='EDN35466' where pubID = 77
update pubs set pubcode='EDN35467' where pubID = 78
update pubs set pubcode='EDN35468' where pubID = 79
update pubs set pubcode='EDN38650' where pubID = 80
update pubs set pubcode='ELCW07' where pubID = 84
update pubs set pubcode='ELCW08' where pubID = 85
update pubs set pubcode='ELCW09' where pubID = 86
update pubs set pubcode='ELCW10' where pubID = 87
update pubs set pubcode='ELNE09' where pubID = 88
update pubs set pubcode='ELNE10' where pubID = 89
update pubs set pubcode='EMD15282' where pubID = 90
update pubs set pubcode='EMD17868' where pubID = 91
update pubs set pubcode='EMD17877' where pubID = 92
update pubs set pubcode='EMD21143' where pubID = 93
update pubs set pubcode='EMD24185' where pubID = 94
update pubs set pubcode='EMDT' where pubID = 96
update pubs set pubcode='EMT18587' where pubID = 98
update pubs set pubcode='EPCK07' where pubID = 102
update pubs set pubcode='EPCK08' where pubID = 103
update pubs set pubcode='EPCK09' where pubID = 104
update pubs set pubcode='EPCK10' where pubID = 105
update pubs set pubcode='GRNE08' where pubID = 106
update pubs set pubcode='GRNE09' where pubID = 107
update pubs set pubcode='GRNE10' where pubID = 108
update pubs set pubcode='GRNM08' where pubID = 109
update pubs set pubcode='GRNM09' where pubID = 110
update pubs set pubcode='GRNS08' where pubID = 111
update pubs set pubcode='GRNS09' where pubID = 112
update pubs set pubcode='GRNS10' where pubID = 113
update pubs set pubcode='GRNU09' where pubID = 114
update pubs set pubcode='GRNW08' where pubID = 115
update pubs set pubcode='GRNW09' where pubID = 116
update pubs set pubcode='GRNW10' where pubID = 117
update pubs set pubcode='IMM' where pubID = 119
update pubs set pubcode='IMM18049' where pubID = 120
update pubs set pubcode='IMM18050' where pubID = 121
update pubs set pubcode='IMM18361' where pubID = 122
update pubs set pubcode='IPOT08' where pubID = 124
update pubs set pubcode='ITSF09' where pubID = 125
update pubs set pubcode='IVD' where pubID = 126
update pubs set pubcode='IVD17866' where pubID = 127
update pubs set pubcode='IVD23228' where pubID = 128
update pubs set pubcode='IVD39024' where pubID = 129
update pubs set pubcode='MB08' where pubID = 131
update pubs set pubcode='MB09' where pubID = 132
update pubs set pubcode='MB10' where pubID = 133
update pubs set pubcode='MDD17870' where pubID = 134
update pubs set pubcode='MDD17871' where pubID = 135
update pubs set pubcode='MDD17879' where pubID = 136
update pubs set pubcode='MDD18204' where pubID = 137
update pubs set pubcode='MDD18261' where pubID = 138
update pubs set pubcode='MDD31047' where pubID = 139
update pubs set pubcode='MDDI' where pubID = 140
update pubs set pubcode='MDE09' where pubID = 142
update pubs set pubcode='MDMC08' where pubID = 144
update pubs set pubcode='MDMC09' where pubID = 145
update pubs set pubcode='MDPK06' where pubID = 148
update pubs set pubcode='MDPK07' where pubID = 149
update pubs set pubcode='MDPK08' where pubID = 150
update pubs set pubcode='MDPK09' where pubID = 151
update pubs set pubcode='MDT07' where pubID = 154
update pubs set pubcode='MDT08' where pubID = 155
update pubs set pubcode='MDT09' where pubID = 156
update pubs set pubcode='MDT10' where pubID = 157
update pubs set pubcode='MEAN' where pubID = 158
update pubs set pubcode='MEM' where pubID = 159
update pubs set pubcode='MEM18474' where pubID = 160
update pubs set pubcode='MEM21624' where pubID = 161
update pubs set pubcode='MINN06' where pubID = 164
update pubs set pubcode='MINN07' where pubID = 165
update pubs set pubcode='MINN08' where pubID = 166
update pubs set pubcode='MINN09' where pubID = 167
update pubs set pubcode='MOPL' where pubID = 168
update pubs set pubcode='MPM18510' where pubID = 169
update pubs set pubcode='MPM18511' where pubID = 170
update pubs set pubcode='MPM21724' where pubID = 171
update pubs set pubcode='MPM21726' where pubID = 172
update pubs set pubcode='MPMN' where pubID = 173
update pubs set pubcode='MPW18362' where pubID = 175
update pubs set pubcode='MPW18364' where pubID = 176
update pubs set pubcode='MPW31695' where pubID = 177
update pubs set pubcode='MTEC08' where pubID = 178
update pubs set pubcode='MTEC09' where pubID = 179
update pubs set pubcode='MTEC10' where pubID = 180
update pubs set pubcode='MTFR09' where pubID = 181
update pubs set pubcode='MTFR10' where pubID = 182
update pubs set pubcode='MTIR06' where pubID = 184
update pubs set pubcode='MTIR07' where pubID = 185
update pubs set pubcode='MTIR08' where pubID = 186
update pubs set pubcode='MTIR09' where pubID = 187
update pubs set pubcode='MTP17867' where pubID = 188
update pubs set pubcode='MTP18512' where pubID = 189
update pubs set pubcode='MTST08' where pubID = 193
update pubs set pubcode='MTST09' where pubID = 194
update pubs set pubcode='MTST10' where pubID = 195
update pubs set pubcode='MTTC09' where pubID = 196
update pubs set pubcode='MX' where pubID = 198
update pubs set pubcode='MX17876' where pubID = 199
update pubs set pubcode='NEPE07' where pubID = 201
update pubs set pubcode='NEPE08' where pubID = 202
update pubs set pubcode='NMWS07' where pubID = 203
update pubs set pubcode='NMWS08' where pubID = 204
update pubs set pubcode='NO18095' where pubID = 205
update pubs set pubcode='NO18096' where pubID = 206
update pubs set pubcode='NO36950' where pubID = 207
update pubs set pubcode='NUTR' where pubID = 209
update pubs set pubcode='OEMB06' where pubID = 211
update pubs set pubcode='OEMB07' where pubID = 212
update pubs set pubcode='OEMB08' where pubID = 213
update pubs set pubcode='ORIN10' where pubID = 214
update pubs set pubcode='ORT36013' where pubID = 215
update pubs set pubcode='ORTH' where pubID = 216
update pubs set pubcode='PAAM' where pubID = 217
update pubs set pubcode='PBS' where pubID = 219
update pubs set pubcode='PBS18410' where pubID = 220
update pubs set pubcode='PBS18507' where pubID = 221
update pubs set pubcode='PBS18731' where pubID = 222
update pubs set pubcode='PBSE07' where pubID = 223
update pubs set pubcode='PDE07' where pubID = 227
update pubs set pubcode='PDE08' where pubID = 228
update pubs set pubcode='PDE09' where pubID = 229
update pubs set pubcode='PDE10' where pubID = 230
update pubs set pubcode='PES09' where pubID = 231
update pubs set pubcode='PHAR' where pubID = 232
update pubs set pubcode='PHL14092' where pubID = 233
update pubs set pubcode='PHL14093' where pubID = 234
update pubs set pubcode='PHL14094' where pubID = 235
update pubs set pubcode='PHL14095' where pubID = 236
update pubs set pubcode='PHL14096' where pubID = 237
update pubs set pubcode='PHL14097' where pubID = 238
update pubs set pubcode='PHL14098' where pubID = 239
update pubs set pubcode='PHL14100' where pubID = 240
update pubs set pubcode='PHL14101' where pubID = 241
update pubs set pubcode='PHL14102' where pubID = 242
update pubs set pubcode='PHL14103' where pubID = 243
update pubs set pubcode='PHL14104' where pubID = 244
update pubs set pubcode='PHL14105' where pubID = 245
update pubs set pubcode='PHL16201' where pubID = 246
update pubs set pubcode='PHL16585' where pubID = 247
update pubs set pubcode='PHL34851' where pubID = 248
update pubs set pubcode='PHMD08' where pubID = 250
update pubs set pubcode='PHMD09' where pubID = 251
update pubs set pubcode='PHPK09' where pubID = 252
update pubs set pubcode='PKG35735' where pubID = 253
update pubs set pubcode='PKG35736' where pubID = 254
update pubs set pubcode='PKG35737' where pubID = 255
update pubs set pubcode='PKG35738' where pubID = 256
update pubs set pubcode='PKG35740' where pubID = 257
update pubs set pubcode='PKG36739' where pubID = 258
update pubs set pubcode='PKGD' where pubID = 259
update pubs set pubcode='PMP18097' where pubID = 264
update pubs set pubcode='PMP18098' where pubID = 265
update pubs set pubcode='PMP18100' where pubID = 266
update pubs set pubcode='PMP18159' where pubID = 267
update pubs set pubcode='PMP22547' where pubID = 268
update pubs set pubcode='PMP22548' where pubID = 269
update pubs set pubcode='PMP22549' where pubID = 270
update pubs set pubcode='PMP22906' where pubID = 271
update pubs set pubcode='PMP32332' where pubID = 272
update pubs set pubcode='PTCC07' where pubID = 273
update pubs set pubcode='PTCE07' where pubID = 275
update pubs set pubcode='PTCE08' where pubID = 276
update pubs set pubcode='PTCE10' where pubID = 277
update pubs set pubcode='PTCM07' where pubID = 278
update pubs set pubcode='PTCM08' where pubID = 279
update pubs set pubcode='PTCS07' where pubID = 280
update pubs set pubcode='PTCS08' where pubID = 281
update pubs set pubcode='PTCS09' where pubID = 282
update pubs set pubcode='PTCS10' where pubID = 283
update pubs set pubcode='PTCW07' where pubID = 286
update pubs set pubcode='PTCW08' where pubID = 287
update pubs set pubcode='PTCW09' where pubID = 288
update pubs set pubcode='PTCW10' where pubID = 289
update pubs set pubcode='PTN21809' where pubID = 290
update pubs set pubcode='PTXI08' where pubID = 292
update pubs set pubcode='PTXI10' where pubID = 293
update pubs set pubcode='PTXS08' where pubID = 294
update pubs set pubcode='PTXS09' where pubID = 295
update pubs set pubcode='PTXW08' where pubID = 296
update pubs set pubcode='PTXW09' where pubID = 297
update pubs set pubcode='PV08' where pubID = 298
update pubs set pubcode='PV09' where pubID = 299
update pubs set pubcode='PV10' where pubID = 300
update pubs set pubcode='QLDT08' where pubID = 301
update pubs set pubcode='QLTS10' where pubID = 302
update pubs set pubcode='QLTY07' where pubID = 304
update pubs set pubcode='QLTY09' where pubID = 305
update pubs set pubcode='QME36447' where pubID = 306
update pubs set pubcode='QME38943' where pubID = 307
update pubs set pubcode='RD' where pubID = 308
update pubs set pubcode='SDE07' where pubID = 309
update pubs set pubcode='SDE08' where pubID = 310
update pubs set pubcode='SDE09' where pubID = 311
update pubs set pubcode='SDE10' where pubID = 312
update pubs set pubcode='SPCK07' where pubID = 314
update pubs set pubcode='SPCK08' where pubID = 315
update pubs set pubcode='SPCK09' where pubID = 316
update pubs set pubcode='SPCK10' where pubID = 317
update pubs set pubcode='SUD09' where pubID = 318
update pubs set pubcode='SUD10' where pubID = 319
update pubs set pubcode='TMW' where pubID = 320
update pubs set pubcode='TMW35544' where pubID = 321
update pubs set pubcode='TMW35545' where pubID = 322
update pubs set pubcode='TMW35546' where pubID = 323
update pubs set pubcode='TMW35547' where pubID = 324
update pubs set pubcode='TMW35548' where pubID = 325
update pubs set pubcode='TMW35549' where pubID = 326
update pubs set pubcode='TMW35707' where pubID = 327
update pubs set pubcode='TMWEN' where pubID = 328
update pubs set pubcode='VTX09' where pubID = 329
update pubs set pubcode='VTX10' where pubID = 330
update pubs set pubcode='WEST07' where pubID = 333
update pubs set pubcode='WEST08' where pubID = 334
update pubs set pubcode='WEST09' where pubID = 335
update pubs set pubcode='WEST10' where pubID = 336
update pubs set pubcode='WPCK07' where pubID = 339
update pubs set pubcode='WPCK08' where pubID = 340
update pubs set pubcode='WPCK09' where pubID = 341
update pubs set pubcode='WPCK10' where pubID = 342

--create PubTypes table
CREATE TABLE [PubTypes](
	[PubTypeID] [int] IDENTITY(1,1) NOT NULL,
	[PubTypeDisplayName] [varchar](50) NOT NULL,
	[ColumnReference] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL
) ON [PRIMARY]
GO

--populate PubTypes table
INSERT INTO PubTypes (PubTypeDisplayName, ColumnReference, IsActive, SortOrder)
	VALUES ('EPROD', 'EPROD', 1, 1)
INSERT INTO PubTypes (PubTypeDisplayName, ColumnReference, IsActive, SortOrder)
	VALUES ('PUB', 'PUB', 1, 2)
INSERT INTO PubTypes (PubTypeDisplayName, ColumnReference, IsActive, SortOrder)
	VALUES ('SHOW', 'SHOW', 1, 3)
	
--add PubTypeID to Pubs
ALTER TABLE Pubs
ADD PubTypeID INT NULL 
GO 

--set PubTypeID from PubType
UPDATE Pubs 
SET PubTypeID = 1
WHERE PubType = 'EPROD'
GO
UPDATE Pubs 
SET PubTypeID = 2
WHERE PubType = 'PUB'
GO
UPDATE Pubs 
SET PubTypeID = 3
WHERE PubType = 'SHOW'
GO
	
--drop PubType from Pubs
ALTER TABLE Pubs
DROP Column PubType
GO 

--create ResponseGroups table
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResponseGroups](
	[ResponseGroupID] [int] IDENTITY(1,1) NOT NULL,
	[PubID] [int] NULL,
	[ResponseGroupName] [varchar](100) NOT NULL,
	[DisplayName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ResponseGroups] PRIMARY KEY CLUSTERED 
(
	[ResponseGroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[ResponseGroups]  WITH CHECK ADD  CONSTRAINT [FK_ResponseGroups_Pubs] FOREIGN KEY([PubID])
REFERENCES [dbo].[Pubs] ([PubID])
GO
ALTER TABLE [dbo].[ResponseGroups] CHECK CONSTRAINT [FK_ResponseGroups_Pubs]
GO

--populate ResponseGroups table
INSERT INTO ResponseGroups (PubID, ResponseGroupName, DisplayName)
	SELECT DISTINCT cs.pubid,responsegroup, responsegroup 
	FROM CodeSheet cs WHERE cs.PubID in (SELECT p.PubID FROM Pubs p)
	
--add PubTypeID to Pubs
ALTER TABLE CodeSheet
ADD ResponseGroupID INT NULL 
GO 

--set ResponseGroupID from ResponseGroups
update CodeSheet
    set CodeSheet.ResponseGroupID = ResponseGroups.ResponseGroupID
    from CodeSheet, ResponseGroups
    where CodeSheet.PubID = ResponseGroups.PubID and CodeSheet.ResponseGroup=ResponseGroups.ResponseGroupName

--create CodeSheet Mastercodesheet bridge table
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CodeSheet_Mastercodesheet_Bridge](
	[CodeSheetID] [int] NOT NULL,
	[MasterID] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CodeSheet_Mastercodesheet_Bridge]  WITH CHECK ADD  CONSTRAINT [FK_CodeSheet_Mastercodesheet_Bridge_CodeSheet] FOREIGN KEY([CodeSheetID])
REFERENCES [dbo].[CodeSheet] ([CodeSheetID])
GO
ALTER TABLE [dbo].[CodeSheet_Mastercodesheet_Bridge] CHECK CONSTRAINT [FK_CodeSheet_Mastercodesheet_Bridge_CodeSheet]
GO
ALTER TABLE [dbo].[CodeSheet_Mastercodesheet_Bridge]  WITH CHECK ADD  CONSTRAINT [FK_CodeSheet_Mastercodesheet_Bridge_Mastercodesheet] FOREIGN KEY([MasterID])
REFERENCES [dbo].[Mastercodesheet] ([MasterID])
GO
ALTER TABLE [dbo].[CodeSheet_Mastercodesheet_Bridge] CHECK CONSTRAINT [FK_CodeSheet_Mastercodesheet_Bridge_Mastercodesheet]
GO

--populate CodeSheet_Mastercodesheet_Bridge table
INSERT INTO CodeSheet_Mastercodesheet_Bridge (CodeSheetID, MasterID)
SELECT DISTINCT CodeSheetID,MasterID 
	FROM CodeSheet
	
--drop foreign key on CodeSheet to Mastercodesheet
ALTER TABLE CodeSheet
DROP CONSTRAINT FK_CodeSheet_Mastercodesheet
GO 

--drop MasterID from CodeSheet
ALTER TABLE CodeSheet
DROP Column MasterID
GO 

--create table pubsubscriptions
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PubSubscriptions](
	[PubSubscriptionID] [int] IDENTITY(1,1) NOT NULL,
	[SubscriptionID] [int] NOT NULL,
	[PubID] [int] NOT NULL,
	[demo7] [varchar](1) NULL,
 CONSTRAINT [PK_PubSubscriptions] PRIMARY KEY CLUSTERED 
(
	[PubSubscriptionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[PubSubscriptions]  WITH CHECK ADD  CONSTRAINT [FK_PubSubscriptions_Pubs] FOREIGN KEY([PubID])
REFERENCES [dbo].[Pubs] ([PubID])
GO
ALTER TABLE [dbo].[PubSubscriptions] CHECK CONSTRAINT [FK_PubSubscriptions_Pubs]
GO
ALTER TABLE [dbo].[PubSubscriptions]  WITH CHECK ADD  CONSTRAINT [FK_PubSubscriptions_Subscriptions] FOREIGN KEY([SubscriptionID])
REFERENCES [dbo].[Subscriptions] ([SubscriptionID])
GO
ALTER TABLE [dbo].[PubSubscriptions] CHECK CONSTRAINT [FK_PubSubscriptions_Subscriptions]
GO

--create CodeSheet insert stored proc
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spSaveCodeSheet]
(
	@CodeSheetID int = 0, 
	@PubID int, 
	@ResponseGroupID int, 
	@ResponseGroup varchar(255), 
	@ResponseValue varchar(255), 
	@ResponseDesc varchar(255),
	@xmlDocument Text
)
as
Begin
	set nocount on    	  
	DECLARE @docHandle int
	DECLARE @newID int    
	  
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xmlDocument 
	
	if (@CodeSheetID =0)
		Begin
			INSERT INTO [CodeSheet]([PubID], [ResponseGroupID], [ResponseGroup], [ResponseValue], [ResponseDesc]) 
				VALUES (@PubID, @ResponseGroupID, @ResponseGroup, @ResponseValue, @ResponseDesc)
			
			SELECT @newID = SCOPE_IDENTITY()
			
			INSERT INTO CodeSheet_Mastercodesheet_Bridge (CodeSheetID, MasterID) 
				SELECT @newID, MasterID FROM OPENXML(@docHandle, N'/ROOT/RECORD')
				WITH (MasterID INT '@MasterID') 
		End
	Else
		Begin
			update CodeSheet
			set ResponseGroupID = @ResponseGroupID, 
				ResponseGroup = @ResponseGroup, 
				ResponseValue = @ResponseValue, 
				ResponseDesc = @ResponseDesc
			where CodeSheetID = @CodeSheetID
				
			delete CodeSheet_Mastercodesheet_Bridge
			where  CodeSheetID = @CodeSheetID
			
			INSERT INTO CodeSheet_Mastercodesheet_Bridge (CodeSheetID, MasterID) 
				SELECT @CodeSheetID, MasterID FROM OPENXML(@docHandle, N'/ROOT/RECORD')
				WITH (MasterID INT '@MasterID')						
		End	
End
GO

--create Mastercodesheet insert stored proc
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spSaveMasterCodeSheet]
(
	@MasterID int = 0, 
	@MasterGroupID int, 
	@MasterGroup varchar(100), 
	@MasterValue varchar(100), 
	@MasterDesc varchar(255)
)
as
Begin
	set nocount on    
	  
	if (@MasterID =0)
		Begin
			INSERT INTO [Mastercodesheet]([MasterGroupID], [MasterGroup], [MasterValue], [MasterDesc]) 
				VALUES (@MasterGroupID, @MasterGroup, @MasterValue, @MasterDesc)
		End
	Else
		Begin
			update Mastercodesheet
			set MasterGroupID = @MasterGroupID, 
				MasterGroup = @MasterGroup, 
				MasterValue = @MasterValue, 
				MasterDesc = @MasterDesc
			where MasterID = @MasterID						
		End	
End
GO

--create MasterGroups insert trigger
CREATE TRIGGER [dbo].[trMasterGroupsInsert]
   ON [dbo].[MasterGroups] 
   AFTER INSERT 
AS  
BEGIN 
   SET NOCOUNT ON;  
   UPDATE MasterGroups  
   SET SortOrder = (Select max(SortOrder) + 1 From MasterGroups) 
   FROM INSERTED AS I 
   WHERE MasterGroups.MasterGroupID = I.MasterGroupID 
END 
GO

--create stored proc for reports
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetAdminReport] 
 @OrderBy varchar(255)  
AS
BEGIN	
	if (@OrderBy ='PRODUCT')
		Begin
			select p.pubID as "ProductID", p.PubName as "ProductName", rg.DisplayName as "ResponseGroup", cs.Responsevalue as "ResponseGroupValue", 
			  cs.Responsedesc as "ResponseGroupDesc", mg.Name as "MasterGroup", mcs.MasterValue as "MasterCodeSheetValue", 
			  mcs.MasterDesc as "MasterCodeSheetDesc"
			from Pubs p
				  inner join ResponseGroups rg on p.PubID = rg.PubID
				  inner join CodeSheet cs on rg.ResponseGroupID = cs.ResponseGroupID
				  inner join CodeSheet_Mastercodesheet_Bridge cmb on cs.CodeSheetID = cmb.CodeSheetID
				  inner join Mastercodesheet mcs on cmb.MasterID = mcs.MasterID
				  inner join MasterGroups mg on mcs.MasterGroupID = mg.MasterGroupID
			order by ProductID, ResponseGroup, ResponseGroupValue
		End
	Else
		Begin
			select p.pubID as "ProductID", p.PubName as "ProductName", rg.DisplayName as "ResponseGroup", cs.Responsevalue as "ResponseGroupValue", 
			  cs.Responsedesc as "ResponseGroupDesc", mg.Name as "MasterGroup", mcs.MasterValue as "MasterCodeSheetValue", 
			  mcs.MasterDesc as "MasterCodeSheetDesc"
			from Pubs p
				  inner join ResponseGroups rg on p.PubID = rg.PubID
				  inner join CodeSheet cs on rg.ResponseGroupID = cs.ResponseGroupID
				  inner join CodeSheet_Mastercodesheet_Bridge cmb on cs.CodeSheetID = cmb.CodeSheetID
				  inner join Mastercodesheet mcs on cmb.MasterID = mcs.MasterID
				  inner join MasterGroups mg on mcs.MasterGroupID = mg.MasterGroupID
			order by MasterGroup, MasterCodeSheetValue, ProductName, ResponseGroup						
		End	
END
GO

--alter function for getting pubs for market
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER function [dbo].[fnGetPubsForMarket](@MarketID int)
RETURNS VARCHAR(8000)
AS
BEGIN
	Declare @dv varchar(8000)
	set @dv = ''
	
	select @dv= @dv + p.PubName + ', ' 
	from Pubs p 
		join pubmarkets pm on p.pubID = pm.pubID 
		join PubTypes pt on p.PubTypeID = pt.PubTypeID
	where MarketID = @MarketID 
	order by pt.PubTypeDisplayName, p.pubname
	
	select @dv = case when @dv <> '' then SUBSTRING(@dv,1, len(@dv)-1) end
	return @dv
END
GO

--clean up data
TRUNCATE TABLE Subscriptiondetails
DELETE Subscriptions
DBCC CHECKIDENT (Subscriptions, 'reseed', 0)
delete canon

--add Demo7 to Canon
ALTER TABLE canon
ADD Demo7 VARCHAR(1) NULL 
GO

--run sql delta comparing Watt to Canon to update canon subscriptions table and sprocs

--Import new canon data
--RDP to .218
--Open Filezilla
--From Connection Manager select Web Query Tool
--in right tab select cognos\cmdb\*.dbf
--in left tab select e:\temp\
--Open SQL Server>Import and Export
--Select FoxPro .dbf datasource type
--select datasource at e:\temp\
--select end datasource as the CanonMasterDB_Test database
--map each file to canon table
--Import the data

--rename canon.pubcode to canon.pubcode_old
--rename canon.pub to canon.pubcode

-- run step 06

--run step 06a

--drop PubId from Subscriptions
ALTER TABLE Subscriptions
DROP Column PubID
GO

--run step 07

--run step 07a

--run step 07b

--run step 07c

--run step 08 ??nothing to do???

--run step 09-10

--change the columns in igrpMasterValues
sp_RENAME 'IgrpMasterValues.MasterFunction', 'Master_Function' , 'COLUMN'
GO 
sp_RENAME 'IgrpMasterValues.MasterBusiness', 'Master_Business' , 'COLUMN'
GO 
sp_RENAME 'IgrpMasterValues.MasterEmploy', 'Master_Employ' , 'COLUMN'
GO 
sp_RENAME 'IgrpMasterValues.MasterIndustry', 'Master_Industry' , 'COLUMN'
GO 
sp_RENAME 'IgrpMasterValues.MasterProduct', 'Master_Product' , 'COLUMN'
GO 
sp_RENAME 'IgrpMasterValues.MasterPurchase', 'Master_Purchase' , 'COLUMN'
GO 
ALTER TABLE IgrpMasterValues
ADD Master_Sector VARCHAR(255) NULL 
GO

--run step 11

--change the columns in CgrpMasterValues
sp_RENAME 'CgrpMasterValues.MasterFunction', 'Master_Function' , 'COLUMN'
GO 
sp_RENAME 'CgrpMasterValues.MasterBusiness', 'Master_Business' , 'COLUMN'
GO 
sp_RENAME 'CgrpMasterValues.MasterEmploy', 'Master_Employ' , 'COLUMN'
GO 
sp_RENAME 'CgrpMasterValues.MasterIndustry', 'Master_Industry' , 'COLUMN'
GO 
sp_RENAME 'CgrpMasterValues.MasterProduct', 'Master_Product' , 'COLUMN'
GO 
sp_RENAME 'CgrpMasterValues.MasterPurchase', 'Master_Purchase' , 'COLUMN'
GO 
ALTER TABLE CgrpMasterValues
ADD Master_Sector VARCHAR(255) NULL 
GO

--run step 12

--run step 13

--run step 14

--compare all values against .198 db




