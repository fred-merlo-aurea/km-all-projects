CREATE proc [dbo].[e_FilterDetails_Save](
@FilterDetailsID int,
@FilterType varchar(50),
@Group varchar(100),
@Name varchar(50),
@Values varchar(MAX),
@SearchCondition varchar(50),
@FilterGroupID int
)
as
BEGIN

	SET NOCOUNT ON
	
	insert into FilterDetails (FilterType, [Group], Name, [Values], SearchCondition, FilterGroupID) 
	values (@FilterType, @Group, @Name, @Values, @SearchCondition, @FilterGroupID)

End