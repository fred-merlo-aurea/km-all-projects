CREATE PROCEDURE [dbo].[e_CrossTabReport_Save]
@CrossTabReportID int,
@CrossTabReportName varchar(50),
@Row varchar(50),
@Column varchar(50),
@ViewType varchar(50),
@PubID int,
@BrandID int,
@IsDeleted bit,
@CreatedUserID int = 0,
@CreatedDate datetime = null,
@UpdatedUserID int = 0,
@UpdatedDate datetime = null
AS
BEGIN

	SET NOCOUNT ON

	if (@CrossTabReportID > 0)
		begin
			update 
				CrossTabReport 
			set 
				CrossTabReportName = @CrossTabReportName, 
				[Row] = @Row, 
				[Column] = @Column, 
				ViewType = @ViewType, 
				PubID = @PubID, 
				BrandID = @BrandID, 
				IsDeleted = @IsDeleted,
				UpdatedUserID = @UpdatedUserID, 
				UpdatedDate = @UpdatedDate  
			where 
				CrossTabReportID = @CrossTabReportID
			
			select @CrossTabReportID;
		end
	else
		begin
			insert into CrossTabReport (CrossTabReportName, [Row], [Column], ViewType, PubID, BrandID, IsDeleted, CreatedUserID, CreatedDate) 
			values (@CrossTabReportName, @Row, @Column, @ViewType, @PubID,  @BrandID, @IsDeleted, @CreatedUserID, @CreatedDate)

			select @@IDENTITY;
		end

END