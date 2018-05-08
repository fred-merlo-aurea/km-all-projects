CREATE proc [dbo].[job_GetBrandWelcomeLetter]
(
@BrandCode varchar(50)
)
as
BEGIN   

	SET NOCOUNT ON 

	declare @currentdate date
	
	declare @eml table (Email varchar(100))
		
	set @currentdate = getdate()
	
	declare @activeEmailSTatusID int
	
	select @activeEmailSTatusID = emailstatusID 
	from EmailStatus 
	where Status = 'Active'

	insert into @eml
	select distinct ps.Email 
	from PubSubscriptions ps with (NOLOCK) 
		join Pubs p with (NOLOCK) on p.PubID = ps.pubID 
		join jobBrandProducts jb on jb.Pubcode = p.PubCode 
		LEFT OUTER JOIN WelcomeHistory wh on wh.BrandCode = jb.BrandCode and ps.EMAIL = wh.EmailAddress
	where LEN(ltrim(rtrim(ps.email))) > 0 and 
		jb.BrandCode =  @BrandCode and 
		ISNULL(emailstatusID, @activeEmailSTatusID) = @activeEmailSTatusID and 
		wh.EmailAddress is null 

	Insert into WelcomeHistory
	select  @BrandCode, Email, GETDATE() from @eml

	select 'lbuchanan@ntmllc.com' as EmailAddress, convert(varchar(10), @currentdate, 101) as DateAdded
	Union
	select 'lroberto@ntmllc.com' as EmailAddress, convert(varchar(10), @currentdate, 101) as DateAdded
	Union
	select Email as EmailAddress, convert(varchar(10), @currentdate, 101) as DateAdded from @eml
	
End