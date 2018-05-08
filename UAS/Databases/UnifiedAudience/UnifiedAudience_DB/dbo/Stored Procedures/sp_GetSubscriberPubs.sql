CREATE PROCEDURE [dbo].[sp_GetSubscriberPubs]
		@SubscriptionID int,
		@BrandID int
AS
BEGIN

	SET NOCOUNT ON;

	if @BrandID = 0
		Begin
			select p.PubID, 
				PubCode, 
				PubName, 
				ISNULL(IsCirc, 0) as IsCirc,
				pt.PubTypeID, 
				ColumnReference, 
				p.SortOrder, 
				EmailStatusID, 
				PubSubscriptionID,
				subscriptionID,
				Email,
				Qualificationdate,
				StatusUpdatedDate,
				Address1,
				Address2,
				Address3,
				City, 
				RegionCode, 
				ZipCode, 
				Country, 
				Phone, 
				Fax,
				CountryID,
				FirstName,
				LastName
			from PubTypes pt with(nolock)  
				join Pubs p with(nolock) on pt.PubTypeID = p.PubTypeID 
				join PubSubscriptions ps with(nolock) on p.PubID =  ps.PubID 
			where pt.IsActive = 1 and subscriptionID = @SubscriptionID
			order by pt.SortOrder,
				p.SortOrder,
				p.PubName
		End
	Else
		Begin
			select p.PubID, 
				PubCode, 
				PubName,
				ISNULL(IsCirc, 0) as IsCirc,
				pt.PubTypeID, 
				ColumnReference, 
				p.SortOrder, 
				EmailStatusID, 
				PubSubscriptionID,
				subscriptionID,
				Email,
				Qualificationdate,
				StatusUpdatedDate,
				Address1,
				Address2,
				Address3,
				City, 
				RegionCode, 
				ZipCode, 
				Country, 
				Phone, 
				Fax,
				CountryID,
				FirstName,
				LastName		 
			from PubTypes pt with(nolock)  
				join Pubs p with(nolock) on pt.PubTypeID = p.PubTypeID 
				join PubSubscriptions ps with(nolock) on p.PubID =  ps.PubID 
				join BrandDetails bd with(nolock) on bd.pubID = ps.PubID 
				join Brand b with(nolock) on b.BrandID = bd.BrandID 
			where pt.IsActive = 1 and subscriptionID = @SubscriptionID and bd.BrandID = @BrandID and b.IsDeleted = 0
			order by pt.SortOrder,
				p.SortOrder,
				p.PubName
		End

END