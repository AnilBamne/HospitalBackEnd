-- stored procedure for registration --
Create Procedure spRegisterDoctor
@DoctorName varchar(200),
@DoctorGender varchar(50),
@DoctorSpecialization varchar(200),
@DoctorNumber int,
@DoctorEmail varchar(200),
@DoctorPassword varchar(200)
As
BEGIN
	BEGIN TRY
		DECLARE @Result VARCHAR='';
		IF EXISTS (Select * From DoctorTable Where DoctorEmail=@DoctorEmail)
		BEGIN
			SET @Result='Email already exist';
			PRINT 'Email already exist';
		END
		ELSE
		BEGIN
			Insert Into DoctorTable(DoctorName,DoctorGender,DoctorSpecialization,DoctorNumber,DoctorEmail,DoctorPassword)
			Values(@DoctorName,@DoctorGender,@DoctorSpecialization,@DoctorNumber,@DoctorEmail,@DoctorPassword);

			Select * from DoctorTable
			SET @Result='Doctor details saved succesfully';
			PRINT @result;
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
	END CATCH
END;

EXEC spRegisterDoctor 'abc','M','heartSpecialist',8888,'abc@gmail.com','abc@123';

select * from DoctorTable;