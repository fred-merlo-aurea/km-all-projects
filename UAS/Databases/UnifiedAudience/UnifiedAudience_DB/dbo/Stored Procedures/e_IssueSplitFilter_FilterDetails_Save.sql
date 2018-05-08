CREATE PROCEDURE e_IssueSplitFilter_FilterDetails_Save
	@FilterID int,
	@PubId int,
	@FilterName varchar(512),
	@CreatedByUserID int=NULL,
	@UpdatedByUserID int= NULL,
    @TVP_IssueSplitFilterDetails IssueFilterDetailsType READONLY 
AS
BEGIN
	SET NOCOUNT ON
	IF (@FilterID > 0)
		BEGIN
			 UPDATE IssueSplitFilter
				SET FilterName = @FilterName,
				PubId = @PubId,
				DateUpdated = GETDATE(),
				CreatedByUserID = @CreatedByUserID,
				UpdatedByUserID = @UpdatedByUserID
			 WHERE FilterID = @FilterID
			 
		     DELETE FROM IssueSplitFilterDetails WHERE FilterID = @FilterID
		     
			 INSERT INTO IssueSplitFilterDetails(FilterID,[Name] ,[Values] ,[SearchCondition],[Group],[Text]) 
             SELECT @FilterID,[Name] ,[Values] ,[SearchCondition],[Group],[Text]
		     FROM  @TVP_IssueSplitFilterDetails 
		END	
	ELSE
		BEGIN
			INSERT INTO IssueSplitFilter (FilterName,PubId,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID ) 
			VALUES (@FilterName,@PubId,GETDATE(),null,@CreatedByUserID,null)
			SET @FilterID = @@IDENTITY 
			
			INSERT INTO IssueSplitFilterDetails(FilterID,[Name] ,[Values] ,[SearchCondition],[Group],[Text]) 
            SELECT @FilterID,[Name] ,[Values] ,[SearchCondition],[Group],[Text]  
		    FROM  @TVP_IssueSplitFilterDetails
		END	
	SELECT @FilterID;
END