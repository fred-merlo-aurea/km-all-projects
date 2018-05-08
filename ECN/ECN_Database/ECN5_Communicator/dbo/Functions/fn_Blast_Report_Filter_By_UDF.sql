CREATE FUNCTION [dbo].[fn_Blast_Report_Filter_By_UDF] (@blastID int, @UDFname varchar(100), @UDFdata varchar(100) ) 
RETURNS 
		@Results TABLE (emailID int) 
AS     

BEGIN     

	Insert into @Results
	select distinct eal.emailID
	from 
			blast b join 
			emailactivitylog eal on b.blastID = eal.blastID join
			groupdatafields gdf on b.groupID = gdf.groupID and shortname = @UDFname join
			emaildatavalues edv on eal.emailID = edv.emailID and edv.groupdatafieldsID = gdf.groupdatafieldsID
	where 
			b.blastID = @blastID and ACTIONTYPECODE = 'SEND' and datavalue= @UDFdata

	RETURN 
END
