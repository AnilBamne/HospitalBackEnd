Create Procedure spGetActiveDoctors
As
Begin
	Begin Try
		Begin Transaction
			Select * from DoctorTable where Status=1;
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


exec spGetActiveDoctors