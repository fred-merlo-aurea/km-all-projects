CREATE PROCEDURE [o_SubscriberConsensusDemographic_Select_SubscriptionID]
@SubscriptionID int
AS
BEGIN

	SET NOCOUNT ON

	--SELECT 
	--	   Upper(MastergroupName) as Name, 
	--	   Upper(DisplayName) as DisplayName,
	--	  STUFF((
	--		SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
	--		FROM [dbo].[SubscriptionDetails] sd1 with(nolock) join Mastercodesheet mc1 with(nolock) on sd1.MasterID = mc1.MasterID  
	--		WHERE (sd1.SubscriptionID = @SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
	--		FOR XML PATH (''))
	--	  ,1,1,'') AS Value
	--	FROM 
	--		(
	--			SELECT distinct  mg.MasterGroupID, mg.Name MasterGroupName, mg.DisplayName
	--		  FROM 
	--				MasterGroups mg  with(nolock)
	--				join Mastercodesheet mc with(nolock) on mg.MasterGroupID = mc.MasterGroupID 
	--		)
	--	 Results
	--	GROUP BY MasterGroupID, MastergroupName, DisplayName
	--	order by Name

	declare @StandardField varchar(255), @CustomField varchar(255)

	Create table #tblAdhoc (DimensionName varchar(255), DimensionValue varchar(max) )

	DECLARE curAdhoc CURSOR LOCAL FAST_FORWARD FOR 
	SELECT CustomField, StandardField 
	FROM SubscriptionsExtensionMapper with(nolock) 
	where Active = 1

	OPEN curAdhoc
	FETCH NEXT FROM curAdhoc INTO @CustomField, @StandardField
	WHILE @@FETCH_STATUS = 0  
		BEGIN 
			EXEC(
					' insert into #tblAdhoc (DimensionValue,DimensionName)
					select s1.' + @StandardField + ' as DimensionValue, ''' + @CustomField + ''' as DimensionName  
					FROM SubscriptionsExtension s1 
					where (s1.' + @StandardField + ' is not null and len(s1.' + @StandardField + ') > 0) and s1.subscriptionID = ' + @SubscriptionID 
				)
			FETCH NEXT FROM curAdhoc INTO @CustomField, @StandardField
		END 
	CLOSE curAdhoc
	DEALLOCATE curAdhoc		

	SELECT Upper(MastergroupName) as Name, 
		Upper(DisplayName) as DisplayName,
		STUFF((
			SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
			FROM [dbo].[SubscriptionDetails] sd1 with(nolock) 
			join Mastercodesheet mc1 with(nolock) on sd1.MasterID = mc1.MasterID  
			WHERE (sd1.SubscriptionID = @SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
			FOR XML PATH (''))
		,1,1,'') AS Value
	FROM 
		(
			SELECT distinct  mg.MasterGroupID, mg.Name MasterGroupName, mg.DisplayName
			FROM MasterGroups mg  with(nolock)
				join Mastercodesheet mc with(nolock) on mg.MasterGroupID = mc.MasterGroupID 
		)
		Results
	GROUP BY MasterGroupID, MastergroupName, DisplayName
	UNION
	select DimensionName as DisplayName,DimensionName as DisplayName, DimensionValue as Value
	from #tblAdhoc
	order by Name
		
	drop table #tblAdhoc

End