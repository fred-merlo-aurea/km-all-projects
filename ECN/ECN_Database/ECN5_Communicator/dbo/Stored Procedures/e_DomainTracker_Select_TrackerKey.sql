CREATE PROC dbo.e_DomainTracker_Select_TrackerKey
(
	@TrackerKey varchar(200)
)
AS 
BEGIN
		SELECT * FROM DomainTracker cd WITH (NOLOCK) 
		WHERE cd.TrackerKey=@TrackerKey and cd.IsDeleted=0
END