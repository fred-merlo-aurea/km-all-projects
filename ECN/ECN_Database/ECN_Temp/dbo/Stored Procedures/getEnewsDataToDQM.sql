-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getEnewsDataToDQM]  
	-- Add the parameters for the stored procedure here
	@ClientName varchar(50) ,
	@Deltastartdate date = null
AS
BEGIN
	declare @sqlString nvarchar(4000);
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

     SET @ClientName = UPPER(@ClientName);
     set @sqlString = 'SELECT * FROM ' +  'MAF_' + @ClientName + '_Enews';
     
--if @Deltastartdate is not null then it is delta refresh and we need to pull all records on or after the given date
     if (@Deltastartdate is not null)
		set @sqlString = @sqlString + ' where (Createdon >= ''' +  convert(varchar(10),@Deltastartdate,101) + ''' or LastChanged >= ''' +  convert(varchar(10),@Deltastartdate,101) + ''')';
--if @Deltastartdate is null then it is FULL refresh and we need to pull only records with SubscribeTypeCode ='S'
     if (@Deltastartdate is null)
		set @sqlString = @sqlString + ' where SubscribeTypeCode = ''S''';

    
	 --DECLARE	@return_value int
     --EXEC @return_value = [ECN5_COMMUNICATOR].[dbo].[getEnewsDataForDQM] @clientName = @ClientName;
	 exec(@sqlString);   
	 --print(@sqlString); 
	
END
