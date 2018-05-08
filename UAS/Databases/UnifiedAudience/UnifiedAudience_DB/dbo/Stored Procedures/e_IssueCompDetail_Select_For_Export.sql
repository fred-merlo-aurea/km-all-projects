CREATE PROCEDURE [dbo].[e_IssueCompDetail_Select_For_Export]
@IssueID int,
@Columns varchar(max),
@Subs TEXT
AS
BEGIN

	SET NOCOUNT ON

	--DECLARE @Subs varchar(max) = '<XML><S><ID>2004345</ID></S></XML>'
	--DECLARE @Columns varchar(max) = 'ps.[PubSubscriptionID],ps.[FirstName],ps.[LastName],demos.[Pubcode],ps.[QSource],ps.[SequenceID],ps.[ReqFlag],ps.[Batch]'
	--DECLARE @ProductID int = 1	 
	
	CREATE TABLE #Subs
	(  
		RowID int IDENTITY(1, 1)
	  ,[SubID] int
	)
	DECLARE @docHandle int
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @Subs  
	INSERT INTO #Subs 
	SELECT [SubID]
	FROM OPENXML(@docHandle,N'/XML/S')
	WITH
	(
		[SubID] nvarchar(256) 'ID'
	)
	EXEC sp_xml_removedocument @docHandle
	
	set @Columns = REPLACE(@Columns,'ps.[CategoryCode]', '(SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode WHERE ps.PubCategoryID = CategoryCodeID) as CategoryCode')
	set @Columns = REPLACE(@Columns,'ps.[TransactionCode]','(SELECT TransactionCodeValue FROM UAD_Lookup..TransactionCode WHERE ps.PubTransactionID = TransactionCodeID) as TransactionCode')
	set @Columns = REPLACE(@Columns,'ps.[PubCategoryID]', '(SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode WHERE ps.PubCategoryID = CategoryCodeID) as CategoryCode')
	set @Columns = REPLACE(@Columns,'ps.[PubTransactionID]','(SELECT TransactionCodeValue FROM UAD_Lookup..TransactionCode WHERE ps.PubTransactionID = TransactionCodeID) as TransactionCode')
	set @Columns = REPLACE(@Columns, 'ps.[QSource]', 'q.CodeValue as ''QSource''')
	set @Columns = REPLACE(@Columns, 'ps.[Batch]', '(SELECT MAX(BatchNumber) FROM Batch b JOIN History h ON b.BatchID = h.BatchID WHERE h.PubSubscriptionID = ps.PubSubscriptionID) as ''Batch #''')
	set @Columns = REPLACE(@Columns, 'ps.[WaveMailingID]', 'ps.[WaveMailingID], wm.WaveMailingName')
	set @Columns = REPLACE(@Columns, 'ps.[Par3C]', '(SELECT DisplayOrder FROM UAD_Lookup..Code WHERE CodeTypeId = (SELECT CodeTypeId FROM UAD_Lookup..CodeType WHERE CodeTypeName = ''Par3C'') AND CodeId = ps.Par3CID ) as Par3C')
	set @Columns = REPLACE(@Columns, 'ps.[reqflag]', '(SELECT codevalue FROM UAD_Lookup..Code where codeid = ps.reqflag) as ReqFlag')
	set @Columns = REPLACE(@Columns, 'ps.[Exp_Qdate]', 'cast(cast(ps.Qualificationdate as date) as varchar(50)) as ExpQdate')
	set @Columns = REPLACE(@Columns, 'ps.[Qualificationdate]', 'cast(cast(ps.[QualificationDate] as date) as varchar(50)) as QualificationDate')
	
	DECLARE @executeString varchar(8000) = ''
	
	SET @executeString += '
	
	SELECT ' + @Columns +
	' FROM IssueCompDetail ps with(NOLOCK)' +
	' JOIN IssueComp ic with(NOLOCK) ON ic.IssueCompID = ps.IssueCompID' +
	' JOIN Pubs p with(nolock) on ps.PubID = p.PubID' +
	' JOIN #Subs sub ON sub.SubID = ps.IssueCompDetailId'

	IF(CHARINDEX('QSource', @Columns)) > 0
		BEGIN
			set @executeString += ' LEFT JOIN UAD_Lookup..Code q with(NOLOCK) ON q.CodeId = ps.PubQSourceID '
		END
	IF(CHARINDEX('WaveMailingID', @Columns)) > 0
		BEGIN
			set @executeString += ' LEFT JOIN WaveMailing wm with(NOLOCK) ON ps.WaveMailingID = wm.WaveMailingID '
		END
	
	set @executeString += ' WHERE ic.IssueID = ' + convert(varchar(10),@IssueID)
	
	--print(@executeString)
	exec(@executeString)
	DROP TABLE #Subs

END