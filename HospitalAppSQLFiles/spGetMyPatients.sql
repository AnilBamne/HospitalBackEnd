Create Procedure spGetMyPatients
@DoctorId int
As
Begin
	Begin Try
		Begin Transaction
			Select * from PatientTable Where DoctorId=@DoctorId;
			Commit Transaction
	END TRY
	BEGIN CATCH
		-- Transaction uncommittable
		IF (XACT_STATE()) = -1
		ROLLBACK TRANSACTION
 
		-- Transaction committable
		IF (XACT_STATE()) = 1
		COMMIT TRANSACTION
	END CATCH
END;

--Exec spGetMyPatients 1;

