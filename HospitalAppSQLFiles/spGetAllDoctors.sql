Create Procedure spGetAllDoctors
As
Begin
	Begin Try
		Begin Transaction
			Select * from DoctorTable;
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

EXEC spGetAllDoctors;
