create proc [dbo].[e_CustomerTemplate_Save] 
(
	@CTID 	int,
	@CustomerID  int,
	@TemplateTypeCode	varchar(50),
	@HeaderSource	text,
	@FooterSource	text,
	@IsActive bit,
	@UserID  int
)
as
Begin
			if @CTID = 0 
			Begin
				INSERT INTO CustomerTemplate
				( 
					[CustomerID], [TemplateTypeCode], [HeaderSource], [FooterSource], [IsActive],
					[CreatedUserID],[CreatedDate],[IsDeleted]
				)
				VALUES
				(
					@CustomerID, @TemplateTypeCode, @HeaderSource, @FooterSource, @IsActive,
					@UserID, getdate(),0
				)
				set @CTID = @@IDENTITY
			End
			Else
			Begin
				Update CustomerTemplate
				Set [CustomerID] = @CustomerID, 
					[TemplateTypeCode] = @TemplateTypeCode, 
					[HeaderSource] = @HeaderSource, 
					[FooterSource] = @FooterSource,
					[IsActive] = @IsActive,
					[UpdatedUserID] = @UserID,
					[UpdatedDate] = getdate()
				where
					CTID = @CTID
			End
				select @CTID as ID
End
