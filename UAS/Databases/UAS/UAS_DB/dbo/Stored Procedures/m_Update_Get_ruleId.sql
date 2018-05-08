create procedure m_Update_Get_ruleId
@ruleId int
as
	begin
		set nocount on
		select *
		from RuleResult with(nolock)
		where RuleId = @ruleId
	end
go
