
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

-- stored procedure for Login --
Alter Procedure spLoginDoctor
@DoctorEmail varchar(200),
@DoctorPassword varchar(200)
As
BEGIN
	BEGIN TRY
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

EXEC spLoginDoctor 'string@gmail.com','string@123';


---patient table   ---
EXEC spRegisterPatient 'abc','abc','abc@gmail.com','abc@123','M','Bnglr','bnglr','Kar','Fever',1;
select * from PatientTable
delete from PatientTable where PatientId>1;
