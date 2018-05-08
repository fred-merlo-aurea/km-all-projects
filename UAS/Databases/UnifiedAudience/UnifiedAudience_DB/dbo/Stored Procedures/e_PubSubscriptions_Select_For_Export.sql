CREATE PROCEDURE [dbo].[e_PubSubscriptions_Select_For_Export]
@CurrentPage int,
@PageSize int,
@ProductID int,
@Columns varchar(8000)
AS
BEGIN

	SET NOCOUNT ON

	DECLARE @executeString varchar(8000) = ''
	DECLARE @FirstRec int, @LastRec int
	SELECT @FirstRec = (@CurrentPage - 1) * @PageSize
	SELECT @LastRec = (@CurrentPage * @PageSize + 1);
	
	set @Columns = REPLACE(@Columns,'ps.PubCategoryID', '(SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode WHERE ps.PubCategoryID = CategoryCodeID) as PubCategoryID')
	set @Columns = REPLACE(@Columns,'ps.PubTransactionID','(SELECT TransactionCodeValue FROM UAD_Lookup..TransactionCode WHERE ps.PubTransactionID = TransactionCodeID) as PubTransactionID')
	set @Columns = REPLACE(@Columns, 'ps.RequesterFlag', '(SELECT CodeValue FROM UAD_LookUp..Code WHERE ps.ReqFlag = CodeId) as Req_Flag')
		 	
	SET @executeString = 'WITH TempResult as (

	SELECT DISTINCT ROW_NUMBER() OVER (ORDER BY ps.PubSubscriptionID) as ''RowNum'', ' + @Columns + ' FROM
	PubSubscriptions ps with(nolock) LEFT JOIN SubscriptionPaid sp with(nolock) on sp.PubSubscriptionID = ps.PubSubscriptionID  WHERE PubID = ' + convert(varchar(10),@ProductID) + '
	)
	SELECT top (' + convert(varchar(10),@LastRec-1) + ') *
	FROM TempResult
	WHERE RowNum > ' + convert(varchar(10),@FirstRec) + ' 
	AND RowNum < ' + convert(varchar(10),@LastRec)
	
	exec(@executeString)

END