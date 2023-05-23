Alter Procedure spLoginPatient
@PatientEmail varchar(200),
@PatientPassword varchar(200)
As
BEGIN
	BEGIN TRY
		DECLARE @Result int;
		IF EXISTS (Select * From PatientTable 
					where @PatientEmail=PatientEmail 
					AND @PatientPassword=PatientPassword)
		BEGIN
			--SET @Result=(select DoctorId from DoctorTable where @DoctorEmail=DoctorEmail AND @DoctorPassword=DoctorPassword);
			PRINT 'Email exist';
			select PatientId from PatientTable where PatientEmail=@PatientEmail AND PatientPassword=@PatientPassword;
			--return @result;
			--print @result
		END
		ELSE
		BEGIN
			--SET @Result='email not found';
			PRINT 'Email not found';
		END
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

EXEC spLoginPatient 'abc@gmail.com','abc@123';
delete from PatientTable where PatientId>1;
select * from DoctorTable
