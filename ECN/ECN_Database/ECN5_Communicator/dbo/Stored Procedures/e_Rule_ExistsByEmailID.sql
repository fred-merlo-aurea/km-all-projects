CREATE PROCEDURE [dbo].[e_Rule_ExistsByEmailID] 
	@RuleID int,
	@GroupID int,
	@EmailID int
AS     
BEGIN 
	

DECLARE @WhereClause VARCHAR(2000),
		@StandAloneUDFs VARCHAR(2000), 
		@standAloneTempQuery VARCHAR(2000)

select @WhereClause=r.WhereClause from [rule]  r where RuleID=@RuleID
SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) 
WHERE g.DatafieldSetID is null and g.IsDeleted = 0 and g.GroupID=@GroupID ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

	set @standAloneTempQuery = '
	SELECT * into #tempStandAlone
	 FROM
	 (
		SELECT gdf.ShortName, edv.DataValue, e.*
		from	EmailDataValues edv WITH (NOLOCK) 
		join groupdatafields gdf WITH (NOLOCK) 
		on edv.groupdatafieldsID = gdf.groupdatafieldsID
		join Emails e WITH (NOLOCK) 
		on e.emailID = edv.emailID
		where 
				gdf.GroupID =' + CONVERT(varchar,@GroupID) + ' and gdf.IsDeleted = 0 and edv.EmailID= ' +  CONVERT(varchar,@EmailID) + '
	 ) u
	 PIVOT
	 (
	 MAX (DataValue)
	 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt;
	 
	 CREATE INDEX IDX_tempStandAlone_EmailID ON #tempStandAlone(EmailID);
	 IF EXISTS (SELECT TOP 1 EmailID from  #tempStandAlone where '+@WhereClause+') SELECT 1 ELSE SELECT 0;'		
			
     exec (@standAloneTempQuery)
			

END