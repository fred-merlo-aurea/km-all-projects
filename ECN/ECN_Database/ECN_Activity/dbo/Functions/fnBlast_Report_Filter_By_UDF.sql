CREATE FUNCTION [dbo].[fnBlast_Report_Filter_By_UDF] (@blastID int, @UDFname varchar(100), @UDFdata varchar(100) ) 
RETURNS 
		@Results TABLE (emailID int) 
AS     

BEGIN     

	Insert into @Results
	select distinct bas.emailID
	from 
			ecn5_communicator..blast b WITH (NOLOCK) join 
			BlastActivitySends bas WITH (NOLOCK) on b.blastID = bas.blastID join
			ecn5_communicator..groupdatafields gdf WITH (NOLOCK) on b.groupID = gdf.groupID and shortname = @UDFname join
			ecn5_communicator..emaildatavalues edv WITH (NOLOCK) on bas.emailID = edv.emailID and edv.groupdatafieldsID = gdf.groupdatafieldsID
	where 
			b.blastID = @blastID and datavalue= @UDFdata and b.StatusCode <> 'Deleted' and gdf.IsDeleted = 0

	RETURN 
END