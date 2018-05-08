CREATE PROCEDURE [dbo].[e_LandingPageAssign_Save]   
@LPAID int,
@LPID int,
@BaseChannelID int,
@CustomerID int,
@CustomerCanOverride bit = NULL,
@CustomerDoesOverride bit = NULL,
@Header varchar(2000) = NULL,
@Footer varchar(2000) = NULL,
@BaseChannelDoesOverride bit = NULL,
@Label varchar(100),
@UserID int

AS
if(@LPAID>0)
BEGIN
	update LandingPageAssign set 
	LPID=@LPID,
	BaseChannelID=@BaseChannelID,
	CustomerID=@CustomerID,
	CustomerCanOverride=@CustomerCanOverride,
	CustomerDoesOverride= @CustomerDoesOverride,
	Header= @Header,
	Footer= @Footer,
	Label= @Label,
	UpdatedUserID= @UserID,
	UpdatedDate= GETDATE(),
	IsDefault=0,
	BaseChannelDoesOverride=@BaseChannelDoesOverride
	where LPAID=@LPAID
	
END
ELSE
BEGIN
	insert into LandingPageAssign(LPID, BaseChannelID, CustomerID, CustomerCanOverride, CustomerDoesOverride,
	Header, Footer , Label, CreatedUserID, CreatedDate, IsDefault,BaseChannelDoesOverride)
	values (@LPID, @BaseChannelID, @CustomerID, @CustomerCanOverride, @CustomerDoesOverride, 
	@Header, @Footer ,@Label,  @UserID, GETDATE(), 0,@BaseChannelDoesOverride)	
	SELECT @LPAID = @@IDENTITY
END

SELECT @LPAID