CREATE procedure [dbo].[dt_Subscribers_Match]
(	
	@PubID int,
	@Firstname varchar(100) = '',
	@Lastname varchar(100) = '',
	@Company varchar(100) = '',
	@Address varchar(255) = '',
	@State varchar(50) = '',
	@zip varchar(50) = '',
	@Phone varchar(100) = '',
	@Email varchar(100) = '',
	@Title varchar(256) = ''
)
as
BEGIN

	set nocount on

	if len(@Firstname) <> 0 and LEN(@Lastname) <> 0
	Begin
		select	s.SubscriptionID, 
				ps.pubsubscriptionID, 
				case when ps.PubSubscriptionID is null then isnull (s.FNAME, '') + ' ' + isnull (s.LName, '') else isnull (ps.FirstName, '') + ' ' + isnull (ps.LastName, '') end as Name, 
				case when ps.PubSubscriptionID is null then s.TITLE else ps.TITLE end as TITLE, 
				case when ps.PubSubscriptionID is null then s.COMPANY else  ps.COMPANY   end as COMPANY, 
				case when ps.PubSubscriptionID is null then 
						isnull(s.Address, '') + ', ' + isnull (s.CITY, '')  + ', ' + isnull (s.STATE, '') + ',' + isnull (s.zip, '') else  
						isnull(ps.Address1, '') + ', ' + isnull (ps.CITY, '')  + ', ' + isnull (ps.RegionCode, '') + ',' + isnull (ps.ZipCode, '') end as ADDRESS,
				case when ps.PubSubscriptionID is null then isnull(s.Email, '') else ps.Email end as EMAIL,
				case when ps.PubSubscriptionID is null then isnull(s.Phone, '') else ps.Phone end as PHONE,
				case when ps.PubSubscriptionID is null then 'UAD' else 'PRODUCT' end as MatchType
		from	Subscriptions s with (NOLOCK) left outer join 
				PubSubscriptions ps  with (NOLOCK) on s.subscriptionID = ps.SubscriptionID and PubID = @PubID
		where	s.FNAME = @Firstname and s.LNAME = @Lastname
	End
End
