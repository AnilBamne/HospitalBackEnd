Alter Procedure spRegisterPatient
@PFirstName varchar(200),
@PLastName varchar(200),
@PEmail varchar(200),
@PPassword varchar(200),
@PGender varchar(50),
@PAddress varchar(240),
@PCity varchar(200),
@PState varchar(200),
@PDesies varchar(200)
--@DoctorId int
As
BEGIN
	BEGIN TRY
		DECLARE @Result VARCHAR='';
		IF EXISTS (Select * From PatientTable Where PatientEmail=@PEmail)
		BEGIN
			SET @Result='Email already exist';
			PRINT 'Email already exist';
			return @Result;
		END
		ELSE
		BEGIN
			Insert Into PatientTable(patientFirstName,patientLastName,PatientEmail,PatientPassword,PatientGender,PatientAddress,PatientCity,PatientState,PatientDesies)
			Values(@PFirstName,@PLastName,@PEmail,@PPassword,@PGender,@PAddress,@PCity,@PState,@PDesies);

			Select * from PatientTable
			SET @Result='Patient details saved succesfully';
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

select * from DoctorTable
