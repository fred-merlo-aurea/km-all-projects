CREATE PROCEDURE [dbo].[e_DomainTrackerActivity_Save]
@DomainTrackerID int,
@Browser varchar(100),
@IPAddress varchar(50),
@OS varchar(100),
@PageURL varchar(8000),
@ProfileID int,
@ReferralURL varchar(8000),
@SourceBlastID int,
@TimeStamp datetime,
@UserAgent varchar(8000)
AS
	insert into DomainTrackerActivity(DomainTrackerID, Browser, IPAddress, OS,
	 PageURL, ProfileID, ReferralURL,SourceBlastID,[TimeStamp],UserAgent)
	values(@DomainTrackerID, @Browser,@IPAddress, @OS, @PageURL, @ProfileID, @ReferralURL,
	@SourceBlastID, @TimeStamp,	@UserAgent);
	Select  @@IDENTITY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerActivity_Save] TO [ecn5]
    AS [dbo];

