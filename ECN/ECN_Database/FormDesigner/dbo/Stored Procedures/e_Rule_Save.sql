CREATE PROCEDURE [dbo].[e_Rule_Save]
	@Rule_Seq_ID int = null,
	@Form_Seq_ID int = null,
	@Control_ID int = null,
	@ConditionGroup_Seq_ID int,
	@Type int,
	@Action nvarchar(MAX) = null,
	@ActionJs nvarchar(MAX) = null,
	@UrlToRedirect nvarchar(1792) = null,
	@Order int,
	@ResultType int,
	@NonQualify int = null,
	@SuspendpostDB int = null,
	@Overwritedatapost int = null
AS
	if @Rule_Seq_ID < 0
	BEGIN
		--do an insert
		insert into [Rule](Form_Seq_ID, Control_ID, ConditionGroup_Seq_ID, [Type], [Action],[ActionJs], UrlToRedirect, [Order], ResultType, NonQualify, SuspendpostDB, Overwritedatapost)
		VALUES(@Form_Seq_ID, @Control_ID, @ConditionGroup_Seq_ID, @Type, @Action,@ActionJs,@UrlToRedirect, @Order, @ResultType, @NonQualify, @SuspendpostDB, @Overwritedatapost)
		select @@IDENTITY;
	END
	else
	BEGIN
		Update [rule]
		set Form_Seq_ID = @Form_Seq_ID,
			Control_ID = @Control_ID,
			ConditionGroup_Seq_ID = @ConditionGroup_Seq_ID,
			[Type] = @Type,
			[Action] = @Action,
			[ActionJs]=@ActionJs,
			UrlToRedirect = @UrlToRedirect,
			[Order] = @Order,
			ResultType = @ResultType,
			NonQualify = @NonQualify, 
			SuspendpostDB = @SuspendpostDB,
			Overwritedatapost = @Overwritedatapost
		Where Rule_Seq_ID = @Rule_Seq_ID
		select @Rule_Seq_ID
	END
