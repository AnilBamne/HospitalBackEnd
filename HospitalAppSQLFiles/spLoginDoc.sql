-- stored procedure for Login --
Alter Procedure spLoginDoctor
@DoctorEmail varchar(200),
@DoctorPassword varchar(200)
As
BEGIN
	BEGIN TRY
		Begin Tran
			If ((select Status from DoctorTable Where DoctorEmail=@DoctorEmail)=1)
			Begin
				DECLARE @Result int;
				IF EXISTS (Select * From DoctorTable 
						where @DoctorEmail=DoctorEmail 
						AND @DoctorPassword=DoctorPassword)
				BEGIN
				--SET @Result=(select DoctorId from DoctorTable where @DoctorEmail=DoctorEmail AND @DoctorPassword=DoctorPassword);
					PRINT 'Email exist';
					select DoctorId from DoctorTable where @DoctorEmail=DoctorEmail AND @DoctorPassword=DoctorPassword;
					return @result;
					print @result
				END
				ELSE
				BEGIN
					--SET @Result='email not found';
					PRINT 'Email not found';
				END
			End
			Else
			Begin
				print 'You do not have access to login';
			End
		Commit Tran
	END TRY
	BEGIN CATCH
		SELECT  
		
            ERROR_NUMBER() AS ErrorNumber  
            ,ERROR_SEVERITY() AS ErrorSeverity  
            ,ERROR_STATE() AS ErrorState  
            ,ERROR_PROCEDURE() AS ErrorProcedure  
            ,ERROR_LINE() AS ErrorLine  
            ,ERROR_MESSAGE() AS ErrorMessage;
			--print 'exception occured';
	END CATCH
END;

EXEC spLoginDoctor 'Sample@gmail.com','U2FtcGxlQDEyMw==';
select * from DoctorTable