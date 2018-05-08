CREATE PROCEDURE [dbo].[e_DomainTrackerUserProfile_Select_DomainTrackerID_ByEmail]
@DomainTrackerID int,
@Email varchar(MAX)
AS

select distinct dtup.* from DomainTrackerUserProfile dtup with (NOLOCK)
join DomainTrackerActivity dta  with (NOLOCK)
on dtup.ProfileID= dta.ProfileID
where dta.DomainTrackerID=@DomainTrackerID and dtup.IsDeleted=0
AND EmailAddress like '%'+@Email+'%'