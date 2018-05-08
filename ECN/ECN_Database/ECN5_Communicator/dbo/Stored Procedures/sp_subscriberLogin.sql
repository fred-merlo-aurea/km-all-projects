CREATE proc [dbo].[sp_subscriberLogin]
(
	@GroupID int,
	@subscriberID varchar(50),
	@filter char(1),
	@filterval varchar(25)
)
as
Begin
	declare @SubsciberGDFID int

	select @SubsciberGDFID = groupdatafieldsID from groupdatafields where GroupID = @GroupID and shortname = 'SubscriberID'

	if @filter = 'L'
	Begin
		SELECT	e.*, isnull(country,'') as eCountry , ISNULL(edv.datavalue,'') as subscriberID
		FROM	
				Emails e WITH(NOLOCK) join 
				EmailGroups eg WITH(NOLOCK) on e.emailID = eg.emailID join 
				EmailDataValues edv WITH(NOLOCK)  on (edv.EmailID = e.EmailID)
		WHERE 
				edv.GroupDatafieldsID= @SubsciberGDFID and 
				edv.datavalue=@subscriberID and 
				SUBSTRING(LastName, 1, 1) =  SUBSTRING(@filterval, 1, 1) AND 
				eg.GroupID = @GroupID

	End
	Else if @filter = 'C'
	Begin
		SELECT	e.*, isnull(country,'') as eCountry, ISNULL(edv.datavalue,'') as subscriberID
		FROM	
				Emails e  WITH(NOLOCK) join 
				EmailGroups eg WITH(NOLOCK)  on e.emailID = eg.emailID join 
				EmailDataValues edv WITH(NOLOCK)  on edv.EmailID = e.EmailID 
		WHERE 
				edv.GroupDatafieldsID= @SubsciberGDFID and 
				edv.datavalue=@subscriberID and 
				SUBSTRING(Country, 1, 1) =  SUBSTRING(@filterval, 1, 1) AND 
				eg.GroupID = @GroupID
	End
	Else if @filter = 'U'
	Begin
		SELECT	e.*, isnull(country,'') as eCountry , ISNULL(edv.datavalue,'') as subscriberID
		FROM	
				Emails e WITH(NOLOCK)  join 
				Emailgroups eg WITH(NOLOCK)  on e.emailID = eg.emailID left outer join 
				EmailDataValues edv WITH(NOLOCK)  on (edv.EmailID = e.EmailID) and edv.GroupDatafieldsID= @SubsciberGDFID
		WHERE 
				emailaddress =@subscriberID and 
				[password] = @filterval AND 
				eg.GroupID = @GroupID
	End
	Else if @filter = 'D'
	Begin
		SELECT	e.*, isnull(country,'') as eCountry , ISNULL(edv.datavalue,'') as subscriberID
		FROM	
				Emails e WITH(NOLOCK)  join 
				Emailgroups eg WITH(NOLOCK)  on e.emailID = eg.emailID left outer join 
				EmailDataValues edv WITH(NOLOCK)  on (edv.EmailID = e.EmailID) and edv.GroupDatafieldsID= @SubsciberGDFID
		WHERE 
				emailaddress =@subscriberID and				
				eg.GroupID = @GroupID
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
			SELECT	e.*, isnull(country,'') as eCountry , ISNULL(edv.datavalue,'') as subscriberID 
			FROM	
					Emails e WITH(NOLOCK)  join 
					Emailgroups eg WITH(NOLOCK)  on e.emailID = eg.emailID join 
					EmailDataValues edv WITH(NOLOCK)  on (edv.EmailID = e.EmailID)
			WHERE 
					edv.GroupDatafieldsID= @SubsciberGDFID and 
					edv.datavalue = @subscriberID and 
					[State] = @filterval AND 
					eg.GroupID = @GroupID
	End

End
