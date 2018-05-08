CREATE proc [dbo].[sp_subscriberLogin_v1]
(
	@GroupID int,
	@subscriberID varchar(50),
	@filter char(1),
	@filterval varchar(10)
)
as
Begin
	if @filter = 'L'
	Begin
		SELECT	e.*, isnull(country,'') as eCountry 
		FROM	
				Emails e join emailgroups eg on e.emailID = eg.emailID left outer join 
				EmailDataValues v on (v.EmailID = e.EmailID) left outer join 
				GroupDatafields g on ( v.GroupDatafieldsID = g.GroupDatafieldsID ) and g.GroupID = eg.groupID 
		WHERE 
				g.shortname = 'SubscriberID' and 
				v.datavalue=@subscriberID and 
				SUBSTRING(LastName, 1, 1) = @filterval AND 
				g.GroupID = @GroupID

	End
	Else if @filter = 'C'
	Begin
		SELECT	e.*, isnull(country,'') as eCountry 
		FROM	
				Emails e join emailgroups eg on e.emailID = eg.emailID left outer join 
				EmailDataValues v on (v.EmailID = e.EmailID) left outer join 
				GroupDatafields g on ( v.GroupDatafieldsID = g.GroupDatafieldsID )  and g.GroupID = eg.groupID 
		WHERE 
				g.shortname = 'SubscriberID' and 
				v.datavalue=@subscriberID and 
				SUBSTRING(Country, 1, 1) = @filterval AND 
				g.GroupID = @GroupID
	End
	Else 
	Begin
		/* 12/05/2007 - Request from IRIS - Dont include state for MPMN and NUTR - temporary */
		/* 12/17/2005 - removed above condition */
		
	/*	if @GroupID = 2852 or @GroupID = 2855
		Begin
			SELECT	e.*, isnull(country,'') as eCountry 
			FROM	
					Emails e left outer join 
					EmailDataValues v on (v.EmailID = e.EmailID) left outer join 
					GroupDatafields g on ( v.GroupDatafieldsID = g.GroupDatafieldsID ) 
			WHERE 
					g.shortname = 'SubscriberID' and 
					v.datavalue = @subscriberID and 
					g.GroupID = @GroupID
		End
		else
		Begin*/
			SELECT	e.*, isnull(country,'') as eCountry --into #tmpsubscriber 
			FROM	
					Emails e join emailgroups eg on e.emailID = eg.emailID left outer join 
					EmailDataValues v on (v.EmailID = e.EmailID) left outer join 
					GroupDatafields g on ( v.GroupDatafieldsID = g.GroupDatafieldsID )  and g.GroupID = eg.groupID 
			WHERE 
					g.shortname = 'SubscriberID' and 
					v.datavalue = @subscriberID and 
					State = @filterval AND 
					g.GroupID = @GroupID
		


/*
End	
		if (SELECT	count(emailID) FROM #tmpsubscriber) = 0
		Begin
			SELECT	e.*, isnull(country,'') as eCountry 
			FROM	
					Emails e left outer join 
					EmailDataValues v on (v.EmailID = e.EmailID) left outer join 
					GroupDatafields g on ( v.GroupDatafieldsID = g.GroupDatafieldsID ) 
			WHERE 
					g.shortname = 'SubscriberID' and 
					v.datavalue = @subscriberID and 
					g.GroupID = @GroupID
		End
		else
		Begin
			select * from #tmpsubscriber
		End	
		drop table #tmpsubscriber		
*/
	End

End
