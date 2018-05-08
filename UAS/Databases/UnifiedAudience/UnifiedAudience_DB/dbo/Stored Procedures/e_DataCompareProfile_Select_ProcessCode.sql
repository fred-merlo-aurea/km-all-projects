CREATE PROCEDURE [dbo].[e_DataCompareProfile_Select_ProcessCode]
@processCode varchar(50)
AS
BEGIN

	set nocount on

	select *
	from DataCompareProfile with(nolock)
	where processCode = @processCode

END

